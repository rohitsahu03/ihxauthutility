using AuthUtility.DomainObject.MediBuddy;
using System;
using System.Collections.Generic;

namespace AuthUtility.Models
{
    public class UserBasicDetails
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ProviderMasterEntityId { get; set; }
        public string Gender { get; set; }
        public long MAID { get; set; }
        public string EmployeeId { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfMarriage { get; set; }
    }
    public class UserDetailModel
    {
        public ApplicationUser User { get; set; }
        public List<ApplicationUserProperty> UserProperties { get; set; }
        public List<UserAccess> AccessDetail { get; set; }
    }
    //public class ApplicationUser
    //{
    //    public int TAU_Id { get; set; }        
    //    public string TAU_MBId { get; set; }        
    //    public string TAU_FirstName { get; set; }        
    //    public string TAU_LastName { get; set; }        
    //    public string TAU_MiddleName { get; set; }        
    //    public string TAU_LoginName { get; set; }        
    //    public byte[] TAU_Password { get; set; }        
    //    public int TAU_ProviderMasterEntityId { get; set; }        
    //    public string TAU_EmailId { get; set; }        
    //    public string TAU_AltEmailId { get; set; }        
    //    public string TAU_PhoneNumber { get; set; }        
    //    public string TAU_AltPhoneNumber { get; set; }        
    //    public bool TAU_IsActive { get; set; }        
    //    public bool TAU_IsLocked { get; set; }        
    //    public int TAU_FailedAttemptCount { get; set; }        
    //    public string TAU_Createdby { get; set; }        
    //    public DateTime TAU_CreatedOn { get; set; }        
    //    public byte[] TAU_Password1 { get; set; }        
    //    public DateTime Modifiedon { get; set; }        
    //    public bool TAU_HasLoggedIn { get; set; }        
    //    public string TAU_AccountLockedOn { get; set; }        
    //    public bool TAU_IsMobileVerified { get; set; }        
    //    public bool TAU_IsEmailVerified { get; set; }        
    //    public string TAU_MobileVerifiedModifiedOn { get; set; }        
    //    public string TAU_EmailVerifiedModifiedOn { get; set; }
    //}

    //public class ApplicationUserProperty
    //{
    //    public int TAUP_Id { get; set; }        
    //    public int TAUP_TAU_Id { get; set; }        
    //    public string TAUP_Name { get; set; }        
    //    public string TAUP_Value { get; set; }        
    //    public bool TAUP_IsActive { get; set; }      
    //    public string TAUP_CratedByApp { get; set; }        
    //    public DateTime TAUP_CreatedOn { get; set; }        
    //    public int TAUP_CreatedBy { get; set; }        
    //    public DateTime? TAUP_ModifiedOn { get; set; }        
    //    public int TAUP_ModifitedBy { get; set; }        
    //    public string TAUP_GroupName { get; set; }
    //}
    public class UserAccess
    {
        public int AppId { get; set; }
        public string AppName { get; set; }
        public string AppGroup { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleGroup { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
    }
}
