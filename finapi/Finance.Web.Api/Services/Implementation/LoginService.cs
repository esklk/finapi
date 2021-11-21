using Finance.Web.Api.Configuration.Implementation;
using Finance.Web.Api.Extensions;
using Finance.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Web.Api.Services.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IEnumerable<KeyValuePair<string, OAuthConfiguration>> _configurations;
        private readonly IUriManager _uriManager;

        public LoginService(IDictionary<string, OAuthConfiguration> configurations, IUriManager uriManager)
        {
            _configurations = configurations ?? throw new ArgumentNullException(nameof(configurations));
            _uriManager = uriManager ?? throw new ArgumentNullException(nameof(uriManager));
        }

        public string BuildUrl(OAuthConfiguration configuration, string redirectUrl)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var actualParameters = configuration.Parameters
                .Select(x => (x.Key, x.Value.ReplacePlaceholders(("redirectUrl", redirectUrl ?? string.Empty))));

            return _uriManager.SetQueryParamaters(configuration.Endpoint, actualParameters.ToArray());
        }

        public LoginOptionModel[] GetLoginOptionModels(string redirectUrl)
        {
            return _configurations.Select(x => new LoginOptionModel
            {
                Provider = x.Key,
                Url = BuildUrl(x.Value, redirectUrl)
            }).ToArray();
        }
    }
}
