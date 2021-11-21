using Finance.Web.Api.Configuration.Implementation;
using System;
using System.Collections.Generic;

namespace Finance.Web.Api.Services.Implementation
{
    public class OauthConfigurationProvider : IOauthConfigurationProvider
    {
        private readonly IDictionary<string, OAuthConfiguration> _configuration;

        public OauthConfigurationProvider(IDictionary<string, OAuthConfiguration> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public OAuthConfiguration GetRelated(Type relatedType)
        {
            if(relatedType.FullName == typeof(GoogleAccessTokenValidator).FullName)
            {
                return _configuration["Google"];
            }

            throw new NotSupportedException($"Related type \"{relatedType.FullName}\" is not supported.");
        }
    }
}
