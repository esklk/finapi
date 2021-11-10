using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Finance.Web.Api.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode code;
            string message;
            if (context.Exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
                message = context.Exception.Message;
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
                message = "Unexpected server error. Please try again later.";
            }

            context.Result = new ObjectResult(new { message }) { StatusCode = (int)code };
            context.ExceptionHandled = true;
        }
    }
}
