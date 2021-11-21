using Finance.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Finance.Web.Api.Extensions
{
    public static class StringExtensions
    {
        public static string ReplacePlaceholders(this string source, params (string placeholder, string value)[] substitutions)
        {
            var substitutionsDictionary = substitutions.ToDictionary(x => x.placeholder, x => x.value, StringComparer.OrdinalIgnoreCase);

            foreach(Match match in Regex.Matches(source, @"\{([^}]*)\}"))
            {
                if(!substitutionsDictionary.TryGetValue(match.Value.TrimStart('{').TrimEnd('}'), out var substitution))
                {
                    throw new KeyNotFoundException($"Can not find substitution for \"{match.Value}\" key. Source: \"{source}\".");
                }

                source = Regex.Replace(source, match.Value, substitution);
            }

            return source;
        }

        public static Dictionary<string, string> ToDictionary(this string source, string pairSeparator, string keyValueSeparator, IEqualityComparer<string> comparer = null)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(source));
            }
            if (string.IsNullOrWhiteSpace(pairSeparator))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(pairSeparator));
            }
            if (string.IsNullOrWhiteSpace(keyValueSeparator))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(keyValueSeparator));
            }

            return source?
                .Split(pairSeparator)
                .Select(x => x.Split(keyValueSeparator))
                .Where(x => x.Length > 1)
                .ToDictionary(x => x[0], x => x[1], comparer);
        }
    }
}
