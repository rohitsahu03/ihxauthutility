using System;
using LinqToDB.Mapping;

namespace AuthUtility.DomainObject.MediBuddy
{
    [Table(Name = "TblApplicationUser")]
    public class ApplicationUser
    {
        [Column, PrimaryKey, Identity]
        public int TAU_Id { get; set; }
        [Column]
        public string TAU_MBId { get; set; }
        [Column]
        public string TAU_FirstName { get; set; }
        [Column]
        public string TAU_LastName { get; set; }
        [Column]
        public string TAU_MiddleName { get; set; }
        [Column]
        public string TAU_LoginName { get; set; }
        [Column]
        public byte[] TAU_Password { get; set; }
        [Column]
        public int TAU_ProviderMasterEntityId { get; set; }
        [Column]
        public string TAU_EmailId { get; set; }
        [Column]
        public string TAU_AltEmailId { get; set; }
        [Column]
        public string TAU_PhoneNumber { get; set; }
        [Column]
        public string TAU_AltPhoneNumber { get; set; }
        [Column]
        public bool TAU_IsActive { get; set; }
        [Column]
        public bool TAU_IsLocked { get; set; }
        [Column]
        public int TAU_FailedAttemptCount { get; set; }
        [Column]
        public string TAU_Createdby { get; set; }
        [Column]
        public DateTime TAU_CreatedOn { get; set; }
        [Column]
        public byte[] TAU_Password1 { get; set; }
        [Column]
        public DateTime Modifiedon { get; set; }
        [Column]
        public bool TAU_HasLoggedIn { get; set; }
        [Column]
        public string TAU_AccountLockedOn { get; set; }
        [Column]
        public int TAU_Modifiedby { get; set; }
        [Column]
        public bool TAU_IsMobileVerified { get; set; }
        [Column]
        public bool TAU_IsEmailVerified { get; set; }
        [Column]
        public string TAU_MobileVerifiedModifiedOn { get; set; }
        [Column]
        public string TAU_EmailVerifiedModifiedOn { get; set; }
    }
}