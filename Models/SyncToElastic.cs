using System.ComponentModel.DataAnnotations;

namespace AuthUtility.Models
{
    public class SyncToElastic : BaseResponse
    {
        [DataType(DataType.MultilineText)]
        [Display(Name = "List of TAU_ID (comma separated) Max:500")]
        public string TauIds { get; set; }
    }
}
