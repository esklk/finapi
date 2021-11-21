using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Finance.Web.Api.Services.Implementation
{
    public class JwtTokenManager : IJwtTokenManager
    {
        public Dictionary<string, string> GetPayload(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jwtToken.Claims.ToDictionary(x => x.Type, x => x.Value, StringComparer.OrdinalIgnoreCase);
        }
    }
}
