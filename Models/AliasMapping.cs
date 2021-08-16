using AuthUtility.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthUtility.Models
{
    public class AliasMapping : BaseResponse
    {
        [Required]
        public long? PolCorporateId { get; set; }
        public long? PolGroupCorporateId { get; set; }
        public DBType? DBType { get; set; }
        [Required]
        public LoginType? LoginType { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Password Format")]
        public string PasswordType { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        [Display(Name = "Possible Aliases")]
        public string PossibleAliases { get; set; }
        public int ActionPerformedBy { get; set; }

        public bool IsValid()
        {
            bool response = true;
            response = response && this.PolCorporateId > 0;

            if (this.LoginType == Enums.LoginType.EMAIL)
                response = response && !string.IsNullOrEmpty(PossibleAliases);
            else
                response = response && !(string.IsNullOrEmpty(this.Suffix) && string.IsNullOrEmpty(this.Prefix));

            return response;
        }
    }
}
