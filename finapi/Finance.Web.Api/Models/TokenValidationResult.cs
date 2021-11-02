using System;
using System.Collections.Generic;

namespace Finance.Web.Api.Models
{
    public class TokenValidationResult
    {
        public TokenValidationResult(Dictionary<string, string> payload)
        {
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            IsValid = true;
        }

        public TokenValidationResult(string error)
        {
            Error = error;
            IsValid = false;
        }

        public bool IsValid { get; }

        public string Error { get; }

        public Dictionary<string, string> Payload { get; }
    }
}
