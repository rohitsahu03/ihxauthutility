using AuthUtility.Models;
using System.Collections.Generic;

namespace AuthUtility.Interfaces
{
    public interface IBulkUpdateProvider
    {
        BaseResponse UpdateBulkUserDetails(UserDetailsBulkUpdate Model);
        BaseResponse UserBulkCreation(List<UserCreation> Model, bool IsVerifyContact,int ActionPerformedBy);
        BaseResponse RemoveElasticBaseProfile(RemoveElasticBaseProfile Model);
    }
}
