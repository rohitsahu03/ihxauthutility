using AuthUtility.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AuthUtility.Models
{
    public class UpdateFirstLastName : BaseResponse
    {
        [Required]
        [StringLength(50)]
        public string UserIdORUserName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }

        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdORUserName);
            response = response && !string.IsNullOrEmpty(FirstName);
            response = response && IsUserId.HasValue;
            if (IsUserId.Value == true)
            {
                response = response && Regex.IsMatch(UserIdORUserName, UtilityConstants.NumberRegex);
            }
            return response;
        }
    }
}
