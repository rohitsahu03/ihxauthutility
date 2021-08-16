using System.ComponentModel.DataAnnotations;
namespace AuthUtility.Models {
    public class BrokerLogin : BaseResponse {

        [Required (ErrorMessage = "UserName is required")]
        [Display (Name = "UserName")]
        public string BrokerName { get; set; }

        [Required (ErrorMessage = "BrokerId is required")]
        [StringLength (20)]
        [RegularExpression (@"^[0-9]+$", ErrorMessage = "Numeric Only")]
        public string BrokerId { get; set; }
    }
}