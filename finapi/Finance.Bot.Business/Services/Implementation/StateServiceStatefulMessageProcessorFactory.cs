using Finance.Bot.Business.Models;
using Finance.Core.Practices;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StateServiceStatefulMessageProcessorFactory : IFactory<IMessageProcessor, IStateService>
    {
        private readonly IServiceProvider _serviceProvider;

        public StateServiceStatefulMessageProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        public IMessageProcessor Create(IStateService stateService)
        {
            IFactory<IMessageProcessor, State> messageProcessorFactory = _serviceProvider.GetRequiredService<IFactory<IMessageProcessor, State>>();

            return new StatefulMessageProcessor(stateService, messageProcessorFactory);
        }
    }
}
