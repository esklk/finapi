using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services
{
    public interface IStatefulMessageProcessor
    {
        Task<MessageResponse> ProcessAsync(State state, string? text);
    }
}
