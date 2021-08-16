using AuthUtility.Constants;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuthUtility.Models
{
    public class UpdatePasswordBulk : BaseResponse
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "List of values (comma separated) Max:500")]
        public string UserIdsORUserNames { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }
        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdsORUserNames);
            response = response && !string.IsNullOrEmpty(Password);
            response = response && IsUserId.HasValue;
            var values = UserIdsORUserNames.Split(",");
            response = response && values.Count() <= UtilityConstants.MaxUserIdCount;
            if (response && IsUserId.Value == true)
                response = values.All(x => Regex.IsMatch(x, UtilityConstants.NumberRegex));
            return response;
        }
    }
}
