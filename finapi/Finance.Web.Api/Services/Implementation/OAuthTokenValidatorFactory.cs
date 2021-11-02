using Finance.Business.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Finance.Web.Api.Services.Implementation
{
    public class OAuthTokenValidatorFactory : ITokenValidatorFactory
    {
        private static readonly Dictionary<string, Type> ProviderValidatorTypeMapping = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Google", typeof(GoogleAccessTokenValidator) }
        };

        private readonly IServiceProvider _serviceProvider;

        public OAuthTokenValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ITokenValidator Create(string tokenProvider)
        {
            if (string.IsNullOrWhiteSpace(tokenProvider))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(tokenProvider));
            }
            if (!ProviderValidatorTypeMapping.ContainsKey(tokenProvider)) 
            {
                throw new ArgumentException($"The {tokenProvider} token provider is not supported.", nameof(tokenProvider));
            }

            return (ITokenValidator)_serviceProvider.GetRequiredService(ProviderValidatorTypeMapping[tokenProvider]);
        }
    }
}
