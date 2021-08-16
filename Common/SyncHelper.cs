using AuthUtility.Constants;
using AuthUtility.Interfaces;
using AuthUtility.Models;
using Microsoft.Extensions.Logging;
using System;

namespace AuthUtility.Common
{
    public class SyncHelper : ISyncHelper
    {
        private readonly ConfigManager _configManager;
        private readonly IRestClient _restClient;
        private readonly ILogger _logger;
        public SyncHelper(ILogger<SyncHelper> logger, ConfigManager configManager, IRestClient restClient)
        {
            this._configManager = configManager;
            this._restClient = restClient;
            this._logger = logger;
        }
        public ElasticSyncResponse SyncToElastic(string UserIds)
        {
            return default;
            //if (string.IsNullOrEmpty(UserIds))
            //    return new ElasticSyncResponse() { ErrorMessage = MsgConstants.RequestCannotBeNull };

            //if (_configManager.IsStaging)
            //    return null;

            //try
            //{
            //    if (_logger != null)
            //        _logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Sync Request For UserIds:" + UserIds + Environment.NewLine);

            //    ElasticSyncRequest requestObj = new ElasticSyncRequest()
            //    {
            //        ApplicationId = UtilityConstants.AuthUtilityAppId,
            //        UserIds = UserIds,
            //        //ValidationKey = _configManager.KeyToSyncToElastic
            //    };
            //    var responseObj = _restClient.MakePostRestCall<ElasticSyncRequest, ElasticSyncResponse>(requestObj, APIConstants.SyncToElastic, _configManager.MediAuthUrl);
            //    if (responseObj != null && responseObj.IsSuccess && _logger != null)
            //        _logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Successfully synced for UserIds:" + UserIds + Environment.NewLine);
            //    else if (_logger != null)
            //        _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in Sync for UserIds:" + UserIds + Environment.NewLine);

            //    return responseObj;
            //}
            //catch (Exception Ex)
            //{
            //    if (_logger != null)
            //        _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in SyncToElastic:" + Ex.Message + Environment.NewLine);

            //    return new ElasticSyncResponse() { ErrorMessage = MsgConstants.ServerError };
            //}
        }

        public BaseResponse RemoveBaseUserProfile(int UserId)
        {
            if (UserId == 0)
                return new BaseResponse() { Message = MsgConstants.NoDataFound };

            if (_configManager.IsStaging)
                return null;

            try
            {
                if (_logger != null)
                    _logger.LogInformation(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Remove Base Profile Request For UserId:" + UserId + Environment.NewLine);

                RemoveBaseUserProfileRequest requestObj = new RemoveBaseUserProfileRequest()
                {
                    ApplicationId = UtilityConstants.AuthUtilityAppId,
                    UserId = UserId
                };
                var responseObj = _restClient.MakePostRestCall<RemoveBaseUserProfileRequest, BaseResponse>(requestObj, APIConstants.RemoveBaseProfile, _configManager.MediAuthUrl);
                if (responseObj != null && responseObj.IsSuccess && _logger != null)
                    _logger.LogInformation(DateTime.Now + " Successfully removed base profile for UserId:" + UserId + Environment.NewLine);
                else if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in removing base profile for UserId:" + UserId + Environment.NewLine);

                return responseObj;
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in RemoveBaseUserProfile:" + Ex.Message + Environment.NewLine);

                return new ElasticSyncResponse() { Message = MsgConstants.ServerError };
            }
        }
    }
    public class ElasticSyncRequest
    {
        public string ApplicationId { get; set; }
        public string ValidationKey { get; set; }
        public string UserIds { get; set; }
    }
    public class ElasticSyncResponse : BaseResponse
    {
        public string SyncedUserIds { get; set; }
        public string ErrorDescription { get; set; }
    }
    public class RemoveBaseUserProfileRequest
    {
        public string ApplicationId { get; set; }
        public int UserId { get; set; }        
    }
}