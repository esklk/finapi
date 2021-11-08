using Microsoft.Extensions.DependencyInjection;
using System;

namespace Finance.Web.Api.Services.Implementation
{
    public class NestingCheckerFactory : INestingCheckerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NestingCheckerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public INestingChecker Create(string nestedResourceIdentifier)
        {
            if(nestedResourceIdentifier.StartsWith("operationcategory", StringComparison.OrdinalIgnoreCase))
            {
                return _serviceProvider.GetRequiredService<OperationCategoryNestingChecker>();
            }

            throw new InvalidOperationException($"Resource identifier \"{nestedResourceIdentifier}\" is not supported.");
        }
    }
}
