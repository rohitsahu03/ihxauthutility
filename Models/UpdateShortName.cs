using System.ComponentModel.DataAnnotations;
namespace AuthUtility.Models {
    public class UpdateShortName : BaseResponse {
        [Required]
        [StringLength (20)]
        [RegularExpression (@"^[0-9]+$", ErrorMessage = "Numeric Only")]
        public string ProviderMasterEntityId { get; set; }

        [Required (ErrorMessage = "Old Alias is required")]
        [Display (Name = "Old Alias")]
        public string OldShortName { get; set; }

        [Required (ErrorMessage = "New Alias is required")]
        [Display (Name = "New Alias")]
        public string NewShortName { get; set; }
        public int ActionPerformedBy { get; set; }
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Must be at least 6 digits long.")]
        [Display(Name = "Policy ID")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numeric Only")]
        public string PolId { get; set; }
    }
}