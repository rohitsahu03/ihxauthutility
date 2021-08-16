using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Interfaces;
using AuthUtility.MediBuddyDBFactory;
using AuthUtility.Models;
using AuthUtility.Models.Supreme;
using AuthUtility.Models.UtilityModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AuthUtility.Repository
{
    public class UtilityRepo : IUtilityRepo
    {
        private ConfigManager _config = null;
        private IRestClient _restClient = null;
        public UtilityRepo(ConfigManager config, IRestClient restClient)
        {
            _config = config;
            _restClient = restClient;
        }
        public Dictionary<int, string> GetDCCity()
        {
            RootObject result = _restClient.MakeGetRestCall<RootObject>(APIConstants.GetCities, _config.UtilityUrl);
            if (result == null || result.lookUpData == null)
                return null;

            return result.lookUpData.Select(a => new { a.lookUpId, a.lookUpValue }).ToDictionary(t => t.lookUpId, t => t.lookUpValue);
        }

        public Dictionary<int, string> GetDCState()
        {
            RootObject result = _restClient.MakeGetRestCall<RootObject>(APIConstants.GetStates, _config.UtilityUrl);
            if (result == null || result.lookUpData == null)
                return null;

            return result.lookUpData.Select(a => new { a.lookUpId, a.lookUpValue }).ToDictionary(t => t.lookUpId, t => t.lookUpValue);
        }

        public Dictionary<int, string> GetDCLocation()
        {
            RootObject result = _restClient.MakeGetRestCall<RootObject>(APIConstants.GetLocations, _config.UtilityUrl);
            if (result == null || result.lookUpData == null)
                return null;

            return result.lookUpData.Select(a => new { a.lookUpId, a.lookUpValue }).ToDictionary(t => t.lookUpId, t => t.lookUpValue);
        }

        public List<Role> GetLoginTypeRoles(string searchfilter)
        {
            return DBFactory.GetTable<Role>(x => x.GroupKey == searchfilter && x.IsActive == true)?.OrderBy(x => x.Name).ToList();
        }

        public List<ProviderMasterContract> GetContractDetails()
        {
            var values = LookUpRequest(new MasterLookUpRequest
            {
                LookUpByColumnName = true,
                ColumnNames = new List<string> { "EntityRelation" }
            });

            return values.IsNotNull() && values.LookupData.HasRecords()
                  ? values.LookupData.Select(x => new ProviderMasterContract { id = x.MasterLookUpID.ToString(), name = x.MasterLookUpColumnValue }).ToList()
                  : default;
        }

        public List<ProviderEntityRelation> GetEntityRelationDetails(int contractId)
        {
            string api = string.Format(APIConstants.EntityRelationDetails, contractId);
            return _restClient.MakeGetRestCall<List<ProviderEntityRelation>>(api, _config.UtilityUrl);
        }

        public List<EntityDTO> GetERDetails(int contractId)
        {
            if (contractId <= 0)
                return default;

            EntityRelationRequest req = new EntityRelationRequest
            {
                PageNumber = 1,
                PageSize = 100,
                EntityRelationByEntityRelationTypeId = true,
                EntityRelationTypeIds = new List<int> { contractId },
                GetPrimaryDetail = true
            };
            List<EntityDTO> result = new List<EntityDTO>();
            EntityRelationResponse response = null;
            do
            {
                
                response = FetchEntityRelation(req);
                if (response.IsNotNull() && response.EntityRelationDetails.HasRecords())
                {
                    result.AddRange(response.EntityRelationDetails.Select(x => x.PrimaryEntityDetail));
                    req.PageNumber++;
                }
            } while (response.IsNotNull() && response.EntityRelationDetails.HasRecords() && result.Count < response.TotalRecords);
            return result;
        }

        public List<DCMasterProvider> GetDCEntity()
        {
            return _restClient.MakeGetRestCall<List<DCMasterProvider>>(APIConstants.DCEntity, _config.UtilityUrl);
        }

        public AuthGetTokenResponse GetBearer(AuthGetTokenRequest RequestObj, string AbsolutePath)
        {
            return _restClient.MakePostRestCall<AuthGetTokenRequest, AuthGetTokenResponse>(RequestObj, AbsolutePath, _config.MediAuthUrl);
        }

        public BaseResponseAuth UpdateBaseProfile(UpdateBaseUserProfileRequest RequestObj)
        {
            return _restClient.MakePostRestCall<UpdateBaseUserProfileRequest, BaseResponseAuth>(RequestObj, APIConstants.UpdateBaseProfilePath, _config.MediAuthUrl);
        }

        public ProfileResponse UpdateAppBasedProfile(UpdateAppBasedProfileRequest RequestObj)
        {
            return _restClient.MakePostRestCall<UpdateAppBasedProfileRequest, ProfileResponse>(RequestObj, APIConstants.UpdateAppBasedProfilePath, _config.MediAuthUrl);
        }

        public UserCreationResponse CreateUser(UserCreation Model, string AbsolutePath)
        {
            return _restClient.MakePostRestCall<UserCreation, UserCreationResponse>(Model, AbsolutePath, _config.MediAuthUrl);
        }

        public MasterLookUpResponse LookUpRequest(MasterLookUpRequest RequestObj)
        {
            if (RequestObj.IsNull())
                return default;

            return _restClient.MakePostRestCall<MasterLookUpRequest, MasterLookUpResponse>(RequestObj, APIConstants.LookUpRequest, _config.IHXSupremeUrl);
        }

        public EntityListResponse FetchEntity(EntityListRequest RequestObj)
        {
            if (RequestObj.IsNull())
                return default;

            return _restClient.MakePostRestCall<EntityListRequest, EntityListResponse>(RequestObj, APIConstants.EntityListRequest, _config.IHXSupremeUrl);
        }

        public EntityRelationResponse FetchEntityRelation(EntityRelationRequest requestObj)
        {
            if (requestObj.IsNull())
                return default;

            return _restClient.MakePostRestCall<EntityRelationRequest, EntityRelationResponse>(requestObj, APIConstants.EntityRelationListRequest, _config.IHXSupremeUrl);
        }
    }
}
