using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB;
using LinqToDB.Data;

namespace AuthUtility.MediBuddyDBFactory
{
    public class DBFactory
    {
        public static List<T> ExecuteProcedure<T>(string SpName, params DataParameter[] Parameters)
        {
            List<T> response = new List<T>();
            using (var db = new MediAuthConnection())
            using (var trans = db.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                db.CommandTimeout = 0;
                response = db.QueryProc<T>(SpName, Parameters).ToList();
            }
            return response;
        }

        public static List<T> ExecuteQuery<T>(string Query)
        {
            List<T> response = new List<T>();
            using (var db = new MediAuthConnection())
            {
                response = db.Query<T>(Query).ToList();
            }
            return response;
        }

        public static bool Insert<T>(T Record) where T : class
        {
            using (var db = new MediAuthConnection())
            {
                int rowsEffected = db.Insert<T>(Record);
                return rowsEffected > 0;
            }
        }

        public static bool InsertAll<T>(List<T> Records) where T : class
        {
            DataConnectionTransaction trans = null;
            try
            {
                using (var db = new MediAuthConnection())
                using (trans = db.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    BulkCopyRowsCopied response = db.BulkCopy<T>(Records);
                    trans.Commit();
                    if (response != null)
                        return response.RowsCopied > 0;
                }
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                throw Ex;
            }
            return false;
        }

        public static List<T> GetTable<T>(Expression<Func<T, bool>> Expression) where T : class
        {
            List<T> tableRecords = null;
            using (var db = new MediAuthConnection())
            using (var trans = db.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                tableRecords = db.GetTable<T>().Where(Expression).ToList();
            }
            return tableRecords;
        }

        public static List<T> GetTable<T, TKey>(
                                                Expression<Func<T, bool>> predicate,
                                                int pageNo,
                                                int pageSize,
                                                Expression<Func<T, TKey>> sorter,
                                                out long totalRecords)
                                                where T : class
        {
            List<T> response = null;
            using (var db = new MediAuthConnection())
            using (var trans = db.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                totalRecords = db.GetTable<T>()
                                 .Where(predicate)
                                 .Count();
                if (totalRecords > 0)
                    response = db.GetTable<T>()
                                 .Where(predicate)
                                 .OrderBy(sorter)
                                 .Skip((pageNo - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();
            }
            return response;
        }

        public static bool ExecuteNonQuery(string Query)
        {
            bool response = default(bool);
            DataConnectionTransaction trans = null;
            try
            {
                using (var db = new MediAuthConnection())
                using (trans = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    int count = db.Execute(Query);
                    if (count > 0)
                    {
                        trans.Commit();
                        response = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                throw Ex;
            }
            return response;
        }

        public static bool ExecuteQueryWithParams(string Query, params DataParameter[] Parameters)
        {
            bool response = default(bool);
            DataConnectionTransaction trans = null;
            try
            {
                using (var db = new MediAuthConnection())
                using (trans = db.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    int count = db.Execute(Query, Parameters);
                    if (count > 0)
                    {
                        trans.Commit();
                        response = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                throw Ex;
            }
            return response;
        }

        public static bool Update<T>(T Record) where T : class
        {
            using (var db = new MediAuthConnection())
            {
                int rowsEffected = db.Update<T>(Record);
                return rowsEffected > 0;
            }
        }

        public static int InsertAndReturnIdentity<T>(T Record) where T : class
        {
            using (var db = new MediAuthConnection())
            using (var trans = db.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                int identity = db.InsertWithInt32Identity<T>(Record);
                return identity;
            }
        }
        public static bool InsertOrReplace<T>(T Record) where T : class
        {
            using (var db = new MediAuthConnection())
            {
                int rowsEffected = db.InsertOrReplace<T>(Record);
                return rowsEffected > 0;
            }
        }
    }
}