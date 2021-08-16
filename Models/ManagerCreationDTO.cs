using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthUtility.Models
{
    public class ManagerCreationDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public int EntityId { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Numeric Only(without country code)")]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNo { get; set; }        
        public List<int> UserRoles { get; set; }
        public string LoginType { get; set; }
        public Dictionary<string, string> UserProperties { get; set; }
        public string PropertiesString { get; set; }
        [Required]
        public string Gender { get; set; }
        public int ActionPerformedBy { get; set; }
        public bool IsPropertyLoaded { get; set; }
        public string ErrorMsg { get; set; }
    }
}
