using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services
{
    public interface IBotMessageSender
    {
        Task SendAsync(BotMessage message);
    }
}
