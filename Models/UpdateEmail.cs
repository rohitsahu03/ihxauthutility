using AuthUtility.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthUtility.Models
{
    public class UpdateEmail : BaseResponse {

        [Required]
        [StringLength (50)]
        public string UserIdORUserName { get; set; }

        [Required (ErrorMessage = "The email address is required")]
        [EmailAddress (ErrorMessage = "Invalid Email Address")]
        [Display (Name = "Email Addesss")]
        public string EmailId { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }

        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdORUserName);
            response = response && !string.IsNullOrEmpty(EmailId);
            response = response && IsUserId.HasValue;
            if (IsUserId.Value == true)
            {
                response = response && Regex.IsMatch(UserIdORUserName, UtilityConstants.NumberRegex);
            }
            return response;
        }
    }
}