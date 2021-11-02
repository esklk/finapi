using AutoMapper;
using System.Collections.Generic;

namespace Finance.Web.Api.Extensions
{
    public static class AutomapperExtensions
    {
        public static void MapFromDictionary<TDestination, TMember>(this IMemberConfigurationExpression<Dictionary<string, TMember>, TDestination, TMember> config, string key, bool throwIfMissing = false)
        {
            config.MapFrom(source => throwIfMissing || source.ContainsKey(key) ? source[key] : default);
        }
    }
}
