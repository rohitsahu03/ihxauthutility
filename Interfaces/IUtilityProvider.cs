using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Models.Supreme;
using AuthUtility.Models.UtilityModel;
using System.Collections.Generic;

namespace AuthUtility.Interfaces
{
    public interface IUtilityProvider
    {
        List<Role> GetLoginTypeRoles(string searchfilter);
        List<ProviderMasterContract> GetContractDetails();
        List<ProviderEntityRelation> GetEntityRelationDetails(int contractId);
        List<DCMasterProvider> GetDCEntity();
        Dictionary<int, string> GetDCCity();
        Dictionary<int, string> GetDCState();
        Dictionary<int, string> GetDCLocation();
        List<EntityDTO> GetERDetails(int contractId);
    }
}
