using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Finance.Business.Exceptions;

namespace Finance.Web.Api.Services.Implementation
{
    public class JwtTokenValidator : ITokenValidator
    {
        private readonly TokenValidationParameters _validationParameters;

        public JwtTokenValidator(TokenValidationParameters validationParameters)
        {
            _validationParameters = validationParameters ?? throw new ArgumentNullException(nameof(validationParameters));
        }

        public IEnumerable<Claim> Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(token));
            }

            IEnumerable<Claim> claims = new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out _).Claims;

            return claims;
        }
    }
}
