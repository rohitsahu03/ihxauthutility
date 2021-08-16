using AuthUtility.Common;
using AuthUtility.Constants;
using AuthUtility.Interfaces;
using AuthUtility.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AuthUtility.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IDBProvider _dbProvider;
        public LoginController(IHttpContextAccessor httpContextAccessor, ILogger<LoginController> logger, IDBProvider dbProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dbProvider = dbProvider;
        }

        public IActionResult Authorize(string Message)
        {

            UserContext presentSession = HttpContext.Session.Get<UserContext>();
            if (presentSession != null)
                return presentSession.Roles.Any(x => x.RoleId == 9227) ? RedirectToAction("Index", "Home") : RedirectToAction("ManagerCreation", "User");

            if (string.IsNullOrEmpty(Message))
                return View("Login");

            return View("Login", new AuthenticationRequest() { Message = Message });
        }

        [HttpPost]
        public IActionResult Authorize(AuthenticationRequest Model)
        {
            AuthGetTokenResponse responseObj = null;
            try
            {
                if (ModelState.IsValid)
                {
                    AuthGetTokenRequest requestObj = new AuthGetTokenRequest(Model.UserName, Model.Password, _httpContextAccessor, null, UtilityConstants.AuthUtilityAppId, default(int));
                    responseObj = _dbProvider.GetBearer(requestObj, APIConstants.SignInWithPassword);
                    if (responseObj != null && responseObj.Authenticated && responseObj.AccessToken != null && !string.IsNullOrEmpty(responseObj.AccessToken.Access_Token) && responseObj.UserId > 0)
                    {
                        HttpContext.Session.Set(new UserContext()
                        {
                            AccessToken = responseObj.AccessToken.Access_Token,
                            UserId = responseObj.UserId,
                            Roles = responseObj.Roles,
                            Features = responseObj.Features
                        });
                        return responseObj.Roles.Any(x => x.RoleId == 9227) ? RedirectToAction("Index", "Home") : RedirectToAction("ManagerCreation", "User");
                    }
                    else
                        return RedirectToAction("Authorize", "Login", new { @message = MsgConstants.LoginFailedMsg });
                }
                else
                    return View("Login");
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "Error in Authorize :" + ex.Message);
            }
            return View("Login");
        }
        public IActionResult SignOut()
        {
            HttpContext.Session.Clean();
            return RedirectToAction("Authorize", "Login");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}