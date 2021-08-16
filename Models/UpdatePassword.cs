using AuthUtility.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthUtility.Models {
    public class UpdatePassword : BaseResponse {
        [Required]
        [StringLength (30)]
        public string UserIdORUserName { get; set; }

        [Required (ErrorMessage = "Password is required")]
        [StringLength (30, MinimumLength = 6, ErrorMessage = "Must be at least 6 digits long.")]
        [Display (Name = "New Password")]
        public string Password { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }

        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdORUserName);
            response = response && !string.IsNullOrEmpty(Password);
            response = response && IsUserId.HasValue;
            if (IsUserId.Value == true)
            {
                response = response && Regex.IsMatch(UserIdORUserName, UtilityConstants.NumberRegex);
            }
            return response;
        }
    }
}