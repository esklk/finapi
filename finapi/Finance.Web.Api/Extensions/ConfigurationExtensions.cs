using Finance.Web.Api.Configuration.Implementation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Web.Api.Extensions
{
    public static class ConfigurationExtensions
    {
        public static JwtOptions GetJwtOptions(this IConfiguration configuration) => configuration
            .GetSection(nameof(JwtOptions))
            .Get<JwtOptions>();

        public static DatabaseConfiguration GetDatabaseConfiguration(this IConfiguration configuration, string name) => configuration
            .GetSection("DatabaseConfiguration")
            .GetChildren()
            .ToDictionary(k => k.Key, e => e.Get<DatabaseConfiguration>())
            .GetValueOrDefault(name) ?? throw new KeyNotFoundException($"Can not find \"{name}\" database configuration.");
    }
}
