using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class Help : IBotCommand
    {
        private readonly IBotMessageSender _messageSender;

        public Help(IBotMessageSender messageSender)
        {
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        public async Task ExecuteAsync(State state, string[] arguments)
        {
            await _messageSender.SendAsync(new BotMessage("No content here so far :)"));
        }
    }
}
