using System;
using System.Collections.Generic;

namespace AuthUtility.Models.Supreme
{
    public class MasterLookUpRequest
    {

        public bool LookUpById { get; set; }
        public bool LookUpByColumnName { get; set; }
        public bool LookUpByColumnValue { get; set; }
        public bool LookUpByParentKey { get; set; }
        public List<int> Ids { get; set; }
        public List<string> ColumnNames { get; set; }
        public List<string> ColumnValues { get; set; }
        public List<int> ParentKeys { get; set; }
        public Operand Operation { get; set; }
    }
    public class MasterLookUpResponse
    {
        public List<LookUp> LookupData { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class LookUp
    {
        public int MasterLookUpID { get; set; }
        public string MasterLookUpColumnName { get; set; }
        public string MasterLookUpColumnValue { get; set; }
        public int MasterLookUpParentLookUpKey { get; set; }
        public int MasterLookUpSortOrder { get; set; }
        public bool MasterLookUpIsActive { get; set; }
        public int MasterLookUpAdduser { get; set; }
        public DateTime MasterLookUpCreatedOn { get; set; }
        public int MasterLookUpModifiedUser { get; set; }
        public DateTime MasterLookUpModifiedOn { get; set; }
    }
    public enum Operand
    {
        AND,
        OR
    }
}
