using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace BookCrossingBackEnd.Filters
{
    public class LoginFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var message = "Server error occured";
            var exceptionType = context.Exception;
            if(exceptionType is SecurityTokenException)
            {
                message = exceptionType.Message;
                context.HttpContext.Response.StatusCode = 401;
            }
            if(exceptionType is InvalidCredentialException)
            {
                message = "Invalid login or password";
                context.HttpContext.Response.StatusCode = 401;
            }
            context.Result = new ObjectResult(new { message=message});
        }
    }
}
