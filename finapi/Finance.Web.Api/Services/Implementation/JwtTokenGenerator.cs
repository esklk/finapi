using Finance.Web.Api.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Finance.Web.Api.Services.Implementation
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtTokenGenerator(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration ?? throw new ArgumentNullException(nameof(jwtConfiguration));
        }

        public string Generate(IEnumerable<Claim> claims)
        {
            DateTime now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _jwtConfiguration.Issuer,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(_jwtConfiguration.LifetimeMinutes)),
                    signingCredentials: new SigningCredentials(_jwtConfiguration.SecurityKey, _jwtConfiguration.SecurityAlgorithm));

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
