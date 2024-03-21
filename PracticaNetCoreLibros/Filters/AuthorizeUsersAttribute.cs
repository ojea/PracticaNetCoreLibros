﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PracticaNetCoreLibros.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute
            , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();
            ITempDataProvider provider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();
            var tempData = provider.LoadTempData(context.HttpContext);
            tempData["controller"] = controller;
            tempData["action"] = action;
            provider.SaveTempData(context.HttpContext, tempData);
            if(!user.Identity.IsAuthenticated)
            {
                context.Result = this.GetRoute("Managed", "Login");
            }
            
        }

        private RedirectToRouteResult GetRoute
           (string controller, string action)

        {
            RouteValueDictionary ruta = 
                new RouteValueDictionary(
                    new {controller=controller, action=action }
                    );
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;

        }
    }
}
