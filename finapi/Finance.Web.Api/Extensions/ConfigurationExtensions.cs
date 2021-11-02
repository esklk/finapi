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

        public static DatabaseConfiguration GetDatabaseConfiguration(this IConfiguration configuration, string name) => configuration
            .GetConfigurationDictionary<DatabaseConfiguration>(ConfigurationConstants.DatabaseConfiguration)
            .TryGetValue(name, out var dbConfig) 
            ? dbConfig 
            : throw new KeyNotFoundException($"Can not find \"{name}\" database configuration.");

        public static Dictionary<string, T> GetConfigurationDictionary<T>(this IConfiguration configuration, string name) => configuration
            .GetSection(name)
            .GetChildren()
            .ToDictionary(k => k.Key, e => e.Get<T>());
    }
}
