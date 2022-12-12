using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StartedStateMessageProcessor : IMessageProcessor
    {
        private const string WelcomeMessage =
            $"Hello, I'm Finn. I will help you manage your finances. To start you must select an account. Don't have one yet? Create it with \"{Commands.CreateAccount}\"! You can also use \"{Commands.Help}\" to learn how to get the most using all existing features. Have fun!";

        private readonly State _state;

        public StartedStateMessageProcessor(State state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public Task<MessageResponse> ProcessAsync(string? text)
        {
            var response = new MessageResponse(WelcomeMessage, Commands.CreateAccount, Commands.Help);

            return new ValueTask<MessageResponse>(response).AsTask();
        }
    }
}
