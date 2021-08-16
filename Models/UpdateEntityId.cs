using AuthUtility.Constants;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuthUtility.Models
{
    public class UpdateEntityId : BaseResponse
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "List of values (comma separated) Max:500")]
        public string UserIdsORUserNames { get; set; }
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numeric Only")]
        [Display(Name = "Old Entity Id")]
        public string OldProviderMasterEntityId { get; set; }
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numeric Only")]
        [Display(Name = "New Entity Id")]
        public string NewProviderMasterEntityId { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }

        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdsORUserNames);
            response = response && !string.IsNullOrEmpty(OldProviderMasterEntityId);
            response = response && !string.IsNullOrEmpty(NewProviderMasterEntityId);
            response = response && IsUserId.HasValue;
            var values = UserIdsORUserNames.Split(",");
            response = response && values.Count() <= UtilityConstants.MaxUserIdCount;
            if (IsUserId.Value == true)
            {
                response = response && values.All(x => Regex.IsMatch(x, UtilityConstants.NumberRegex));
            }
            return response;
        }
    }
}