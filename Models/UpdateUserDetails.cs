using System.Collections.Generic;

namespace AuthUtility.Models
{
    public class UserDetailsBulkUpdate 
    {
        public List<UserDetails> UserDetails { get; set; }
        public int ActionPerformedBy { get; set; }
    }

    public class UserDetails
    {
        public int UserId { get; set; }
        public string OldUserName { get; set; }
        public string NewUserName { get; set; }
        public string EmployeeId { get; set; }
        public int EntityId { get; set; }
    }
}
