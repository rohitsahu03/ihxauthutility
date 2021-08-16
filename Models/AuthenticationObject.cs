using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AuthUtility.Models
{
    public class AuthGetTokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationId { get; set; }
        public string UserAgent { get; set; }
        public string AppKey { get; set; }
        public int AuthUserId { get; set; }

        public AuthGetTokenRequest(string userName, string password, IHttpContextAccessor httpContextAccessor,string appKey,string appId,int userId)
        {
            this.Username = userName;
            this.Password = password;
            this.UserAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            this.ApplicationId = appId;
            this.AppKey = appKey;
            this.AuthUserId = userId;
        }
        public AuthGetTokenRequest()
        {
        }

    }
    public class AuthGetTokenResponse : BaseResponse
    {
        public int UserId { get; set; }
        public bool Authenticated { get; set; }
        public AuthAccessToken AccessToken { get; set; }
        public List<RoleForBearerDTO> Roles { get; set; }
        public List<FeatureDTO> Features { get; set; }
    }

    public class AuthAccessToken
    {
        public string Access_Token { get; set; }
    }
    public class RoleForBearerDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class FeatureDTO
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string GroupKey { get; set; }
    }
}