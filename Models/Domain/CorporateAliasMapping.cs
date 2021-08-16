using LinqToDB.Mapping;
using System;

namespace AuthUtility.Models.Domain
{
    [Table(Name = "tblCorporateAliasMapping")]
    public class CorporateAliasMapping
    {
        [Column, PrimaryKey, Identity]
        public int CAM_ID { get; set; }
        [Column]
        public long CAM_PolCorporateId { get; set; }
        [Column]
        public long? CAM_PolGroupCorporateId { get; set; }
        [Column]
        public string CAM_DBType { get; set; }
        [Column]
        public string CAM_Prefix { get; set; }
        [Column]
        public string CAM_LoginType { get; set; }
        [Column]
        public string CAM_Suffix { get; set; }
        [Column]
        public string CAM_PossibleAliases { get; set; }
        [Column]
        public bool CAM_IsActive { get; set; }
        [Column]
        public DateTime CAM_CreatedOn { get; set; }
        [Column]
        public int CAM_CreatedBy { get; set; }
        [Column]
        public DateTime CAM_ModifiedOn { get; set; }
        [Column]
        public int CAM_ModifiedBy { get; set; }
        [Column]
        public string PasswordType { get; set; }
    }
}
