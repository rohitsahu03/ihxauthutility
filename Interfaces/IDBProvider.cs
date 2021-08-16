using AuthUtility.Models;
using System.Collections.Generic;

namespace AuthUtility.Interfaces
{
    public interface IDBProvider
    {
        BaseResponse ChangeEmail(UpdateEmail Model);
        BaseResponse ChangeMob(UpdateMob Model);
        BaseResponse UnlockUsers(UnlockUsers Model);
        BaseResponse ChangeActiveStatus(UpdateActiveStatus Model);
        BaseResponse UpdateRolesinAuth(UpdateRole Model);
        BaseResponse ChangePassword(UpdatePassword Model);
        BaseResponse UpdateIsOTPVerified(UpdateIsOTPVerified Model);
        BaseResponse UpdateLoginname(UpdateLoginname Model);
        BaseResponse UpdateFromCMS(UpdateFromCMS Model);
        BaseResponse CreateBrokerLogin(BrokerLogin Model);
        BaseResponse UpdateProclaimRole(UpdateProclaimRole Model);
        Dictionary<string, string> GetAllRoles(bool isFetchGroupKey = false);
        BaseResponse UpdateShortName(UpdateShortName Model);
        BaseResponse UpdateEntityId(UpdateEntityId Model);
        string GetNameFromEntityId(int EntityId);
        BaseResponse ResetPassword(ResetPassword Model);
        ManagerCreationDTO GetUserForManager(string LoginName, int UserId, string EmailId, string PhoneNo);
        AuthGetTokenResponse GetBearer(AuthGetTokenRequest RequestObj, string AbsolutePath);
        BaseResponse UpdateUser(ManagerCreationDTO Model);
        BaseResponse CreateUser(ManagerCreationDTO Model);
        Dictionary<string, string> GetPreviousAppBasedProfile(int UserId, string LoginType);
        BaseResponse SaveCorporateAlias(AliasMapping Model);
        AliasMapping GetAliasMappingForUi(long PolCorporateId);
        BaseResponse UpdateFirstLastName(UpdateFirstLastName Model);
        BaseResponse UpdatePasswordBulk(UpdatePasswordBulk Model);
    }
}
