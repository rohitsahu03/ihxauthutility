using AuthUtility.Common;
using AuthUtility.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuthUtility.Models
{
    public class UpdateRole : BaseResponse
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "List of values (comma separated) Max:500")]
        public string UserIdsORUserNames { get; set; }
        [Required]
        [Display(Name = "Deactivate Previous Role(s)")]
        public bool DisablePreviousRoles { get; set; }
        [Required]
        [Display(Name = "Select Role(s)")]
        public List<string> SelectedRolesIds { get; set; }
        public Dictionary<string, string> AllRoles { get; set; }
        public int ActionPerformedBy { get; set; }
        [Required]
        public bool? IsUserId { get; set; }
        public bool Validate()
        {
            bool response = true;
            response = response && !string.IsNullOrEmpty(UserIdsORUserNames);
            response = response && SelectedRolesIds.HasRecords();
            response = response && AllRoles.HasRecords();
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