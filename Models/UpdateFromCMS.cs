using System.ComponentModel.DataAnnotations;
namespace AuthUtility.Models
{
    public class UpdateFromCMS : BaseResponse
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "List of TAU_ID (comma separated) Max:500")]
        public string TauIds { get; set; }

        [Required(ErrorMessage = "PolId is required")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Must be at least 6 digits long.")]
        [Display(Name = "Policy ID")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numeric Only")]
        public string PolId { get; set; }

        [Required]
        public string UsernameFormat { get; set; }
        public int ActionPerformedBy { get; set; }

    }
}