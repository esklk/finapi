using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services.Implementation.Stateful
{
    public class AccountSelectedMessageProcessor : IStatefulMessageProcessor
    {
        public Task<MessageResponse> ProcessAsync(State state, string? text)
        {
            throw new NotImplementedException();
        }
    }
}
