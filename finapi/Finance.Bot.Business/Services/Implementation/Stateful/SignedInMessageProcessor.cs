using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services.Implementation.Stateful
{
    public class SignedInMessageProcessor : IStatefulMessageProcessor
    {
        public async Task<MessageResponse> ProcessAsync(State state, string? text)
        {
            switch (text)
            {
                case Commands.CreateAccount:
                    return new MessageResponse("Create account command received.");
                case Commands.Help:
                    return new MessageResponse("Help command received.");
                default:
                    throw new InvalidCommandException(text);
            }

        }
    }
}
