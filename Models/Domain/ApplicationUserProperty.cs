using System;
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace AuthUtility.DomainObject.MediBuddy
{
    [Table(Name = "TblApplicationUserProperty")]
    public class ApplicationUserProperty
    {
        [Column, PrimaryKey, Identity]
        public int TAUP_Id { get; set; }
        [Column]
        public int TAUP_TAU_Id { get; set; }
        [Column]
        public string TAUP_Name { get; set; }
        [Column]
        public string TAUP_Value { get; set; }
        [Column]
        public bool TAUP_IsActive { get; set; }
        [Column]
        public string TAUP_CratedByApp { get; set; }
        [Column]
        public DateTime TAUP_CreatedOn { get; set; }
        [Column]
        public int TAUP_CreatedBy { get; set; }
        [Column]
        public DateTime? TAUP_ModifiedOn { get; set; }
        [Column]
        public int TAUP_ModifitedBy { get; set; }
        [Column]
        public string TAUP_GroupName { get; set; }
        public ApplicationUserProperty()
        {

        }
        public ApplicationUserProperty(int UserId, string AppId, string Name, string Value, int ActionPerformedBy, string GroupName = null, bool Active = true)
        {
            this.TAUP_TAU_Id = UserId;
            this.TAUP_CratedByApp = AppId;
            this.TAUP_IsActive = Active;
            this.TAUP_Name = Name;
            this.TAUP_Value = Value;
            this.TAUP_ModifiedOn = DateTime.Now;
            this.TAUP_CreatedOn = DateTime.Now;
            this.TAUP_GroupName = GroupName;
            this.TAUP_CreatedBy = ActionPerformedBy;
            this.TAUP_ModifitedBy = ActionPerformedBy;
        }
    }

    public class ExtendedApplicationUser : ApplicationUser
    {
        // public ApplicationUser User { get; set; }
        public List<ApplicationUserProperty> Properties { get; set; }
        public List<Role> Roles { get; set; }

        public ExtendedApplicationUser()
        {

        }
        public ExtendedApplicationUser(ApplicationUser _user)
        {
            this.TAU_Id = _user.TAU_Id;
            this.TAU_MBId = _user.TAU_MBId;
            this.TAU_FirstName = _user.TAU_FirstName;
            this.TAU_LastName = _user.TAU_LastName;
            this.TAU_MiddleName = _user.TAU_MiddleName;
            this.TAU_LoginName = _user.TAU_LoginName;
            this.TAU_Password = _user.TAU_Password;
            this.TAU_ProviderMasterEntityId = _user.TAU_ProviderMasterEntityId;
            this.TAU_EmailId = _user.TAU_EmailId;
            this.TAU_AltEmailId = _user.TAU_AltEmailId;
            this.TAU_PhoneNumber = _user.TAU_PhoneNumber;
            this.TAU_AltPhoneNumber = _user.TAU_AltPhoneNumber;
            this.TAU_IsActive = _user.TAU_IsActive;
            this.TAU_IsLocked = _user.TAU_IsLocked;
            this.TAU_FailedAttemptCount = _user.TAU_FailedAttemptCount;
            this.TAU_Createdby = _user.TAU_Createdby;
            this.TAU_CreatedOn = _user.TAU_CreatedOn;
            this.TAU_Password1 = _user.TAU_Password1;
            this.Modifiedon = _user.Modifiedon;
            this.TAU_HasLoggedIn = _user.TAU_HasLoggedIn;
            this.TAU_AccountLockedOn = _user.TAU_AccountLockedOn;
            this.TAU_IsMobileVerified = _user.TAU_IsMobileVerified;
            this.TAU_IsEmailVerified = _user.TAU_IsEmailVerified;
            this.TAU_MobileVerifiedModifiedOn = _user.TAU_MobileVerifiedModifiedOn;
            this.TAU_EmailVerifiedModifiedOn = _user.TAU_EmailVerifiedModifiedOn;
        }
    }
}
