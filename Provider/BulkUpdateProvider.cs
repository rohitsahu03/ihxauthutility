using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Interfaces;
using AuthUtility.Models;
using AuthUtility.Models.Elastic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthUtility.Provider
{
    public class BulkUpdateProvider : IBulkUpdateProvider
    {
        private readonly IDBRepo _dbRepo;
        private readonly ISyncHelper _syncHelper;
        private readonly IUtilityRepo _utilityRepo;
        private readonly ILogger _logger;
        private readonly ConfigManager _configManager;
        private readonly IRestClient _restClient;
        public BulkUpdateProvider(IDBRepo dbRepo, ISyncHelper syncHelper,
                                  IUtilityRepo utilityRepo, ILogger<BulkUpdateProvider> logger,
                                  ConfigManager configManager, IRestClient restClient)
        {
            _dbRepo = dbRepo;
            _syncHelper = syncHelper;
            _utilityRepo = utilityRepo;
            _logger = logger;
            _configManager = configManager;
            _restClient = restClient;
        }
        public BaseResponse UpdateBulkUserDetails(UserDetailsBulkUpdate Model)
        {
            if (Model == null || !Model.UserDetails.HasRecords())
                return new BaseResponse() { Message = MsgConstants.NoDataFound };

            List<string> newUserNames = Model.UserDetails.Where(x => !string.IsNullOrEmpty(x.NewUserName)).Select(x => x.NewUserName).ToList();
            if (newUserNames.HasRecords())
            {
                List<ApplicationUser> usersWithNewUserNames = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => newUserNames.Contains(x.TAU_LoginName));
                if (usersWithNewUserNames.HasRecords())
                    return new BaseResponse() { Message = string.Format(MsgConstants.UsersAlreadyPresent, string.Join(",", usersWithNewUserNames.Select(x => x.TAU_LoginName))) };
            }

            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>
                        (x => Model.UserDetails.Where(y => !string.IsNullOrEmpty(y.OldUserName)).Select(y => y.OldUserName).ToList().Contains(x.TAU_LoginName)
                        || Model.UserDetails.Where(z => z.UserId != 0).Select(z => z.UserId).ToList().Contains(x.TAU_Id));

            var invalidUserIds = FindIfUserIdIsValid(Model.UserDetails.Where(x => x.UserId != 0 && !string.IsNullOrEmpty(x.OldUserName)).ToDictionary(x => x.UserId, x => x.OldUserName), users);
            if (invalidUserIds.HasRecords())
                return new BaseResponse() { Message = string.Format(MsgConstants.UserIdUserNameMismatch, string.Join(",", invalidUserIds.Keys)) };

            Model.UserDetails
           .Where(x => x.UserId == 0 && !string.IsNullOrEmpty(x.OldUserName) && !string.IsNullOrEmpty(x.NewUserName))
           .ToList()
           .ForEach(x => x.UserId = users.Where(y => y.TAU_LoginName.Trim().ToLower() == x.OldUserName.Trim().ToLower()).Select(z => z.TAU_Id).FirstOrDefault());

            BaseResponse response = _dbRepo.UpdateUserDetailsInBulk(Model);
            if (response != null && response.IsSuccess)
            {
                Task.Factory.StartNew(() =>
                {
                    List<int> userIds = Model.UserDetails.Select(x => x.UserId).OrderBy(x => x).ToList();
                    SyncToElasticWithPagination(userIds);
                });
            }
            return response;
        }

        public BaseResponse UserBulkCreation(List<UserCreation> Model, bool IsVerifyContact, int ActionPerformedBy)
        {
            if (!Model.HasRecords())
                return new BaseResponse() { Message = MsgConstants.RequestCannotBeNull };

            List<string> notCreatedUsers = new List<string>();
            List<int> useridsOfcreatedUsers = new List<int>();
            foreach (var item in Model)
            {
                //string absolutePath = item.ProviderMasterEntityId == UtilityConstants.RetailEntityId ? APIConstants.CreateRetaileUser : APIConstants.CreateCorporateUser;
                UserCreationResponse userCreationResponse = _utilityRepo.CreateUser(item, APIConstants.CreateRetaileUser);
                if (userCreationResponse.IsNull() || !userCreationResponse.IsSuccess)
                {
                    notCreatedUsers.Add(item.UserName + "-" + (userCreationResponse.IsNull() ? "Null response from API" : userCreationResponse.ErrorMessage));
                    if (_logger.IsNotNull())
                        _logger.LogError("User Creation Failed for : " + JsonConvert.SerializeObject(item));
                }
                else
                    useridsOfcreatedUsers.Add(userCreationResponse.UserId);
            }
            if (IsVerifyContact && useridsOfcreatedUsers.HasRecords())
                Task.Factory.StartNew(() => _dbRepo.UpdateIsOTPVerified(useridsOfcreatedUsers, true, ActionPerformedBy, true, true));

            if (notCreatedUsers.HasRecords())
                return new BaseResponse() { Message = string.Format(MsgConstants.FailedForUsers, string.Join(",", notCreatedUsers)) };
            else
                return new BaseResponse() { IsSuccess = true };
        }

        public void SyncToElasticWithPagination(List<int> UserIds)
        {
            if (!UserIds.HasRecords())
                return;

            int iteration = 1;
            int size = 500;
            do
            {
                _syncHelper.SyncToElastic(string.Join(",", UserIds.Skip((iteration - 1) * size).Take(size)));
                iteration++;

            } while (UserIds.Count > (iteration - 1) * size);
        }

        private Dictionary<int, string> FindIfUserIdIsValid(Dictionary<int, string> KeyValuePair, List<ApplicationUser> Users)
        {
            if (!KeyValuePair.HasRecords() || !Users.HasRecords())
                return null;

            Dictionary<int, string> response = new Dictionary<int, string>();
            foreach (var item in KeyValuePair)
            {
                ApplicationUser presentUser = Users.Where(x => x.TAU_Id == item.Key).FirstOrDefault();
                if (presentUser.IsNull() || presentUser.TAU_LoginName.IsNull() || presentUser.TAU_LoginName.ToLower().Trim() != item.Value.ToLower().Trim())
                    response.Add(item.Key, item.Value);
            }
            return response;
        }

        public BaseResponse RemoveElasticBaseProfile(RemoveElasticBaseProfile Model)
        {
            if (Model.IsNull() || !Model.IsValid() || _configManager.IsStaging)
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            //try
            //{
            //    ElasticDeleteRequest requestObj = new ElasticDeleteRequest()
            //    {
            //        query = new Query()
            //        {
            //            match = new Match() { EntityId = Model.EntityId }
            //        }
            //    };

            //    var restResponse = _restClient.MakePostRestCall<ElasticDeleteRequest, ElasticDeleteResponse>(requestObj, "/authbaseuserprofile/baseprofile/_delete_by_query", _configManager.ElasticUrl, true);

            //    //for testing
            //    //var restResponse = _restClient.MakePostRestCall<ElasticDeleteRequest, ElasticDeleteResponse>(requestObj, "/deletetest/baseprofile/_delete_by_query", "http://192.168.1.124:9200/", true);

            //    if (restResponse.IsNotNull())
            //    {
            //        return new BaseResponse()
            //        {
            //            IsSuccess = restResponse.deleted != 0,
            //            ErrorMessage = "Total => " + restResponse.total + " ,Deleted => " + restResponse.deleted
            //        };
            //    }
            //    else
            //        return new BaseResponse() { ErrorMessage = MsgConstants.NullResponse_RestCall };
            //}
            //catch (Exception Ex)
            //{
            //    return new BaseResponse() { ErrorMessage = Ex.Message };
            //}
            return default;
        }
    }
}
