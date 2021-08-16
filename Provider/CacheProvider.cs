using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Interfaces;
using AuthUtility.Models.Supreme;
using AuthUtility.Models.UtilityModel;
using System.Collections.Generic;
using System.Linq;

namespace AuthUtility.Provider
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IUtilityRepo _utilityRepo;
        private readonly ICachingHelper _cachingHelper;
        private readonly IDBRepo _dbRepo;
        public CacheProvider(IUtilityRepo utilityRepo, ICachingHelper cachingHelper, IDBRepo dbRepo)
        {
            _utilityRepo = utilityRepo;
            _cachingHelper = cachingHelper;
            _dbRepo = dbRepo;
        }

        public void RefreshCache()
        {
            //FillCacheWithContractDetails();
            //FillCacheWithCities();
            //FillCacheWithStates();
            //FillCacheWithLocations();
            FillCacheWithUserRoles();
            //FillCacheWithProclaimRoles();
            //FillCacheWithDCDetails();
            //FillCacheWithDCMaster();
            FillCacheWithEntities();
        }

        public Dictionary<long, string> GetEntities()
        {
            Dictionary<long, string> response = _cachingHelper.GetCachedData<Dictionary<long, string>>(CachingConstants.Entities);
            if (!response.HasRecords())
                FillCacheWithEntities();
            return _cachingHelper.GetCachedData<Dictionary<long, string>>(CachingConstants.Entities);
        }

        private Dictionary<long, string> FetchAllEntities()
        {
            MasterLookUpRequest lookUpReqObj = new MasterLookUpRequest()
            {
                LookUpByColumnName = true,
                ColumnNames = new List<string> { "Entity_Type" }
            };
            MasterLookUpResponse lookUpResponse = _utilityRepo.LookUpRequest(lookUpReqObj);
            if (lookUpResponse.IsNull() || !lookUpResponse.LookupData.HasRecords())
                return default;

            EntityListRequest entityReqObj = new EntityListRequest()
            {
                EntityListByEntityType = true,
                EntityTypes = lookUpResponse.LookupData.Select(x => x.MasterLookUpID).ToList(),
                PageNumber = 1,
                PageSize = 500
            };
            EntityListResponse entityResponse = null;
            List<EntityDetail> entities = new List<EntityDetail>();
            do
            {
                entityResponse = _utilityRepo.FetchEntity(entityReqObj);
                if (entityResponse.IsNotNull() && entityResponse.Entities.HasRecords())
                {
                    entities.AddRange(entityResponse.Entities);
                    entityReqObj.PageNumber++;
                }
            } while (entityResponse.IsNotNull() && entityResponse.Entities.HasRecords() && entities.Count < entityResponse.TotalRecords);

            return entities
                   ?.GroupBy(x => x.EntityId)
                   .Select(x => x.FirstOrDefault())
                   .ToDictionary(x => x.EntityId, x => x.EntityId + " | " + x.EntityName + "," + x.Address + "," + x.City + "-" + x.Pincode + "," + x.State);
        }

        public Dictionary<int, string> GetCities()
        {
            Dictionary<int, string> response = _cachingHelper.GetCachedData<Dictionary<int, string>>(CachingConstants.Cities);
            if (response == null || response.Count == 0)
                FillCacheWithCities();
            response = _cachingHelper.GetCachedData<Dictionary<int, string>>(CachingConstants.Cities);

            return response;
        }

        public Dictionary<int, string> GetStates()
        {
            Dictionary<int, string> response = _cachingHelper.GetCachedData<Dictionary<int, string>>(CachingConstants.States);
            if (response == null || response.Count == 0)
                FillCacheWithStates();
            response = _cachingHelper.GetCachedData<Dictionary<int, string>>(CachingConstants.States);

            return response;
        }

        public Dictionary<int, string> GetLocations()
        {
            Dictionary<int, string> response = _cachingHelper.GetCachedData<Dictionary<int, string>>(CachingConstants.Locations);
            if (response == null || response.Count == 0)
                FillCacheWithLocations();
            response = _cachingHelper.GetCachedData<Dictionary<int, string>>(CachingConstants.Locations);

            return response;
        }

        public Dictionary<string, string> GetAuthRoles(bool isFetchGroupKey= false)
        {
            List<Role> response = _cachingHelper.GetCachedData<List<Role>>(CachingConstants.UserRoles);
            if (response.HasRecords())
                FillCacheWithUserRoles();
            response = _cachingHelper.GetCachedData<List<Role>>(CachingConstants.UserRoles);
            if(response.HasRecords())
               return response.ToDictionary(x => x.Id.ToString(), x => x.Name + (isFetchGroupKey ? " - " + x.GroupKey : ""));

            return default;
        }

        public Dictionary<string, string> GetProclaimRoles()
        {
            Dictionary<string, string> response = _cachingHelper.GetCachedData<Dictionary<string, string>>(CachingConstants.UserRolesProclaim);
            if (response == null || response.Count == 0)
                FillCacheWithProclaimRoles();
            response = _cachingHelper.GetCachedData<Dictionary<string, string>>(CachingConstants.UserRolesProclaim);

            return response;
        }

        public List<ProviderEntityRelation> GetAllDCDetails()
        {
            List<ProviderEntityRelation> response = _cachingHelper.GetCachedData<List<ProviderEntityRelation>>(CachingConstants.DC);
            if (response == null || response.Count == 0)
                FillCacheWithDCDetails();
            response = _cachingHelper.GetCachedData<List<ProviderEntityRelation>>(CachingConstants.DC);

            return response;
        }

        public List<DCMasterProvider> GetDCMaster()
        {
            List<DCMasterProvider> response = _cachingHelper.GetCachedData<List<DCMasterProvider>>(CachingConstants.DCMaster);
            if (response == null || response.Count == 0)
                FillCacheWithDCMaster();
            response = _cachingHelper.GetCachedData<List<DCMasterProvider>>(CachingConstants.DCMaster);

            return response;
        }

        public List<ProviderMasterContract> GetContractDetails()
        {
            List<ProviderMasterContract> response = _cachingHelper.GetCachedData<List<ProviderMasterContract>>(CachingConstants.ContractDetails);
            if (response == null || response.Count == 0)
                FillCacheWithContractDetails();
            response = _cachingHelper.GetCachedData<List<ProviderMasterContract>>(CachingConstants.ContractDetails);

            return response;
        }

        public List<ProviderEntityRelation> GetEntityRelation(int ContractId)
        {
            string key = string.Format("ER{0}", ContractId);
            List<ProviderEntityRelation> response = _cachingHelper.GetCachedData<List<ProviderEntityRelation>>(key);
            if (response == null || response.Count == 0)
                FillCacheWithSingleEntityRelation(ContractId);
            response = _cachingHelper.GetCachedData<List<ProviderEntityRelation>>(key);

            return response;
        }

        private void FillCacheWithEntities()
        {
            _cachingHelper.RemoveCache(CachingConstants.Entities);
            Dictionary<long, string> entities = FetchAllEntities();
            if (entities.HasRecords())
                _cachingHelper.SetDataInCache<Dictionary<long, string>>(CachingConstants.Entities, entities);
        }

        private void FillCacheWithCities()
        {
            _cachingHelper.RemoveCache(CachingConstants.Cities);
            Dictionary<int, string> cities = _utilityRepo.GetDCCity();
            if (cities != null && cities.Count > 0)
                _cachingHelper.SetDataInCache<Dictionary<int, string>>(CachingConstants.Cities, cities);
        }

        private void FillCacheWithStates()
        {
            _cachingHelper.RemoveCache(CachingConstants.States);
            Dictionary<int, string> states = _utilityRepo.GetDCState();
            if (states != null && states.Count > 0)
                _cachingHelper.SetDataInCache<Dictionary<int, string>>(CachingConstants.States, states);
        }

        private void FillCacheWithLocations()
        {
            _cachingHelper.RemoveCache(CachingConstants.Locations);
            Dictionary<int, string> locations = _utilityRepo.GetDCLocation();
            if (locations != null && locations.Count > 0)
                _cachingHelper.SetDataInCache<Dictionary<int, string>>(CachingConstants.Locations, locations);
        }

        private void FillCacheWithUserRoles()
        {
            _cachingHelper.RemoveCache(CachingConstants.UserRoles);
           List<Role> roles = _dbRepo.GetAllRoles(SqlQueries.GetAllRolesinAuth);
            if (roles != null && roles.Count > 0)
                _cachingHelper.SetDataInCache<List<Role>>(CachingConstants.UserRoles, roles);
        }


        private void FillCacheWithProclaimRoles()
        {
            _cachingHelper.RemoveCache(CachingConstants.UserRolesProclaim);
            List<Role> roles = _dbRepo.GetAllRoles(SqlQueries.GetAllProclaimRoles);
            if (roles != null && roles.Count > 0)
                _cachingHelper.SetDataInCache<List<Role>>(CachingConstants.UserRolesProclaim, roles);
        }

        private void FillCacheWithDCDetails()
        {
            _cachingHelper.RemoveCache(CachingConstants.DC);
            List<ProviderEntityRelation> dcDetails = _utilityRepo.GetEntityRelationDetails(5290);
            if (dcDetails != null && dcDetails.Count > 0)
                _cachingHelper.SetDataInCache<List<ProviderEntityRelation>>(CachingConstants.DC, dcDetails);
        }

        private void FillCacheWithDCMaster()
        {
            _cachingHelper.RemoveCache(CachingConstants.DCMaster);
            var DCEntity = GetAllDCDetails();
            List<DCMasterProvider> refinedDc = new List<DCMasterProvider>();
            Dictionary<int, string> dccity = GetCities();
            Dictionary<int, string> dcstate = GetStates();
            Dictionary<int, string> dclocation = GetLocations();

            if (dccity.Count > 0 && dcstate.Count > 0 && dclocation.Count > 0)
            {
                DCEntity.ForEach(x =>
                {
                    refinedDc.Add(new DCMasterProvider()
                    {
                        fullName = x.entityName + ","
                                  + dcstate.Where(y => y.Key == x.stateId).Select(y => y.Value).FirstOrDefault() + ","
                                  + dccity.Where(y => y.Key == x.cityId).Select(y => y.Value).FirstOrDefault() + ","
                                  + dclocation.Where(y => y.Key == x.locationId).Select(y => y.Value).FirstOrDefault() + " => "
                                  + x.id,
                        id = x.entityId,
                        DCid = x.id
                    });
                });
            }

            if (refinedDc != null && refinedDc.Count > 0)
                _cachingHelper.SetDataInCache<List<DCMasterProvider>>(CachingConstants.DCMaster, refinedDc);
        }

        private void FillCacheWithContractDetails()
        {
            _cachingHelper.RemoveCache(CachingConstants.ContractDetails);

            List<ProviderMasterContract> response = _utilityRepo.GetContractDetails();
            if (response != null)
                _cachingHelper.SetDataInCache<List<ProviderMasterContract>>(CachingConstants.ContractDetails, response);
        }

        private void FillCacheWithEntityRelationDetails()
        {
            var contracts = GetContractDetails();
            if (contracts == null || contracts.Count == 0)
                return;

            contracts.ForEach(y =>
            {
                FillCacheWithSingleEntityRelation(int.Parse(y.id));
            });
        }

        private void FillCacheWithSingleEntityRelation(int ContractId)
        {
            string key = string.Format("ER{0}", ContractId);
            _cachingHelper.RemoveCache(key);
            var erDetails = _utilityRepo.GetEntityRelationDetails(ContractId);
            if (erDetails != null)
                _cachingHelper.SetDataInCache<List<ProviderEntityRelation>>(key, erDetails);
        }
    }
}
