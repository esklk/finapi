using System;

namespace Finance.Web.Api.Exceptions
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(Exception exception) : base("Authentication failed.", exception)
        {

        }
    }
}
