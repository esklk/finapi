using AutoMapper;
using Finance.Bot.Data.Models;
using Finance.Core.Practices;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StateServiceFactory: IFactory<IStateService, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Type _initialProcessorType;

        public StateServiceFactory(IServiceProvider serviceProvider, Type initialProcessorType)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _initialProcessorType = initialProcessorType ?? throw new ArgumentNullException(nameof(initialProcessorType));
        }

        public IStateService Create(string stateId)
        {
            return new StateService(
                _serviceProvider.GetRequiredService<IRepository<StateEntity, string>>(),
                _serviceProvider.GetRequiredService<IMapper>(),
                stateId,
                _initialProcessorType);
        }
    }
}
