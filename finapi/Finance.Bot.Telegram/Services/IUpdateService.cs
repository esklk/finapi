using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services
{
    public interface IUpdateService
    {
        Task HandleAsync(Update update);
    }
}
