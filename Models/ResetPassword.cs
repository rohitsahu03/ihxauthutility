using System.ComponentModel.DataAnnotations;

namespace AuthUtility.Models
{
    public class ResetPassword : BaseResponse
    {        
        [DataType(DataType.MultilineText)]
        [Display(Name = "List of TAU_ID (comma separated)")]
        public string UserIds { get; set; }
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numeric Only")]
        [Display(Name = "Entity Id")]
        public string ProviderMasterEntityId { get; set; }
        public int ActionPerformedBy { get; set; }
    }
}
