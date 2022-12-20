using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Commands
{
    public interface IBotCommand
    {
        Task ExecuteAsync(State state, string[] arguments);
    }
}
