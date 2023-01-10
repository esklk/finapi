using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services.Implementation
{
    internal class CallbackQueryDeleteMessageSender : IBotMessageSender
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBotMessageSender _messageSender;
        private readonly Message? _callbackQueryMessage;

        public CallbackQueryDeleteMessageSender(ITelegramBotClient botClient, IUpdateProvider updateProvider, IBotMessageSender messageSender)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _callbackQueryMessage = updateProvider.Update.CallbackQuery?.Message;
        }

        public async Task SendAsync(BotMessage message)
        {
            if (_callbackQueryMessage != null)
            {
                await _botClient.DeleteMessageAsync(_callbackQueryMessage.Chat.Id, _callbackQueryMessage.MessageId);
            }

            await _messageSender.SendAsync(message);
        }
    }
}
