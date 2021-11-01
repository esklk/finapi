using Finance.Web.Api.Configuration.Implementation;
using Finance.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Finance.Web.Api.Services.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IEnumerable<KeyValuePair<string, OAuthConfiguration>> _configurations;

        public LoginService(IEnumerable<KeyValuePair<string, OAuthConfiguration>> configurations)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
        }

        public static string BuildUrl(OAuthConfiguration configuration)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var uriBuilder = new UriBuilder(configuration.Endpoint);
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var parameter in configuration.Parameters)
            {
                query[parameter.Key] = parameter.Value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        public LoginOptionModel[] GetLoginOptionModels()
        {
            return _configurations.Select(x => new LoginOptionModel
            {
                Provider = x.Key,
                State = x.Value.Parameters.GetValueOrDefault("state")
                    ?? throw new InvalidOperationException($"Missing required parameter \"state\" for \"{x.Key}\" OAuth configuration"),
                Url = BuildUrl(x.Value)
            }).ToArray();
        }
    }
}
