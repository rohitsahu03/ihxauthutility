using AuthUtility.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthUtility.Models {
    public class UpdateLoginname : BaseResponse {
        [Required]
        [StringLength (70)]
        public string UserIdORUserName { get; set; }

        [Required (ErrorMessage = "Username is required")]
        [Display (Name = "New Username")]
        public string LoginName { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }

        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdORUserName);
            response = response && !string.IsNullOrEmpty(LoginName);
            response = response && IsUserId.HasValue;
            if (IsUserId.Value == true)
            {
                response = response && Regex.IsMatch(UserIdORUserName, UtilityConstants.NumberRegex);
            }
            return response;
        }
    }
}