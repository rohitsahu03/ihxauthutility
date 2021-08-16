using System;

using LinqToDB.Mapping;

namespace AuthUtility.DomainObject.MediBuddy
{
  [Table(Name = "Role")]
  public class Role
  {
    [Column, PrimaryKey, Identity]
    public int Id { get; set; }
    [Column]
    public string Name { get; set; }
    [Column]
    public string GroupKey { get; set; }
    [Column]
    public bool IsActive { get; set; }
    [Column]
    public string CreatedBy { get; set; }
    [Column]
    public DateTime CreatedOn { get; set; }
  }
}