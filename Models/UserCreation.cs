using AuthUtility.Constants;
using System;
using System.Collections.Generic;

namespace AuthUtility.Models
{
    public class UserCreation
    {
        public string ApplicationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ProviderMasterEntityId { get; set; }
        public string Gender { get; set; }
        public string EmployeeId { get; set; }
        public string Password { get; set; }
        public List<int> UserRoles { get; set; }
        public DateTime DOB { get; set; }
        public long MAID { get; set; }
        public bool IgnoreAliasConfig { get; set; }
        public bool IsValid()
        {
            bool response = true;
            response = response && !(string.IsNullOrEmpty(this.FirstName));
            response = response && !(string.IsNullOrEmpty(this.UserName));
            response = response && !(string.IsNullOrEmpty(this.Password));
            response = response && !(string.IsNullOrEmpty(this.Email) && string.IsNullOrEmpty(this.PhoneNumber));
            response = response && this.ProviderMasterEntityId > 0;
            if (!string.IsNullOrEmpty(this.Email))
                response = response && !this.Email.Contains(',');
            return response;
        }
    }

    public class UserCreationResponse : BaseResponseAuth
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
