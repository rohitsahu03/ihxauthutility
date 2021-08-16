using AuthUtility.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthUtility.Models {
    public class UpdateMob : BaseResponse {
        [Required]
        [StringLength (30)]
        public string UserIdORUserName { get; set; }

        [Required (ErrorMessage = "The mobile number is required")]
        [StringLength (10, MinimumLength = 10, ErrorMessage = "Must be at least 10 digits long.")]
        [Display (Name = "Mobile Number")]
        [RegularExpression (@"^[0-9]+$", ErrorMessage = "Numeric Only(without country code)")]
        public string MobNo { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }

        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdORUserName);
            response = response && !string.IsNullOrEmpty(MobNo) && Regex.IsMatch(MobNo, UtilityConstants.NumberRegex);
            response = response && IsUserId.HasValue;
            if (IsUserId.Value == true)
            {
                response = response && Regex.IsMatch(UserIdORUserName, UtilityConstants.NumberRegex);
            }
            return response;
        }
    }
}