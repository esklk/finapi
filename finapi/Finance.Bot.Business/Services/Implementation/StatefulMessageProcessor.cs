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

        public async Task<MessageResponse> ProcessAsync(string? text)
        {
            State? state = await _stateService.GetStateAsync();
            if (state == null)
            {
                throw new InvalidOperationException("Cannot retrieve state to process message.");
            }

            MessageResponse response = await _statefulMessageProcessorFactory.Create(state).ProcessAsync(text);
            
            await _stateService.SetStateAsync(state);

            return response;
        }
    }
}
