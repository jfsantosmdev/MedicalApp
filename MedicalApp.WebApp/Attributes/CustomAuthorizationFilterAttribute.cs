using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace MedicalApp.WebApp.Attributes
{
    public class CustomAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var serialisedLogin = context.HttpContext.Session.GetString(SessionKeys.Login);
            if (string.IsNullOrEmpty(serialisedLogin))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Auth" }));
            }
        }
    }
}
