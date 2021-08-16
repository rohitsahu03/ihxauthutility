using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Models;
using AuthUtility.Models.Supreme;
using AuthUtility.Models.UtilityModel;
using System.Collections.Generic;

namespace AuthUtility.Interfaces
{
    public interface IUtilityRepo
    {
        List<Role> GetLoginTypeRoles(string searchfilter);
        List<ProviderMasterContract> GetContractDetails();
        List<ProviderEntityRelation> GetEntityRelationDetails(int contractId);
        List<DCMasterProvider> GetDCEntity();
        Dictionary<int, string> GetDCCity();
        Dictionary<int, string> GetDCState();
        Dictionary<int, string> GetDCLocation();
        AuthGetTokenResponse GetBearer(AuthGetTokenRequest RequestObj, string AbsolutePath);
        BaseResponseAuth UpdateBaseProfile(UpdateBaseUserProfileRequest RequestObj);
        ProfileResponse UpdateAppBasedProfile(UpdateAppBasedProfileRequest RequestObj);
        UserCreationResponse CreateUser(UserCreation Model, string AbsolutePath);
        MasterLookUpResponse LookUpRequest(MasterLookUpRequest RequestObj);
        EntityListResponse FetchEntity(EntityListRequest RequestObj);
        List<EntityDTO> GetERDetails(int contractId);
    }
}
