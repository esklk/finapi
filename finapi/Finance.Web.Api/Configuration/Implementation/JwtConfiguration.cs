using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Finance.Web.Api.Configuration.Implementation
{
    public class JwtConfiguration : IJwtConfiguration
    {
        public string Issuer { get; set; }

        public int LifetimeMinutes { get; set; }

        public string SecurityKeyWord { get; set; }

        public string SecurityAlgorithm { get; set; }

        private SecurityKey _securityKey;
        public SecurityKey SecurityKey
        {
            get
            {
                if (_securityKey == null)
                {
                    _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKeyWord));
                }

                return _securityKey;
            }
        }
    }
}
