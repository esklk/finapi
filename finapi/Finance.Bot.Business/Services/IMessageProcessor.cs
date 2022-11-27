using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services
{
    public interface IMessageProcessor
    {
        Task<MessageResponse> ProcessAsync(string text);
    }
}
