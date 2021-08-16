using System.ComponentModel.DataAnnotations;
namespace AuthUtility.Models
{
    public class AuthenticationRequest : BaseResponse
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}