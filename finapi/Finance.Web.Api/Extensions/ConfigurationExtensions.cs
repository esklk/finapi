using Finance.Web.Api.Configuration.Implementation;
using Microsoft.Extensions.Configuration;

namespace Finance.Web.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtOptions GetJwtOptions(this IConfiguration configuration) => configuration
            .GetSection(nameof(JwtOptions))
            .Get<JwtOptions>();
    }
}
