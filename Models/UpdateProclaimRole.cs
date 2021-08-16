using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AuthUtility.Models
{
  public class UpdateProclaimRole : BaseResponse
  {
    [Required]
    [DataType(DataType.MultilineText)]
    [Display(Name = "List of UserIds (comma separated) Max:500")]
    public string UserIds { get; set; }
    [Required]
    [Display(Name = "Deactivate Previous Role(s)")]
    public bool DisablePreviousRoles { get; set; }

    [Required]
    [Display(Name = "Select Role(s)")]
    public List<string> SelectedRolesIds { get; set; }
    public Dictionary<string, string> AllRoles { get; set; }
    public int ActionPerformedBy { get; set; }
  }
}