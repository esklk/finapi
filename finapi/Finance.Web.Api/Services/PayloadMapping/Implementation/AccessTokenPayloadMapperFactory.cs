using AutoMapper;
using Finance.Business.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services.Tokens.PayloadMapping.Implementation
{
    public class AccessTokenPayloadMapperFactory : IPayloadMapperFactory
    {
        private static readonly Dictionary<string, Type> ProviderValidatorTypeMapping = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Google", typeof(GoogleAccessTokenConfiguration) }
        };

        private readonly IServiceProvider _serviceProvider;

        public AccessTokenPayloadMapperFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IMapper Create(string tokenProvider)
        {
            if (string.IsNullOrWhiteSpace(tokenProvider))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(tokenProvider));
            }
            if (!ProviderValidatorTypeMapping.ContainsKey(tokenProvider))
            {
                throw new ArgumentException($"The {tokenProvider} token provider is not supported.", nameof(tokenProvider));
            }

            MapperConfiguration configuration = (MapperConfiguration)_serviceProvider.GetRequiredService(ProviderValidatorTypeMapping[tokenProvider]);

            return configuration.CreateMapper();
        }
    }
}
