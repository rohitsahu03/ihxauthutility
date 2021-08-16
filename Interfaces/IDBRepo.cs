using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Models;
using AuthUtility.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AuthUtility.Interfaces
{
    public interface IDBRepo
    {
        BaseResponse SaveInDBWithQuery(string Query);
        List<Role> GetAllRoles(string Query);
        BaseResponse UpdatePasswordAuth(List<int> UserId, string Password, int ActionPerformedBy);
        BaseResponse UpdateIsOTPVerified(List<int> TauIds, bool Status, int ActionPerformedBy, bool IsEmailVerified, bool IsMobileVerified);
        BaseResponse UpdateFromCMS(UpdateFromCMS Model);
        BaseResponse UpdateRolesinAuth(List<int> TauIds, List<string> SelectedRolesIds, Dictionary<string, string> AllRoles, bool IsDisablePreviousRoles, int ActionPerformedBy);
        Dictionary<int, string> GetAllBrokers();
        BaseResponse CreateBrokerLogin(BrokerLogin Model);
        BaseResponse UpdateProclaimRolesinAuth(UpdateProclaimRole Model);
        int GetDuplicateUserNames(string Query);
        string CheckValidityOfEntityId(int EntityId);
        string GetUserIdsFromEntityIdWithPagination(int EntityId, int Iteration);
        string GetDbNameByPolId(string PolId);
        CorporateAliasMapping GetAliasMapping(long PolCorporateId, bool ActiveRecordsOnly);
        List<int> GetAllUserIdsFromEntityId(int EntityId);
        UserDetailModel GetUserDetails(int UserId);
        List<T> GetTableRecordsFromExpression<T>(Expression<Func<T, bool>> searchfilter) where T : class;
        bool MapUserMultipleRoles(int UserId, List<int> RoleIds, int ActionPerformedBy);
        bool InsertInDB<T>(T Record) where T : class;
        bool UpdateInDB<T>(T Record) where T : class;
        BaseResponse UpdateUserDetailsInBulk(UserDetailsBulkUpdate Model);
        bool SaveAliasMapping(CorporateAliasMapping Mapping);
    }
}
