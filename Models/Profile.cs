namespace AuthUtility.Models
{
    public class UserBaseProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int EntityId { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public bool Active { get; set; }
        public string EmployeeId { get; set; }
        public string Gender { get; set; }
    }
    public class UpdateAppBasedProfileRequest
    {
        public string ApplicationId { get; set; }
        public string AuthToken { get; set; }
        public string ProfileDataJson { get; set; }
    }
    public class UpdateBaseUserProfileRequest
    {
        public string ApplicationId { get; set; }
        public UserBaseProfile BaseProfile { get; set; }
        public string AuthToken { get; set; }
    }
    public class ProfileResponse : BaseResponse
    {
        public int UserId { get; set; }
        public string ApplicationId { get; set; }
        public string AppBasedProfileData { get; set; }
    }
}
