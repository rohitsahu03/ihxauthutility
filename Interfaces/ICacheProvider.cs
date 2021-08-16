using AuthUtility.Models.UtilityModel;
using System.Collections.Generic;

namespace AuthUtility.Interfaces
{
    public interface ICacheProvider
    {
        void RefreshCache();
        Dictionary<int, string> GetCities();
        Dictionary<int, string> GetStates();
        Dictionary<int, string> GetLocations();
        Dictionary<string, string> GetAuthRoles(bool isFetchGroupKey = false);
        Dictionary<string, string> GetProclaimRoles();
        List<ProviderEntityRelation> GetAllDCDetails();
        List<DCMasterProvider> GetDCMaster();
        List<ProviderMasterContract> GetContractDetails();
        List<ProviderEntityRelation> GetEntityRelation(int ContractId);
        Dictionary<long, string> GetEntities();
    }
}
