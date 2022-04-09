using Finance.Core.Configuration.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Core.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string BuildConnectionString(this DatabaseConfiguration configuration) =>
            $"server={configuration.Server};port={configuration.Port};database={configuration.Database};UserId={configuration.UserId};Password={configuration.Password};";

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
