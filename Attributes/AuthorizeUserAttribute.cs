using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace netflix_clone.Attributes
{
    public class AuthorizeUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var email = context.HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                // If the session does not contain an email, redirect to login
                context.Result = new RedirectToActionResult("Login", "User", null);
            }

            base.OnActionExecuting(context);
        }
    }
}