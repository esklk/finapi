using Finance.Web.Api.Configuration;
using Finance.Web.Api.Configuration.Implementation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Web.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtConfiguration GetJwtConfiguration(this IConfiguration configuration) => configuration
            .GetSection(ConfigurationConstants.JwtConfiguration)
            .Get<JwtConfiguration>();
    }
}
