using AuthUtility.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace AuthUtility.Common
{

    public class AuthorizationFilter : ActionFilterAttribute
    {
        private readonly List<int> _roles = null;
        public AuthorizationFilter(string RoleIds)
        {
            _roles = string.IsNullOrEmpty(RoleIds) ? new List<int>() : RoleIds.Split(",").Select(x => int.Parse(x)).ToList();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {

                UserContext user = context.HttpContext.Session.Get<UserContext>();
                if (user == null || string.IsNullOrEmpty(user.AccessToken))
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Login" }, { "Action", "Authorize" } });

                if (user.Roles.Select(x => x.RoleId).Intersect(_roles).Count() > 0)
                    base.OnActionExecuting(context);
                else
                    //context.Result = new ContentResult() { Content = "Unauthorised" };
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Login" }, { "Action", "Error" } });
            }
            catch { }
        }
    }
}