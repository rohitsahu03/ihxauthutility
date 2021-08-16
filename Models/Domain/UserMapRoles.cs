using System;
using LinqToDB.Mapping;

namespace AuthUtility.DomainObject.MediBuddy
{
  [Table(Name = "TblUserMap_Role")]
  public class UserMapRoles
  {
    [Column, PrimaryKey, Identity]
    public int TUMR_Id { get; set; }
    [Column]
    public int TUMR_TAU_Id { get; set; }
    [Column]
    public bool TUMR_IsActive { get; set; }
    [Column]
    public int TUMR_Role { get; set; }
    [Column]
    public int TUMR_CreatedBy { get; set; }
    [Column]
    public DateTime TUMR_CreatedOn { get; set; }
    [Column]
    public int TUMR_ModifiedBy { get; set; }
    [Column]
    public DateTime TUMR_ModifiedOn { get; set; }
  }
}