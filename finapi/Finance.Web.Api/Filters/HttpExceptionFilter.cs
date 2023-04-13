using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using Finance.Web.Api.Exceptions;

namespace Finance.Web.Api.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode code;
            string message;
            switch (context.Exception)
            {
                case AuthenticationFailedException:
                    code = HttpStatusCode.Unauthorized;
                    message = context.Exception.Message;
                    break;
                case ArgumentException:
                    code = HttpStatusCode.BadRequest;
                    message = context.Exception.Message;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    message = "Unexpected server error. Please try again later.";
                    break;
            }

            context.Result = new ObjectResult(new { message }) { StatusCode = (int)code };
            context.ExceptionHandled = true;
        }
    }
}
