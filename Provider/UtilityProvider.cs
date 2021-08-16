using AuthUtility.Constants;
using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Interfaces;
using AuthUtility.Models.Supreme;
using AuthUtility.Models.UtilityModel;
using System.Collections.Generic;

namespace AuthUtility.Provider
{
    public class UtilityProvider : IUtilityProvider
    {
        private readonly IUtilityRepo _utilityRepo;
        private readonly ICacheProvider _cacheProvider;
        public UtilityProvider(IUtilityRepo utilityRepo, ICacheProvider cacheProvider)
        {
            _utilityRepo = utilityRepo;
            _cacheProvider = cacheProvider;
        }

        public Dictionary<int, string> GetDCCity()
        {
            return _cacheProvider.GetCities();
        }

        public Dictionary<int, string> GetDCState()
        {
            return _cacheProvider.GetStates();
        }

        public Dictionary<int, string> GetDCLocation()
        {
            return _cacheProvider.GetLocations();
        }

        public List<DCMasterProvider> GetDCEntity()
        {
            return _utilityRepo.GetDCEntity();
        }

        public List<ProviderEntityRelation> GetEntityRelationDetails(int contractId)
        {
            return _utilityRepo.GetEntityRelationDetails(contractId);
        }

        public List<Role> GetLoginTypeRoles(string searchfilter)
        {
            return _utilityRepo.GetLoginTypeRoles(searchfilter);
        }

        public List<ProviderMasterContract> GetContractDetails()
        {
            return _utilityRepo.GetContractDetails();
        }
        public List<EntityDTO> GetERDetails(int contractId)
        {
            return _utilityRepo.GetERDetails(contractId);
        }
    }
}
