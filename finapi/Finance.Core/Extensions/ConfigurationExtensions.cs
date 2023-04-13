using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Finance.Core.Exceptions;

namespace Finance.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
        {
            T? defaultValue = default;
            T? value = configuration.GetValue(key, defaultValue!);
            if (Comparer<T>.Default.Compare(value, defaultValue) == 0)
            {
                throw new MissingConfigurationException(key);
            }

            return value!;
        }
        public static T GetRequired<T>(this IConfiguration configuration, string key)
        {
            T? defaultValue = default;
            T? value = configuration.GetSection(key).Get<T>();
            if (Comparer<T>.Default.Compare(value, defaultValue) == 0)
            {
                throw new MissingConfigurationException(key);
            }

            return value!;
        }
    }
}
