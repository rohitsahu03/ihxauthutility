using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Enums;
using AuthUtility.Interfaces;
using AuthUtility.Mapper;
using AuthUtility.Models;
using AuthUtility.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthUtility.Repository
{
    public class DBProvider : IDBProvider
    {
        private readonly ConfigManager _configManager;
        private readonly IDBRepo _dbRepo;
        private readonly ISyncHelper _syncHelper;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUtilityRepo _utilityRepo;
        private readonly ICacheProvider _cacheProvider;
        public DBProvider(ILogger<DBProvider> logger, IDBRepo dbRepo, ISyncHelper syncHelper, ConfigManager ConfigManager, IHttpContextAccessor httpContextAccessor, IUtilityRepo utilityRepo, ICacheProvider cacheProvider)
        {
            this._dbRepo = dbRepo;
            this._syncHelper = syncHelper;
            this._logger = logger;
            this._configManager = ConfigManager;
            this._httpContextAccessor = httpContextAccessor;
            this._utilityRepo = utilityRepo;
            this._cacheProvider = cacheProvider;
        }
        public BaseResponse ChangeEmail(UpdateEmail Model)
        {
            try
            {
                if (Model == null || !Model.Validate())
                    return new BaseResponse() { Message = MsgConstants.InvalidData };

                var isPresent = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_IsActive && x.TAU_EmailId == Model.EmailId);
                if (isPresent != null && isPresent.Count > 0)
                    return new BaseResponse() { Message = MsgConstants.AlreadyPresent };

                Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
                exp = Model.IsUserId == true ? exp.And(x => x.TAU_Id == int.Parse(Model.UserIdORUserName)) : exp.And(x => x.TAU_LoginName == Model.UserIdORUserName);
                ApplicationUser user = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp)?.FirstOrDefault();
                if (user.IsNull())
                    return new BaseResponse() { Message = MsgConstants.UserNotFound };

                user.TAU_EmailId = Model.EmailId;
                user.Modifiedon = DateTime.Now;
                user.TAU_Modifiedby = Model.ActionPerformedBy;

                bool isSuccess = _dbRepo.UpdateInDB<ApplicationUser>(user);
                return new BaseResponse() { IsSuccess = isSuccess, Message = isSuccess ? MsgConstants.SuccessMsg : MsgConstants.ErrorMsg };
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ChangeEmail:" + ex.Message + " " + JsonConvert.SerializeObject(Model) + Environment.NewLine);
                return default;
            }
        }
        public BaseResponse UpdateFirstLastName(UpdateFirstLastName Model)
        {
            try
            {
                if (Model == null || !Model.Validate())
                    return new BaseResponse() { Message = MsgConstants.InvalidData };

                Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
                exp = Model.IsUserId == true ? exp.And(x => x.TAU_Id == int.Parse(Model.UserIdORUserName)) : exp.And(x => x.TAU_LoginName == Model.UserIdORUserName);
                ApplicationUser user = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp)?.FirstOrDefault();
                if (user.IsNull())
                    return new BaseResponse() { Message = MsgConstants.UserNotFound };

                user.TAU_FirstName = Model.FirstName;
                user.TAU_LastName = Model.LastName;
                user.Modifiedon = DateTime.Now;
                user.TAU_Modifiedby = Model.ActionPerformedBy;

                bool isSuccess = _dbRepo.UpdateInDB<ApplicationUser>(user);
                return new BaseResponse() { IsSuccess = isSuccess, Message = isSuccess ? MsgConstants.SuccessMsg : MsgConstants.ErrorMsg };
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ChangeEmail:" + ex.Message + " " + JsonConvert.SerializeObject(Model) + Environment.NewLine);
                return default;
            }
        }

        public BaseResponse ChangeMob(UpdateMob Model)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if (Model == null || !Model.Validate())
                    return new BaseResponse() { Message = MsgConstants.InvalidData };

                var isPresent = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_IsActive && x.TAU_PhoneNumber == Model.MobNo);
                if (isPresent != null && isPresent.Count > 0)
                    return new BaseResponse() { Message = MsgConstants.AlreadyPresent };

                Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
                exp = Model.IsUserId == true ? exp.And(x => x.TAU_Id == int.Parse(Model.UserIdORUserName)) : exp.And(x => x.TAU_LoginName == Model.UserIdORUserName);
                ApplicationUser user = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp)?.FirstOrDefault();
                if (user.IsNull())
                    return new BaseResponse() { Message = MsgConstants.UserNotFound };

                user.TAU_PhoneNumber = Model.MobNo;
                user.Modifiedon = DateTime.Now;
                user.TAU_Modifiedby = Model.ActionPerformedBy;

                bool isSuccess = _dbRepo.UpdateInDB<ApplicationUser>(user);
                return new BaseResponse() { IsSuccess = isSuccess, Message = isSuccess ? MsgConstants.SuccessMsg : MsgConstants.ErrorMsg };
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ChangeMob:" + ex.Message + " " + JsonConvert.SerializeObject(Model) + Environment.NewLine);
            }
            return response;
        }

        public BaseResponse ChangePassword(UpdatePassword Model)
        {
            if (Model == null || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
            exp = Model.IsUserId == true ? exp.And(x => x.TAU_Id == int.Parse(Model.UserIdORUserName)) : exp.And(x => x.TAU_LoginName == Model.UserIdORUserName);
            ApplicationUser user = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp)?.FirstOrDefault();
            if (user.IsNull())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };

            return _dbRepo.UpdatePasswordAuth(new List<int> { user.TAU_Id }, Model.Password, Model.ActionPerformedBy);
        }

        public BaseResponse UpdateLoginname(UpdateLoginname Model)
        {
            BaseResponse response = new BaseResponse();
            if (Model == null || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            var isPresent = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_IsActive && x.TAU_LoginName == Model.LoginName);
            if (isPresent != null && isPresent.Count > 0)
                return new BaseResponse() { Message = MsgConstants.AlreadyPresent };

            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
            exp = Model.IsUserId == true ? exp.And(x => x.TAU_Id == int.Parse(Model.UserIdORUserName)) : exp.And(x => x.TAU_LoginName == Model.UserIdORUserName);
            ApplicationUser user = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp)?.FirstOrDefault();
            if (user.IsNull())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };

            user.TAU_LoginName = Model.LoginName;
            user.Modifiedon = DateTime.Now;
            user.TAU_Modifiedby = Model.ActionPerformedBy;

            bool isSuccess = _dbRepo.UpdateInDB<ApplicationUser>(user);
            return new BaseResponse() { IsSuccess = isSuccess, Message = isSuccess ? MsgConstants.SuccessMsg : MsgConstants.ErrorMsg };
        }

        public BaseResponse UpdateEntityId(UpdateEntityId Model)
        {
            BaseResponse response = new BaseResponse();
            if (Model == null || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            if (string.IsNullOrEmpty(_dbRepo.CheckValidityOfEntityId(int.Parse(Model.NewProviderMasterEntityId))))
                return new BaseResponse() { Message = MsgConstants.InvalidEntityId };

            var values = Model.UserIdsORUserNames.Split(",");
            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
            exp = Model.IsUserId == true ? exp.And(x => values.Select(y => int.Parse(y)).Contains(x.TAU_Id)) : exp.And(x => values.Contains(x.TAU_LoginName));
            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp);
            if (!users.HasRecords())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };

            string query = string.Format(SqlQueries.ChangeProviderMasterEntityId, string.Join(",", users.Select(x => x.TAU_Id)), Model.OldProviderMasterEntityId, Model.NewProviderMasterEntityId, Model.ActionPerformedBy);
            return _dbRepo.SaveInDBWithQuery(query);
        }

        public BaseResponse UnlockUsers(UnlockUsers Model)
        {
            if (Model == null || string.IsNullOrEmpty(SqlQueries.UnlockUsers) || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            var values = Model.UserIdsORUserNames.Split(",");
            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
            exp = Model.IsUserId == true ? exp.And(x => values.Select(y => int.Parse(y)).Contains(x.TAU_Id)) : exp.And(x => values.Contains(x.TAU_LoginName));
            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp);
            if (!users.HasRecords())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };
            return _dbRepo.SaveInDBWithQuery(string.Format(SqlQueries.UnlockUsers, Model.ActionPerformedBy, string.Join(",", users.Select(x => x.TAU_Id))));
        }

        public BaseResponse ChangeActiveStatus(UpdateActiveStatus Model)
        {
            if (Model == null || string.IsNullOrEmpty(SqlQueries.UpdateActiveStatusAuth) || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            int bitvalue = bool.Parse(Model.Status) ? 1 : 0;
            var values = Model.UserIdsORUserNames.Split(",");
            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_Id != default;
            exp = Model.IsUserId == true ? exp.And(x => values.Select(y => int.Parse(y)).Contains(x.TAU_Id)) : exp.And(x => values.Contains(x.TAU_LoginName));
            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp);
            if (!users.HasRecords())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };

            return _dbRepo.SaveInDBWithQuery(string.Format(SqlQueries.UpdateActiveStatusAuth, bitvalue, Model.ActionPerformedBy, string.Join(",", users.Select(x => x.TAU_Id))));
        }

        public BaseResponse UpdateIsOTPVerified(UpdateIsOTPVerified Model)
        {
            BaseResponse response = new BaseResponse();
            if (Model == null || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            var values = Model.UserIdsORUserNames.Split(",");
            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
            exp = Model.IsUserId == true ? exp.And(x => values.Select(y => int.Parse(y)).Contains(x.TAU_Id)) : exp.And(x => values.Contains(x.TAU_LoginName));
            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp);
            if (!users.HasRecords())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };

            return _dbRepo.UpdateIsOTPVerified(users.Select(x => x.TAU_Id).ToList(), bool.Parse(Model.Status), Model.ActionPerformedBy, Model.IsEmailVerified, Model.IsOTPVerified);
        }

        public BaseResponse UpdateRolesinAuth(UpdateRole Model)
        {
            if (Model == null || string.IsNullOrEmpty(SqlQueries.MergeQueryRole) || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            var values = Model.UserIdsORUserNames.Split(",");
            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
            exp = Model.IsUserId == true ? exp.And(x => values.Select(y => int.Parse(y)).Contains(x.TAU_Id)) : exp.And(x => values.Contains(x.TAU_LoginName));
            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp);
            if (!users.HasRecords())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };

            return _dbRepo.UpdateRolesinAuth(users.Select(x => x.TAU_Id).ToList(), Model.SelectedRolesIds, Model.AllRoles, Model.DisablePreviousRoles, Model.ActionPerformedBy);
        }
        public BaseResponse UpdatePasswordBulk(UpdatePasswordBulk Model)
        {
            if (Model == null || !Model.Validate())
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            var values = Model.UserIdsORUserNames.Split(",");
            Expression<Func<ApplicationUser, bool>> exp = x => x.TAU_IsActive;
            exp = Model.IsUserId == true ? exp.And(x => values.Select(y => int.Parse(y)).Contains(x.TAU_Id)) : exp.And(x => values.Contains(x.TAU_LoginName));
            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(exp);
            if (!users.HasRecords())
                return new BaseResponse() { Message = MsgConstants.UserNotFound };

            return _dbRepo.UpdatePasswordAuth(users.Select(x => x.TAU_Id).ToList(), Model.Password, Model.ActionPerformedBy);
        }
        public Dictionary<string, string> GetAllRoles(bool isFetchGroupKey = false)
        {
            return _cacheProvider.GetAuthRoles(isFetchGroupKey);
        }

        public BaseResponse UpdateFromCMS(UpdateFromCMS Model)
        {
            BaseResponse response = new BaseResponse();
            if (Model == null || string.IsNullOrEmpty(SqlQueries.GetDbNameByPolId) || string.IsNullOrEmpty(SqlQueries.MergeQueryProperty) || string.IsNullOrEmpty(SqlQueries.GetPropertiesFromCMS))
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            response = _dbRepo.UpdateFromCMS(Model);

            if (response != null && response.IsSuccess)
                Task.Factory.StartNew(() => _syncHelper.SyncToElastic(Model.TauIds));
            return response;
        }

        public BaseResponse CreateBrokerLogin(BrokerLogin Model)
        {
            if (Model == null || string.IsNullOrEmpty(SqlQueries.GetAllBrokers) || string.IsNullOrEmpty(SqlQueries.sp_brokerLogin))
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            var brokers = _dbRepo.GetAllBrokers();
            if (brokers == null || brokers.Keys.Count == 0)
                return new BaseResponse() { Message = MsgConstants.ErrorInGettingBrokers };

            if (brokers.Keys.Count > 0 && !brokers.Keys.Contains(int.Parse(Model.BrokerId)))
                return new BaseResponse() { Message = MsgConstants.InvalidBroker };

            return _dbRepo.CreateBrokerLogin(Model);
        }

        public BaseResponse UpdateProclaimRole(UpdateProclaimRole Model)
        {
            if (Model == null || string.IsNullOrEmpty(SqlQueries.MergeQueryProclaimRole))
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            return _dbRepo.UpdateProclaimRolesinAuth(Model);
        }


        public BaseResponse UpdateShortName(UpdateShortName Model)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                if (Model == null || string.IsNullOrEmpty(SqlQueries.ChangeCorporateAlias) || !(int.Parse(Model.ProviderMasterEntityId) > 0))
                    return new BaseResponse() { Message = MsgConstants.InvalidData };

                Model.OldShortName = Model.OldShortName.Trim('@').Trim();
                Model.NewShortName = Model.NewShortName.Trim('@').Trim();

                int DuplicateUsers = _dbRepo.GetDuplicateUserNames(string.Format(SqlQueries.GetDuplicateUserNames, Model.ProviderMasterEntityId, Model.OldShortName, Model.NewShortName));
                if (DuplicateUsers > 0)
                    return new BaseResponse() { Message = string.Format(MsgConstants.DuplicateUsersError, DuplicateUsers) };

                // AliasMapping aliasMapping = _dbRepo.GetAliasMapping(int.Parse(Model.ProviderMasterEntityId));
                //if (aliasMapping != null && aliasMapping.CAM_LoginType != UtilityConstants.EmpIdLoginType || aliasMapping.CAM_Suffix == null || aliasMapping.CAM_Suffix.ToLower() != ("@" + Model.OldShortName).ToLower())
                //    return new BaseResponse() { ErrorMessage = UtilityConstants.IncorrectMapping };

                BaseResponse aliasUpdateResult = _dbRepo.SaveInDBWithQuery(string.Format(SqlQueries.SaveAlias, int.Parse(Model.ProviderMasterEntityId), Model.NewShortName, Model.ActionPerformedBy));
                if (aliasUpdateResult == null || !aliasUpdateResult.IsSuccess)
                    return new BaseResponse() { Message = MsgConstants.ErrorInSavingAlias };

                if (!string.IsNullOrEmpty(Model.PolId))
                {
                    string dbName = _dbRepo.GetDbNameByPolId(Model.PolId);
                    if (string.IsNullOrEmpty(dbName))
                        return new BaseResponse() { Message = MsgConstants.InvalidPolIdMsg };
                    response = _dbRepo.SaveInDBWithQuery(string.Format(SqlQueries.UpdateCorpAliasWithPolId, Model.OldShortName, Model.NewShortName, dbName, Model.ProviderMasterEntityId, Model.PolId, Model.ActionPerformedBy));
                }
                else
                    response = _dbRepo.SaveInDBWithQuery(string.Format(SqlQueries.ChangeCorporateAlias, Model.OldShortName, Model.NewShortName, Model.ActionPerformedBy, Model.ProviderMasterEntityId));

                if (response != null && response.IsSuccess)
                    Task.Factory.StartNew(() =>
                    {
                        //_dbRepo.UpdateInDB(string.Format(SqlQueries.UpdateDomainNameinAliasMapping, "@" + Model.NewShortName, Model.ProviderMasterEntityId));

                        int i = 1;
                        string userIds = null;
                        do
                        {
                            userIds = _dbRepo.GetUserIdsFromEntityIdWithPagination(int.Parse(Model.ProviderMasterEntityId), i);
                            if (!string.IsNullOrEmpty(userIds))
                            {
                                _syncHelper.SyncToElastic(userIds);
                                i++;
                            }
                        } while (!string.IsNullOrEmpty(userIds));
                    });

                return response;
            }
            catch (Exception Ex)
            {
                return new BaseResponse() { Message = Ex.Message };
            }
        }



        public string GetNameFromEntityId(int EntityId)
        {
            if (!(EntityId > default(int)))
                return null;

            return _dbRepo.CheckValidityOfEntityId(EntityId);
        }

        public BaseResponse ResetPassword(ResetPassword Model)
        {
            BaseResponse response = new BaseResponse();
            if (Model == null || string.IsNullOrEmpty(Model.ProviderMasterEntityId))
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            try
            {
                var aliasMapping = _dbRepo.GetAliasMapping(long.Parse(Model.ProviderMasterEntityId), true);
                if (aliasMapping == null || string.IsNullOrEmpty(aliasMapping.PasswordType))
                    return new BaseResponse() { Message = MsgConstants.NoAliasMappingFound };

                List<int> UserIds = new List<int>();
                if (!string.IsNullOrEmpty(Model.UserIds))
                    UserIds = Model.UserIds.Split(',').Select(x => int.Parse(x.Trim())).ToList();
                else
                    UserIds = _dbRepo.GetAllUserIdsFromEntityId(int.Parse(Model.ProviderMasterEntityId));

                response = ResetPassWithUserId(UserIds, aliasMapping.PasswordType, Model.ActionPerformedBy);
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ResetPassword:" + Ex.Message + " " + JsonConvert.SerializeObject(Model) + Environment.NewLine);
            }
            return response;
        }

        public ManagerCreationDTO GetUserForManager(string LoginName, int UserId, string EmailId, string PhoneNo)
        {
            ManagerCreationDTO response = new ManagerCreationDTO();
            try
            {
                ApplicationUser user = null;
                if (UserId > 0)
                    user = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_Id == UserId).FirstOrDefault();
                else if (!string.IsNullOrEmpty(LoginName))
                    user = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_LoginName == LoginName).FirstOrDefault();
                else if (!string.IsNullOrEmpty(EmailId))
                {
                    var usersWithEmailId = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_EmailId == EmailId);
                    if (usersWithEmailId.HasRecords() && usersWithEmailId.Count > 1)
                        return new ManagerCreationDTO() { ErrorMsg = MsgConstants.MultipleUsers };

                    user = usersWithEmailId.FirstOrDefault();
                }
                else if (!string.IsNullOrEmpty(PhoneNo))
                {
                    var usersWithPhoneNo = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_PhoneNumber == PhoneNo);
                    if (usersWithPhoneNo.HasRecords() && usersWithPhoneNo.Count > 1)
                        return new ManagerCreationDTO() { ErrorMsg = MsgConstants.MultipleUsers };

                    user = usersWithPhoneNo.FirstOrDefault();
                }

                if (user != null)
                {
                    response.Id = user.TAU_Id;
                    response.UserName = user.TAU_LoginName;
                    response.FirstName = user.TAU_FirstName;
                    response.LastName = user.TAU_LastName;
                    response.EntityId = user.TAU_ProviderMasterEntityId;
                    response.Email = user.TAU_EmailId;
                    response.IsActive = user.TAU_IsActive;
                    response.PhoneNo = user.TAU_PhoneNumber;
                    var genderProp = _dbRepo.GetTableRecordsFromExpression<ApplicationUserProperty>(x => x.TAUP_TAU_Id == user.TAU_Id && x.TAUP_IsActive == true && x.TAUP_Name == "Gender")?.FirstOrDefault();
                    response.Gender = genderProp.IsNotNull() ? genderProp.TAUP_Value : null;
                    var userRoles = _dbRepo.GetTableRecordsFromExpression<UserMapRoles>(x => x.TUMR_TAU_Id == user.TAU_Id && x.TUMR_IsActive == true);
                    if (userRoles.HasRecords())
                    {
                        response.UserRoles = userRoles.Select(x => x.TUMR_Role).ToList();
                        //Role allowedLoginType = _dbRepo.GetTableRecordsFromExpression<Role>(x => x.IsActive == true && response.UserRoles.Contains(x.Id)).FirstOrDefault(x => UtilityConstants.GroupKeys.Split(',').ToList().Contains(x.GroupKey));
                        //response.LoginType = allowedLoginType != null ? allowedLoginType.GroupKey : null;
                    }

                    response.UserProperties = GetIndividualPropertyFromDB(user.TAU_Id);
                    if (response.UserProperties != null && response.UserProperties.Count > 0)
                        response.IsPropertyLoaded = true;
                }
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetUserForManager: " + Ex.Message + " UserId: " + UserId + Environment.NewLine);
                response.ErrorMsg = Ex.Message;
            }
            return response;
        }

        public BaseResponse UpdateUser(ManagerCreationDTO Model)
        {
            if (Model == null)
                return new BaseResponse() { Message = MsgConstants.RequestCannotBeNull };

            //if (string.IsNullOrEmpty(Model.LoginType) || !UtilityConstants.GroupKeys.Split(',').ToList().Contains(Model.LoginType))
            //    return new BaseResponse() { Message = MsgConstants.InvalidLoginType };

            try
            {
                //string appId = UtilityConstants.AuthUtilityAppId;

                if (string.IsNullOrEmpty(Model.UserName))
                    return new BaseResponse() { Message = MsgConstants.UserNameCannotBeNull };

                if (!string.IsNullOrWhiteSpace(Model.Password))
                    Task.Factory.StartNew(() => ChangePassword(new UpdatePassword() { IsUserId = true, UserIdORUserName = Model.Id.ToString(), Password = Model.Password, ActionPerformedBy = Model.ActionPerformedBy }));

                List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(x => x.TAU_Id == Model.Id);
                if (!users.HasRecords())
                    return new BaseResponse() { Message = MsgConstants.UserNotFound };

                if (users.First().TAU_IsActive != Model.IsActive)
                    ChangeActiveStatus(new UpdateActiveStatus()
                    {
                        IsUserId = true,
                        Status = Model.IsActive.ToString(),
                        UserIdsORUserNames = Model.Id.ToString(),
                        ActionPerformedBy = Model.ActionPerformedBy
                    });

                if (!Model.IsActive)
                    return new BaseResponse() { IsSuccess = true };

                string accessToken = GetTokenWithAppKey(UtilityConstants.AuthUtilityAppId, Model.Id, _configManager.AuthUtilityAuthenticationKey);
                if (string.IsNullOrEmpty(accessToken))
                    return new BaseResponse() { Message = MsgConstants.ErrorGettingToken };

                if (users.First().TAU_LoginName.ToLower() != Model.UserName.ToLower())
                {
                    BaseResponse unameUpdateResp = _dbRepo.SaveInDBWithQuery(string.Format(SqlQueries.UpdateLoginName, Model.UserName, Model.ActionPerformedBy, Model.Id));
                    if (unameUpdateResp.IsNull() || !unameUpdateResp.IsSuccess)
                        return new BaseResponse() { Message = MsgConstants.UnableToUpdateUName + " " + unameUpdateResp.Message };
                }

                BaseResponseAuth baseProfileResponse = UpdateBaseUserProfile(new UpdateBaseUserProfileRequest()
                {
                    ApplicationId = UtilityConstants.AuthUtilityAppId,
                    AuthToken = accessToken,
                    BaseProfile = GetUserBaseProfile(Model)
                });

                //BaseResponse appBasedProfileResponse = new BaseResponse() { IsSuccess = true };
                //if (Model.UserProperties.HasRecords())
                //{
                //    appBasedProfileResponse = UpdateAppBasedProfile(new UpdateAppBasedProfileRequest()
                //    {
                //        ApplicationId = appId,
                //        AuthToken = accessToken,
                //        ProfileDataJson = JsonConvert.SerializeObject(Model.UserProperties)
                //    });
                //}

                BaseResponse response = new BaseResponse()
                {
                    IsSuccess = baseProfileResponse.IsNotNull() && baseProfileResponse.IsSuccess,
                    Message = baseProfileResponse.IsNull() ? MsgConstants.ServerError : baseProfileResponse.IsSuccess ? MsgConstants.SuccessMsg : baseProfileResponse.ErrorMessage
                };

                if (response.IsSuccess)
                    Task.Factory.StartNew(() =>
                    {
                        _dbRepo.MapUserMultipleRoles(Model.Id, Model.UserRoles, Model.ActionPerformedBy);
                        //TODO
                        // SaveUserPropertiesInDB(Model.UserProperties, Model.Id, appId, Model.ActionPerformedBy);
                    });

                return response;
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateUser => ManagerCreation:" + Ex.Message + " " + JsonConvert.SerializeObject(Model) + Environment.NewLine);
                return new BaseResponse() { Message = Ex.Message };
            }

        }

        public AuthGetTokenResponse GetBearer(AuthGetTokenRequest RequestObj, string AbsolutePath)
        {
            return _utilityRepo.GetBearer(RequestObj, AbsolutePath);
        }

        public BaseResponse CreateUser(ManagerCreationDTO Model)
        {
            BaseResponse isPresent = GetIfAlreadyPresent(Model.UserName, Model.Email, Model.PhoneNo);
            if (isPresent.IsNotNull() && !isPresent.IsSuccess && !string.IsNullOrEmpty(isPresent.Message))
                return isPresent;

            UserCreation userDetails = GetUserDetailsForCreation(Model);

            UserCreationResponse response = null;

            if (userDetails != null)
                response = _utilityRepo.CreateUser(userDetails, APIConstants.CreateCorporateUser);

            // Update AppBased Profile
            //if (response != null && response.IsSuccess && Model.UserProperties.HasRecords())
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        string appId = Helper.Instance.GetAppId(Model.LoginType);
            //        string authenticationKey = Helper.Instance.GetAuthenticationKey(appId, _configManager);
            //        string appBasedProfile = JsonConvert.SerializeObject(Model.UserProperties);

            //        if (appBasedProfile != null)
            //            UpdateAppBasedProfile(new UpdateAppBasedProfileRequest()
            //            {
            //                ApplicationId = appId,
            //                AuthToken = GetTokenWithAppKey(appId, response.UserId, authenticationKey),
            //                ProfileDataJson = appBasedProfile
            //            });

            //        SaveUserPropertiesInDB(Model.UserProperties, response.UserId, appId, Model.ActionPerformedBy);
            //    });

            //}
            return new BaseResponse()
            {
                IsSuccess = response.IsNotNull() && response.IsSuccess,
                Message = response.IsNull() ? MsgConstants.ServerError : response.IsSuccess ? MsgConstants.SuccessMsg : response.ErrorMessage
            };
        }

        public Dictionary<string, string> GetPreviousAppBasedProfile(int UserId, string LoginType)
        {
            string appId = Helper.Instance.GetAppId(LoginType);

            if (string.IsNullOrEmpty(appId))
                return null;

            var previousProfile = _dbRepo.GetTableRecordsFromExpression<ApplicationUserProperty>(x => x.TAUP_TAU_Id == UserId && x.TAUP_IsActive == true && x.TAUP_Name == "Profile_" + appId);
            string previousProfileJson = previousProfile.Count > 0 ? previousProfile.FirstOrDefault().TAUP_Value : null;

            dynamic previousProfileObj = null;

            if (!string.IsNullOrEmpty(previousProfileJson))
                previousProfileObj = JValue.Parse(previousProfileJson);

            return Helper.Instance.JObjectToDictionary(previousProfileObj);
        }

        public BaseResponse SaveCorporateAlias(AliasMapping Model)
        {
            if (Model.IsNull() || !Model.IsValid())
                return new BaseResponse() { Message = MsgConstants.ParametersNotFound };

            if (!string.IsNullOrEmpty(Model.PasswordType) && !Model.PasswordType.Contains('[') && !Model.PasswordType.Contains(']'))
                Model.PasswordType = "\"" + Model.PasswordType.Replace("\"", "") + "\"";

            if (!string.IsNullOrEmpty(Model.Prefix))
                Model.Prefix = Model.Prefix.Trim();

            if (!string.IsNullOrEmpty(Model.Suffix))
                Model.Suffix = Model.Suffix.Trim();

            if (!string.IsNullOrEmpty(Model.PossibleAliases))
                Model.PossibleAliases = string.Join(",", Model.PossibleAliases
                                              .Split(",")
                                              .ToList()
                                              .Select(x => x.Replace("@", "").Insert(0, "@")));

            var previousRecord = _dbRepo.GetAliasMapping(Model.PolCorporateId.Value, false);
            bool isSuccess = _dbRepo.SaveAliasMapping(GetMapping(Model, previousRecord));
            return new BaseResponse()
            {
                IsSuccess = isSuccess,
                Message = !isSuccess ? MsgConstants.DataUpdationFailed : MsgConstants.SuccessMsg
            };
        }

        public AliasMapping GetAliasMappingForUi(long PolCorporateId)
        {
            var mapping = _dbRepo.GetAliasMapping(PolCorporateId, false);
            AliasMapping response = new AliasMapping();
            if (mapping.IsNull())
                return response;

            return ToModel(mapping);
        }

        private AliasMapping ToModel(CorporateAliasMapping from)
        {
            if (from.IsNull())
                return new AliasMapping();

            return new AliasMapping()
            {
                PolCorporateId = from.CAM_PolCorporateId,
                PolGroupCorporateId = from.CAM_PolGroupCorporateId,
                DBType = !string.IsNullOrEmpty(from.CAM_DBType) ? from.CAM_DBType.Contains(",") ? DBType.BOTH : (Nullable<DBType>)Enum.Parse(typeof(DBType), from.CAM_DBType, true) : null,
                IsActive = from.CAM_IsActive,
                LoginType = !string.IsNullOrEmpty(from.CAM_LoginType) ? (Nullable<LoginType>)Enum.Parse(typeof(LoginType), from.CAM_LoginType, true) : null,
                PasswordType = !string.IsNullOrEmpty(from.PasswordType) ? from.PasswordType.Replace("\"", "") : null,
                Prefix = from.CAM_Prefix,
                Suffix = from.CAM_Suffix,
                PossibleAliases = from.CAM_PossibleAliases
            };
        }

        private CorporateAliasMapping GetMapping(AliasMapping Model, CorporateAliasMapping PreviousRecord)
        {
            if (Model.IsNull())
                return null;

            return new CorporateAliasMapping()
            {
                CAM_ID = PreviousRecord.IsNotNull() ? PreviousRecord.CAM_ID : default(int),
                CAM_PolCorporateId = Model.PolCorporateId.Value,
                CAM_PolGroupCorporateId = (Model.PolGroupCorporateId.IsNotNull() && Model.PolGroupCorporateId > 0) ? Model.PolGroupCorporateId : PreviousRecord.IsNotNull() && PreviousRecord.CAM_PolGroupCorporateId > 0 ? PreviousRecord.CAM_PolGroupCorporateId : null,
                CAM_DBType = Model.DBType != DBType.BOTH ? Model.DBType.ToString() : string.Join(",", new List<string> { DBType.MA.ToString(), DBType.MAUHS.ToString() }),
                CAM_IsActive = Model.IsActive,
                CAM_LoginType = Model.LoginType.ToString(),
                CAM_PossibleAliases = Model.PossibleAliases,
                CAM_Prefix = Model.Prefix,
                CAM_Suffix = Model.Suffix,
                PasswordType = Model.PasswordType,
                CAM_ModifiedOn = DateTime.Now,
                CAM_ModifiedBy = Model.ActionPerformedBy,
                CAM_CreatedOn = PreviousRecord.IsNotNull() ? PreviousRecord.CAM_CreatedOn : DateTime.Now,
                CAM_CreatedBy = PreviousRecord.IsNotNull() ? PreviousRecord.CAM_CreatedBy : Model.ActionPerformedBy
            };
        }

        private BaseResponse GetIfAlreadyPresent(string UserName, string EmailId, string PhoneNo)
        {
            if (string.IsNullOrEmpty(UserName))
                return new BaseResponse() { Message = MsgConstants.UserNameCannotBeNull };

            Expression<Func<ApplicationUser, bool>> Expression = x => x.TAU_LoginName == UserName;
            if (!string.IsNullOrEmpty(EmailId))
                Expression = Expression.Or(x => x.TAU_EmailId == EmailId);
            if (!string.IsNullOrEmpty(PhoneNo))
                Expression = Expression.Or(x => x.TAU_PhoneNumber == PhoneNo);

            List<ApplicationUser> users = _dbRepo.GetTableRecordsFromExpression<ApplicationUser>(Expression.And(x => x.TAU_IsActive));

            if (users.HasRecords())
            {
                if (users.Any(x => x.TAU_LoginName == UserName))
                    return new BaseResponse() { Message = string.Format(MsgConstants.UsersAlreadyPresent, "With Same UserName") };
                if (users.Any(x => x.TAU_EmailId == EmailId))
                    return new BaseResponse() { Message = string.Format(MsgConstants.UsersAlreadyPresent, "With Same EmailId") };
                if (users.Any(x => x.TAU_PhoneNumber == PhoneNo))
                    return new BaseResponse() { Message = string.Format(MsgConstants.UsersAlreadyPresent, "With Same PhoneNo") };
            }

            return new BaseResponse() { IsSuccess = true };
        }

        //private string GetErrorMsg(BaseResponse BaseProfileResponse, BaseResponse AppBasedProfileResponse)
        //{
        //    string response = null;

        //    if (BaseProfileResponse != null && !BaseProfileResponse.IsSuccess)
        //        response = BaseProfileResponse.ErrorMessage;
        //    else if (AppBasedProfileResponse != null && !AppBasedProfileResponse.IsSuccess)
        //        response = AppBasedProfileResponse.ErrorMessage;

        //    return response;
        //}

        private Dictionary<string, string> GetIndividualPropertyFromDB(int UserId)
        {
            var applicableProps = new Dictionary<string, string> { { "IWP_EmpID", "EmployeeId" } };
            List<ApplicationUserProperty> propertiesInDB = _dbRepo.GetTableRecordsFromExpression<ApplicationUserProperty>(x => x.TAUP_TAU_Id == UserId && x.TAUP_IsActive == true);
            Dictionary<string, string> response = new Dictionary<string, string>();
            if (propertiesInDB.HasRecords())
            {
                foreach (var item in applicableProps)
                {
                    var propInDB = propertiesInDB.Where(x => x.TAUP_Name.ToLower() == item.Key.ToLower())?.FirstOrDefault();
                    if (propInDB.IsNotNull())
                        response.Add(item.Value, propInDB.TAUP_Value);
                }
            }
            return response;
        }

        //private void SaveUserPropertiesInDB(Dictionary<string, string> Properties, int UserId, string AppId, int ActionPerformedBy)
        //{
        //    if (UserId == 0)
        //        return;

        //    List<ApplicationUserProperty> propertiesInDB = _dbRepo.GetTableRecordsFromExpression<ApplicationUserProperty>(x => x.TAUP_TAU_Id == UserId);
        //    foreach (var item in _configManager.AppBasedFields)
        //    {
        //        var propertyInModel = Properties != null ? Properties.FirstOrDefault(x => x.Key == item) : default(KeyValuePair<string, string>);
        //        string propertyName = GetPropertyName(item);
        //        ApplicationUserProperty propertyInDb = propertiesInDB.Count > 0 ? propertiesInDB.FirstOrDefault(x => x.TAUP_Name == propertyName) : null;
        //        bool isPresentInModel = !string.IsNullOrEmpty(propertyInModel.Key);
        //        bool isPresentinDb = propertyInDb != null;

        //        if (isPresentInModel && isPresentinDb && (!propertyInDb.TAUP_IsActive || propertyInDb.TAUP_Value != propertyInModel.Value))
        //        {
        //            propertyInDb.TAUP_IsActive = true;
        //            propertyInDb.TAUP_Value = propertyInModel.Value;
        //            propertyInDb.TAUP_ModifiedOn = DateTime.Now;
        //            propertyInDb.TAUP_ModifitedBy = ActionPerformedBy;
        //            _dbRepo.UpdateInDB<ApplicationUserProperty>(propertyInDb);
        //        }
        //        else if (!isPresentInModel && isPresentinDb)
        //        {
        //            propertyInDb.TAUP_IsActive = false;
        //            propertyInDb.TAUP_ModifiedOn = DateTime.Now;
        //            propertyInDb.TAUP_ModifitedBy = ActionPerformedBy;
        //            _dbRepo.UpdateInDB<ApplicationUserProperty>(propertyInDb);
        //        }
        //        else if (isPresentInModel && !isPresentinDb)
        //        {
        //            propertyInDb = new ApplicationUserProperty(UserId, AppId, propertyName, propertyInModel.Value, ActionPerformedBy);
        //            _dbRepo.InsertInDB<ApplicationUserProperty>(propertyInDb);
        //        }
        //    }
        //}

        //private string GetPropertyName(string Key)
        //{
        //    string propertyName = null;
        //    switch (Key)
        //    {
        //        case "AssignedContracts":
        //            propertyName = "Assigned Contracts";
        //            break;
        //        case "AssignedCorporates":
        //            propertyName = "Assigned Corporates";
        //            break;
        //        default:
        //            propertyName = Key;
        //            break;
        //    }
        //    return propertyName;
        //}

        private UserCreation GetUserDetailsForCreation(ManagerCreationDTO Model)
        {
            if (Model == null)
                return null;

            return new UserCreation()
            {
                ApplicationId = UtilityConstants.AuthUtilityAppId,
                Email = Model.Email,
                EmployeeId = Model.UserProperties != null ? Model.UserProperties.FirstOrDefault(x => x.Key == "EmployeeId").Value : null,
                FirstName = Model.FirstName.Trim(),
                LastName = !string.IsNullOrEmpty(Model.LastName) ? Model.LastName.Trim() : null,
                Gender = Model.Gender,
                Password = Model.Password,
                ProviderMasterEntityId = Model.EntityId,
                UserName = Model.UserName,
                UserRoles = Model.UserRoles,
                PhoneNumber = Model.PhoneNo,
                IgnoreAliasConfig = true
            };
        }

        private string GetAbsolutePathForUserCreation(string AppId)
        {
            string path = null;
            switch (AppId)
            {
                case UtilityConstants.ProcheckAppId:
                    path = APIConstants.CreateCorporateUser;
                    break;

                case UtilityConstants.MediMarketAppId:
                    path = APIConstants.CreateRetaileUser;
                    break;

                default:
                    break;
            }
            return path;
        }

        private BaseResponseAuth UpdateBaseUserProfile(UpdateBaseUserProfileRequest Request)
        {
            return _utilityRepo.UpdateBaseProfile(Request);
        }

        private BaseResponse UpdateAppBasedProfile(UpdateAppBasedProfileRequest Request)
        {
            return _utilityRepo.UpdateAppBasedProfile(Request) as BaseResponse;
        }
        private UserBaseProfile GetUserBaseProfile(ManagerCreationDTO Model)
        {
            if (Model == null)
                return null;

            return new UserBaseProfile()
            {
                FirstName = Model.FirstName,
                Active = Model.IsActive,
                LastName = Model.LastName,
                UserName = Model.UserName,
                EntityId = Model.EntityId,
                EmailId = Model.Email,
                MobileNo = Model.PhoneNo,
                EmployeeId = Model.UserProperties != null ? Model.UserProperties.FirstOrDefault(x => x.Key == "EmployeeId").Value : null,
                Gender = Model.Gender
            };
        }

        private string GetTokenWithAppKey(string AppId, int UserId, string AuthenticationKey)
        {

            AuthGetTokenRequest requestObj = new AuthGetTokenRequest(null, null, _httpContextAccessor, AuthenticationKey, AppId, UserId);
            AuthGetTokenResponse response = GetBearer(requestObj, APIConstants.SignInWithAppKey);
            if (response == null || response.AccessToken == null || response.AccessToken.Access_Token == null)
                return null;

            return response.AccessToken.Access_Token;
        }

        private BaseResponse ResetPassWithUserId(List<int> UserIds, string PasswordType, int ActionPerformedBy)
        {
            if (UserIds == null || UserIds.Count == 0 || string.IsNullOrEmpty(PasswordType))
                return new BaseResponse() { Message = MsgConstants.InvalidData };

            int count = 0;
            BaseResponse response = new BaseResponse();
            try
            {
                foreach (int userId in UserIds)
                {
                    string password = string.Empty;
                    var userDetails = _dbRepo.GetUserDetails(userId);
                    if (userDetails != null)
                        password = GetPassword(userDetails.ToBasicDetails(), PasswordType);

                    if (!string.IsNullOrEmpty(password))
                    {
                        var res = _dbRepo.UpdatePasswordAuth(new List<int> { userId }, password, ActionPerformedBy);
                        if (res.IsSuccess)
                            count++;
                    }
                }
                response.IsSuccess = count > 0;
                response.Message = string.Format(MsgConstants.SuccessMsgWithCount, count);
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in ResetPassWithUserId:" + Ex.Message + " " + string.Join(",", UserIds) + Environment.NewLine);
            }
            return response;
        }

        private string GetPassword(UserBasicDetails UserDetails, string PasswordType)
        {
            if (UserDetails == null || string.IsNullOrEmpty(PasswordType))
                return null;

            string password = null;
            try
            {
                if (PasswordType.Contains('"'))
                    password = PasswordType.Trim('"');
                else if (PasswordType.ToUpper() == "[ENCRYPTED]")
                    password = RandomPassword.GeneratePassword();
                else
                {
                    var format = PasswordType.Split('^');
                    if (format != null && format.Count() > 0)
                    {
                        password = format.First();
                        string caseFormat = format.FirstOrDefault(x => x.Contains("U") || x.Contains("L"));
                        string dateFormat = format.FirstOrDefault(x => x.StartsWith("dd") || x.LastIndexOf("yy") > -1);
                        dateFormat = dateFormat ?? "dd-MM-yyyy";

                        if (!string.IsNullOrEmpty(UserDetails.EmployeeId) && password.Contains("[EMPID]"))
                            password = password.Replace("[EMPID]", UserDetails.EmployeeId.Trim());
                        if (UserDetails.DOB != null && UserDetails.DOB.Date != default(DateTime) && password.Contains("[DOB]"))
                            password = password.Replace("[DOB]", UserDetails.DOB.ToString(dateFormat));
                        if (UserDetails.DateOfJoining != null && UserDetails.DateOfJoining.Date != default(DateTime) && password.Contains("[DOJ]"))
                            password = password.Replace("[DOJ]", UserDetails.DateOfJoining.ToString(dateFormat));
                        if (UserDetails.DateOfMarriage != null && UserDetails.DateOfMarriage.Date != default(DateTime) && password.Contains("[DOM]"))
                            password = password.Replace("[DOM]", UserDetails.DateOfMarriage.ToString(dateFormat));

                        if (password.IndexOfAny(new char[] { '[', ']' }) > -1)
                            return null;

                        switch (caseFormat)
                        {
                            case "L":
                                password = password.ToLower();
                                break;
                            case "U":
                                password = password.ToUpper();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetPassword:" + Ex.Message + " " + JsonConvert.SerializeObject(UserDetails) + Environment.NewLine);
            }
            return password;
        }
    }
}