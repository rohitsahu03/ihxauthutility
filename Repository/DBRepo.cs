using Authutility.Enums;
using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.DomainObject.MediBuddy;
using AuthUtility.Interfaces;
using AuthUtility.MediBuddyDBFactory;
using AuthUtility.Models;
using AuthUtility.Models.Domain;
using Dapper;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace AuthUtility.DBRepository
{
    public class DBRepo : IDBRepo
    {
        private readonly ConfigManager _configManager;
        private readonly ILogger _logger;
        public DBRepo(ILogger<DBRepo> logger, ConfigManager ConfigManager)
        {
            this._configManager = ConfigManager;
            this._logger = logger;
        }
        public BaseResponse SaveInDBWithQuery(string Query)
        {
            BaseResponse result = new BaseResponse();
            SqlTransaction trans = null;
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        SqlCommand command = new SqlCommand(Query, con, trans);
                        int i = command.ExecuteNonQuery();
                        if (i > 0)
                        {
                            trans.Commit();
                            result.IsSuccess = true;
                            result.Message = MsgConstants.SuccessMsg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (trans.IsNotNull())
                    trans.Rollback();
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateInDB:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public List<Role> GetAllRoles(string Query)
        {
            if (string.IsNullOrEmpty(Query))
                return default;

            List<Role> roles = new List<Role>();
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (var trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        SqlCommand command = new SqlCommand(Query, con, trans);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            roles.Add(new Role
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["Name"].ToString(),
                                GroupKey = row["GroupKey"].ToString(),
                                IsActive = Convert.ToBoolean(row["IsActive"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetAllRoles:" + ex.Message + Environment.NewLine);
            }
            return roles;
        }

        public BaseResponse UpdatePasswordAuth(List<int> UserId, string Password, int ActionPerformedBy)
        {
            if (!UserId.HasRecords() || string.IsNullOrEmpty(Password) || ActionPerformedBy == default)
                return default;

            BaseResponse result = new BaseResponse();
            SqlTransaction trans = null;
            string ids = string.Join(",", UserId);
            var password = PasswordHelper.StrToByteArray(PasswordHelper.GenerateMD5(Password));
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (trans = con.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        SqlCommand command = new SqlCommand(string.Format(SqlQueries.UpdatePasswordinAuth, ids), con, trans);
                        SqlParameter param1 = command.Parameters.Add("@Password", SqlDbType.VarBinary);
                        param1.Value = password;
                        SqlParameter param2 = command.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                        param2.Value = ActionPerformedBy;
                        //SqlParameter param3 = command.Parameters.Add("@TAU_Id", SqlDbType.VarChar);
                        //param3.Value = ids;
                        int i = command.ExecuteNonQuery();
                        if (i > 0)
                        {
                            trans.Commit();
                            result.IsSuccess = true;
                            result.Message = MsgConstants.SuccessMsg;
                            Task.Factory.StartNew(() => UpdatePasswordInSecondColumn(ids, Password));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (trans.IsNotNull())
                    trans.Rollback();
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdatePasswordAuth:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public BaseResponse UpdateIsOTPVerified(List<int> TauIds, bool Status, int ActionPerformedBy, bool IsEmailVerified, bool IsMobileVerified)
        {
            if (!TauIds.HasRecords() || (!IsEmailVerified && !IsMobileVerified))
                return default;

            BaseResponse result = new BaseResponse() { IsSuccess = true };
            List<string> checkedBoxes = new List<string>();
            int bitvalue = Status ? 1 : 0;
            if (IsEmailVerified)
            {
                string query = string.Format(SqlQueries.UpdateIsEmailVerified, bitvalue, DateTime.Now.ToString(), ActionPerformedBy, string.Join(",", TauIds));
                result = SaveInDBWithQuery(query);
                checkedBoxes.Add(UtilityConstants.IsEmailVerfied);
            }
            if (IsMobileVerified && result.IsSuccess)
            {
                string query = string.Format(SqlQueries.UpdateIsMobileVerified, bitvalue, DateTime.Now.ToString(), ActionPerformedBy, string.Join(",", TauIds));
                result = SaveInDBWithQuery(query);
                checkedBoxes.Add(UtilityConstants.IsOTPVerified);
            }
            //Task.Factory.StartNew(() =>
            //{
            //    SqlTransaction trans = null;
            //    try
            //    {
            //        if (checkedBoxes.Count > 0)
            //        {
            //            DataTable dt = new DataTable();
            //            dt.Columns.Add("Id", typeof(int));
            //            dt.Columns.Add("Name", typeof(string));
            //            dt.Columns.Add("Value", typeof(string));
            //            foreach (var t in Model.TauIds.Split(','))
            //            {
            //                foreach (var b in checkedBoxes)
            //                {
            //                    dt.Rows.Add(int.Parse(t), b, Model.Status.ToString());
            //                }
            //            }

            //            using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
            //            {
            //                con.Open();
            //                using (trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
            //                {
            //                    SqlCommand command = new SqlCommand(SqlQueries.MergeQueryProperty, con, trans);
            //                    SqlParameter param1 = new SqlParameter("@SourceTable", SqlDbType.Structured)
            //                    {
            //                        TypeName = UtilityConstants.TypePropertyMerge,
            //                        Value = dt
            //                    };
            //                    command.Parameters.Add(param1);
            //                    SqlParameter param2 = new SqlParameter("@UserTauId", SqlDbType.Int)
            //                    {
            //                        Value = Model.ActionPerformedBy
            //                    };
            //                    command.Parameters.Add(param2);
            //                    SqlParameter param3 = new SqlParameter("@AppId", SqlDbType.VarChar)
            //                    {
            //                        Value = UtilityConstants.AuthUtilityAppId
            //                    };
            //                    command.Parameters.Add(param3);
            //                    int i = command.ExecuteNonQuery();
            //                    if (i > 0)
            //                    {
            //                        trans.Commit();
            //                        //result.IsSuccess = true;
            //                        //result.Message = string.Format(UtilityConstants.SuccessMsgWithCount, GetNumberOfUsersAffected(dt));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        if (_logger != null)
            //            _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateIsOTPVerified:" + ex.Message + Environment.NewLine);
            //    }
            //});
            return result;
        }

        public string GetDbNameByPolId(string PolId)
        {
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(string.Format(SqlQueries.GetDbNameByPolId, PolId), con);
                    var dbName = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(dbName))
                        return dbName.ToString();
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetDbNameByPolId:" + ex.Message + Environment.NewLine);
            }
            return null;
        }

        public BaseResponse UpdateFromCMS(UpdateFromCMS Model)
        {
            BaseResponse result = new BaseResponse();
            SqlTransaction trans = null;
            try
            {
                SqlCommand cmd1 = null, cmd2 = null;
                DataTable dt = new DataTable();
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        string dbName = GetDbNameByPolId(Model.PolId);
                        if (!string.IsNullOrEmpty(dbName))
                        {
                            cmd1 = new SqlCommand(string.Format(SqlQueries.GetPropertiesFromCMS, Model.UsernameFormat, Model.TauIds, dbName, Model.PolId), con, trans);
                            SqlDataAdapter da = new SqlDataAdapter(cmd1);
                            da.Fill(dt);
                            cmd2 = new SqlCommand(SqlQueries.MergeQueryProperty, con, trans);
                            SqlParameter param1 = new SqlParameter("@SourceTable", SqlDbType.Structured)
                            {
                                TypeName = UtilityConstants.TypePropertyMerge,
                                Value = dt
                            };
                            cmd2.Parameters.Add(param1);
                            SqlParameter param2 = new SqlParameter("@UserTauId", SqlDbType.Int)
                            {
                                Value = Model.ActionPerformedBy
                            };
                            cmd2.Parameters.Add(param2);
                            SqlParameter param3 = new SqlParameter("@AppId", SqlDbType.VarChar)
                            {
                                Value = UtilityConstants.AuthUtilityAppId
                            };
                            cmd2.Parameters.Add(param3);

                            if (cmd2.ExecuteNonQuery() > 0)
                            {
                                trans.Commit();
                                result.IsSuccess = true;
                                var effectedUsers = GetTotalUsersAffected(dt);
                                result.Message = string.Format(MsgConstants.SuccessMsgWithCount, effectedUsers.Count);

                                if (Model.UsernameFormat == UtilityConstants.EmployeeCodeFormat && dt != null && dt.Rows.Count > 0)
                                    Task.Factory.StartNew(() => UpdateEntityId(Model.TauIds, Model.PolId, dbName, Model.ActionPerformedBy));
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = MsgConstants.InvalidPolIdMsg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (trans.IsNotNull())
                    trans.Rollback();
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateFromCMS:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public BaseResponse UpdateRolesinAuth(List<int> TauIds, List<string> SelectedRolesIds, Dictionary<string, string> AllRoles, bool IsDisablePreviousRoles, int ActionPerformedBy)
        {
            if (!TauIds.HasRecords() || !SelectedRolesIds.HasRecords() || !AllRoles.HasRecords())
                return default;

            BaseResponse result = new BaseResponse();
            SqlTransaction trans = null;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Role", typeof(int));
                dt.Columns.Add("IsActive", typeof(bool));
                foreach (var userid in TauIds)
                {
                    foreach (var roleid in AllRoles.Keys)
                    {
                        if (SelectedRolesIds.Contains(roleid))
                            dt.Rows.Add(userid, roleid, true);
                        //else
                        // dt.Rows.Add (int.Parse (userid), b, false);
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    bool prevDisabled = true;
                    if (IsDisablePreviousRoles)
                        prevDisabled = DisablePreviousRoles(dt, ActionPerformedBy.ToString(), RoleType.MediAuth);

                    if (prevDisabled)
                    {
                        using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                        {
                            con.Open();
                            using (trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                            {
                                SqlCommand command = new SqlCommand(SqlQueries.MergeQueryRole, con, trans);
                                SqlParameter param1 = new SqlParameter("@SourceTable", SqlDbType.Structured)
                                {
                                    TypeName = UtilityConstants.TypeRoleMerge,
                                    Value = dt
                                };
                                command.Parameters.Add(param1);
                                SqlParameter param2 = new SqlParameter("@UserTauId", SqlDbType.Int)
                                {
                                    Value = ActionPerformedBy
                                };
                                command.Parameters.Add(param2);
                                int i = command.ExecuteNonQuery();
                                if (i > 0)
                                {
                                    trans.Commit();
                                    result.IsSuccess = true;
                                    result.Message = string.Format(MsgConstants.SuccessMsgWithCount, GetTotalUsersAffected(dt).Count);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (trans.IsNotNull())
                    trans.Rollback();
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateRolesinAuth:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public Dictionary<int, string> GetAllBrokers()
        {
            Dictionary<int, string> brokers = null;
            SqlTransaction trans = null;
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        brokers = new Dictionary<int, string>();
                        SqlCommand command = new SqlCommand(SqlQueries.GetAllBrokers, con, trans);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            brokers.Add(Convert.ToInt32(row["Id"]), row["BrokerName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetAllBrokers:" + ex.Message + Environment.NewLine);
            }
            return brokers;
        }

        public BaseResponse CreateBrokerLogin(BrokerLogin Model)
        {
            BaseResponse result = new BaseResponse();
            SqlTransaction trans = null;
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        SqlCommand cmd = new SqlCommand(SqlQueries.sp_brokerLogin, con, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@BrokerName", SqlDbType.VarChar).Value = Model.BrokerName.Replace(" ", string.Empty);
                        cmd.Parameters.Add("@BrokerId", SqlDbType.Int).Value = Model.BrokerId;
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            trans.Commit();
                            result.IsSuccess = true;
                            result.Message = MsgConstants.SuccessMsg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (trans.IsNotNull())
                    trans.Rollback();
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in CreateBrokerLogin:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public BaseResponse UpdateProclaimRolesinAuth(UpdateProclaimRole Model)
        {
            BaseResponse result = new BaseResponse();
            SqlTransaction trans = null;
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Role", typeof(int));
                dt.Columns.Add("IsActive", typeof(bool));
                foreach (var userid in Model.UserIds.Split(','))
                {
                    foreach (var roleid in Model.AllRoles.Keys)
                    {
                        if (Model.SelectedRolesIds.Contains(roleid))
                            dt.Rows.Add(int.Parse(userid), roleid, true);
                        //else
                        // dt.Rows.Add (int.Parse (userid), role, false);
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    bool prevDisabled = true;
                    if (Model.DisablePreviousRoles)
                        prevDisabled = DisablePreviousRoles(dt, Model.ActionPerformedBy.ToString(), RoleType.Proclaim);

                    if (prevDisabled)
                    {
                        using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                        {
                            con.Open();
                            using (trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                            {

                                SqlCommand command = new SqlCommand(SqlQueries.MergeQueryProclaimRole, con, trans);
                                SqlParameter param1 = new SqlParameter("@SourceTable", SqlDbType.Structured)
                                {
                                    TypeName = UtilityConstants.TypeRoleMerge,
                                    Value = dt
                                };
                                command.Parameters.Add(param1);
                                SqlParameter param2 = new SqlParameter("@UserTauId", SqlDbType.VarChar)
                                {
                                    Value = Model.ActionPerformedBy.ToString()
                                };
                                command.Parameters.Add(param2);
                                int i = command.ExecuteNonQuery();
                                if (i > 0)
                                {
                                    trans.Commit();
                                    result.IsSuccess = true;
                                    result.Message = string.Format(MsgConstants.SuccessMsgWithCount, GetTotalUsersAffected(dt).Count);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                if (trans.IsNotNull())
                    trans.Rollback();
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateProclaimRolesinAuth:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public int GetDuplicateUserNames(string Query)
        {
            int result = default(int);
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (SqlTransaction trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        SqlCommand command = new SqlCommand(Query, con, trans);
                        result = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetDuplicateUserNames:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public string CheckValidityOfEntityId(int EntityId)
        {
            string result = string.Empty;
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (SqlTransaction trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        string query = string.Format(SqlQueries.CheckIfEntityIdIsCorrect, EntityId);
                        SqlCommand command = new SqlCommand(query, con, trans);
                        result = (string)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetDuplicateUserNames:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public string GetUserIdsFromEntityIdWithPagination(int EntityId, int Iteration)
        {
            string result = null;
            try
            {
                using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    con.Open();
                    using (var trans = con.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        string query = string.Format(SqlQueries.GetTauIdsFromEntityId, Iteration, EntityId);
                        SqlCommand command = new SqlCommand(query, con, trans);
                        result = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetUserIdsFromEntityId:" + ex.Message + Environment.NewLine);
            }
            return result;
        }

        public CorporateAliasMapping GetAliasMapping(long PolCorporateId, bool ActiveRecordsOnly)
        {
            try
            {
                Expression<Func<CorporateAliasMapping, bool>> expr = x => x.CAM_PolCorporateId == PolCorporateId;
                if (ActiveRecordsOnly)
                    expr.And(x => x.CAM_IsActive);

                var mapping = DBFactory.GetTable<CorporateAliasMapping>(expr);
                if (mapping.HasRecords())
                    return mapping.First();
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetAliasMapping:" + Ex.Message + Environment.NewLine);
            }
            return null;
        }

        public List<int> GetAllUserIdsFromEntityId(int EntityId)
        {
            try
            {
                string query = string.Format(SqlQueries.GetTauIds, EntityId);
                return DBFactory.ExecuteQuery<int>(query);
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetAllUserIdsFromEntityId:" + Ex.Message + Environment.NewLine);
            }
            return null;
        }

        //TODO: Remove Dapper and use Linq2DB
        public UserDetailModel GetUserDetails(int UserId)
        {
            if (!(UserId > 0))
                return null;

            UserDetailModel response = new UserDetailModel();
            try
            {
                using (IDbConnection con = new SqlConnection(_configManager.ConnectionStringAuth))
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@TAUID", UserId);
                    var multi = con.QueryMultiple(SqlQueries.SpUserDetail, dynamicParams, commandType: CommandType.StoredProcedure);
                    response.User = multi.Read<ApplicationUser>().SingleOrDefault();
                    response.UserProperties = multi.Read<ApplicationUserProperty>().ToList();
                    response.AccessDetail = multi.Read<UserAccess>().ToList();
                }
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in GetUserDetails:" + Ex.Message + Environment.NewLine);
            }
            return response;
        }

        public List<T> GetTableRecordsFromExpression<T>(Expression<Func<T, bool>> searchfilter) where T : class
        {
            if (searchfilter == null)
                return null;

            return DBFactory.GetTable<T>(searchfilter);
        }

        public bool MapUserMultipleRoles(int UserId, List<int> RoleIds, int ActionPerformedBy)
        {
            bool response = true;
            try
            {
                if (UserId != 0)
                {
                    List<string> groupKeys = null;
                    if (!string.IsNullOrEmpty(UtilityConstants.GroupKeys))
                        groupKeys = UtilityConstants.GroupKeys.Split(',').ToList();

                    if (groupKeys == null || groupKeys.Count == 0)
                        return false;

                    List<Role> allowedRoles = DBFactory.GetTable<Role>(x => x.IsActive == true && groupKeys.Contains(x.GroupKey));
                    var roles = GetTableRecordsFromExpression<UserMapRoles>(x => x.TUMR_TAU_Id == UserId && allowedRoles.Select(y => y.Id).ToList().Contains(x.TUMR_Role));
                    if (RoleIds != null && RoleIds.Count > 0)
                    {
                        if (roles != null)
                        {
                            var prevRoleIds = roles.Where(x => x.TUMR_IsActive == true).Select(x => x.TUMR_Role).ToList();
                            if (RoleIds.SequenceEqual(prevRoleIds))
                                return true;

                            roles.ForEach(x =>
                            {
                                x.TUMR_IsActive = RoleIds.Contains(x.TUMR_Role);
                                x.TUMR_ModifiedOn = DateTime.Now;
                                x.TUMR_ModifiedBy = ActionPerformedBy;
                                if (x.TUMR_IsActive)
                                    RoleIds.Remove(x.TUMR_Role);

                                DBFactory.Update<UserMapRoles>(x);
                            });
                        }
                        RoleIds.ForEach(x =>
                        {
                            try
                            {
                                UserMapRoles role = new UserMapRoles()
                                {
                                    TUMR_TAU_Id = UserId,
                                    TUMR_Role = x,
                                    TUMR_IsActive = true,
                                    TUMR_CreatedOn = DateTime.Now,
                                    TUMR_CreatedBy = ActionPerformedBy,
                                    TUMR_ModifiedBy = ActionPerformedBy,
                                    TUMR_ModifiedOn = DateTime.Now
                                };
                                DBFactory.Insert<UserMapRoles>(role);
                            }
                            catch (Exception Ex)
                            {
                                if (_logger != null)
                                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in Inserting role in MapUserMultipleRoles: " + Ex.Message + " For UserId: " + UserId + Environment.NewLine);
                            }
                        });
                    }
                    else if (roles != null)
                    {
                        roles.ForEach(x =>
                        {
                            x.TUMR_IsActive = false;
                            x.TUMR_ModifiedOn = DateTime.Now;
                            x.TUMR_ModifiedBy = ActionPerformedBy;
                            DBFactory.Update<UserMapRoles>(x);
                        });
                    }
                }
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in MapUserMultipleRoles:" + Ex.Message + Environment.NewLine);

                response = false;
            }
            return response;
        }

        public bool InsertInDB<T>(T Record) where T : class
        {
            try
            {
                return DBFactory.Insert<T>(Record);
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in InsertInDB:" + Ex.Message + " " + JsonConvert.SerializeObject(Record) + Environment.NewLine);

                return false;
            }
        }

        public bool UpdateInDB<T>(T Record) where T : class
        {
            try
            {
                return DBFactory.Update<T>(Record);
            }
            catch (Exception Ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateInDB:" + Ex.Message + " " + JsonConvert.SerializeObject(Record) + Environment.NewLine);

                return false;
            }
        }

        public BaseResponse UpdateUserDetailsInBulk(UserDetailsBulkUpdate Model)
        {
            if (Model.IsNull() || !Model.UserDetails.HasRecords())
                return new BaseResponse() { Message = MsgConstants.NoDataFound };

            if (Model.UserDetails.Any(x => x.UserId == 0))
            {
                var invalidUserNames = Model.UserDetails.Where(x => x.UserId == 0)
                                                        .Select(x => x.OldUserName)
                                                        .ToList();
                return new BaseResponse()
                {
                    Message = MsgConstants.UnableToGetUserId + (invalidUserNames.HasRecords() ? " => " + string.Join(",", invalidUserNames) : "")
                };
            }

            try
            {
                DataTable dt = CreateDataTable<UserDetails>(Model.UserDetails);
                DataParameter[] parameters = new[]
                {
                    new DataParameter("@SourceTable",dt,DataType.Structured,UtilityConstants.TypeBulkUpdateUserDetails),
                    new DataParameter("@UserTauId", Model.ActionPerformedBy, DataType.Int32),
                    new DataParameter("@AppId", UtilityConstants.AuthUtilityAppId)
                };

                bool result = DBFactory.ExecuteQueryWithParams(SqlQueries.UpdateBulkUserDetails, parameters);

                return new BaseResponse() { IsSuccess = result, Message = result == false ? MsgConstants.ServerError : null };
            }
            catch (Exception Ex)
            {

                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " Error in UpdateUserDetailsInBulk:" + Ex.Message + " " + JsonConvert.SerializeObject(Model) + Environment.NewLine);

                return new BaseResponse() { Message = Ex.Message };
            }
        }

        //public List<int> GetUserIds(List<string> UserNames)
        //{
        //    List<ApplicationUser> users = DBFactory.GetTable<ApplicationUser>(x => x.TAU_IsActive && UserNames.Contains(x.TAU_LoginName));
        //    if (users != null && users.Count > 0)
        //        return users.Select(x => x.TAU_Id).ToList();

        //    return null;
        //}

        public bool SaveAliasMapping(CorporateAliasMapping Mapping)
        {
            if (Mapping.IsNull())
                return false;

            if (Mapping.CAM_ID > 0)
                return DBFactory.Update<CorporateAliasMapping>(Mapping);
            else
                return DBFactory.Insert<CorporateAliasMapping>(Mapping);
        }

        private DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private long GetEntityIdFromPolId(string PolId, string DBName)
        {
            using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
            {
                con.Open();
                string query = string.Format(SqlQueries.GetEntityIdFromPolId, DBName, PolId);
                SqlCommand command = new SqlCommand(query, con);
                return (long)command.ExecuteScalar();
            }
        }

        private bool UpdateEntityId(string UserIds, string PolId, string DBName, int ActionPerformedBy)
        {
            if (string.IsNullOrEmpty(UserIds))
                return false;

            long EntityId = GetEntityIdFromPolId(PolId, DBName);
            if (EntityId == 0)
                return false;

            string query = string.Format(SqlQueries.UpdateEntityIdFromCMS, EntityId, ActionPerformedBy, UserIds);

            BaseResponse result = SaveInDBWithQuery(query);
            if (result != null)
                return result.IsSuccess;

            return false;
        }

        private List<int> GetTotalUsersAffected(DataTable DataTable)
        {
            List<int> tauids = new List<int>();
            foreach (DataRow row in DataTable.Rows)
            {
                tauids.Add((int)row["Id"]);
            }
            return tauids.Select(x => x).Distinct().ToList();
        }

        private bool DisablePreviousRoles(DataTable DataTable, string ActionPerformedBy, RoleType Type)
        {
            string query = null;
            if (Type == RoleType.Proclaim)
                query = SqlQueries.DeactivatePreviousRolesinProclaim;
            else if (Type == RoleType.MediAuth)
                query = SqlQueries.DeactivatePreviousRolesinAuth;

            using (var con = new SqlConnection(_configManager.ConnectionStringAuth))
            {
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                SqlParameter param1 = new SqlParameter("@SourceTable", SqlDbType.Structured)
                {
                    TypeName = UtilityConstants.TypeRoleMerge,
                    Value = DataTable
                };
                command.Parameters.Add(param1);
                SqlParameter param2 = new SqlParameter("@UserTauId", SqlDbType.VarChar)
                {
                    Value = ActionPerformedBy
                };
                command.Parameters.Add(param2);
                int i = command.ExecuteNonQuery();
                return i > 0;
            }
        }

        private void UpdatePasswordInSecondColumn(string Tauid, string Password)
        {
            string query = string.Format(SqlQueries.InsertPasswordInSecondColumn, Tauid, Password);
            SaveInDBWithQuery(query);
        }

    }
}