using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services
{
    public interface IUpdateProvider
    {
        Update Update { get; }
    }
}