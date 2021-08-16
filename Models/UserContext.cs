using System.Collections.Generic;

namespace AuthUtility.Models {
    public class UserContext {
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public List<RoleForBearerDTO> Roles { get; set; }
        public List<FeatureDTO> Features { get; set; }
    }
}