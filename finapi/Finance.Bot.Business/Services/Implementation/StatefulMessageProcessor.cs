using Finance.Bot.Business.Models;
using Finance.Core.Practices;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StatefulMessageProcessor : IMessageProcessor
    {
        private readonly IStateService _stateService;
        private readonly IFactory<IMessageProcessor, State> _statefulMessageProcessorFactory;

        public StatefulMessageProcessor(IStateService stateService, IFactory<IMessageProcessor, State> statefulMessageProcessorFactory)
        {
            _stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
            _statefulMessageProcessorFactory = statefulMessageProcessorFactory ?? throw new ArgumentNullException(nameof(statefulMessageProcessorFactory));
        }

        public async Task<MessageResponse> ProcessAsync(string text)
        {
            var state = await _stateService.GetStateAsync();
            var response = await _statefulMessageProcessorFactory.Create(state).ProcessAsync(text);
            await _stateService.UpdateStateAsync(state);

            return response;
        }
    }
}
