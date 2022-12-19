using System;
using System.Threading.Tasks;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Finance.Bot.Telegram.Services.Implementation
{
    internal class CallbackQueryUpdateService : IUpdateService
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly ITelegramBotClient _botClient;

        public CallbackQueryUpdateService(IMessageProcessor messageProcessor, ITelegramBotClient botClient)
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        }

        public async Task HandleAsync(Update update)
        {
            if (update.Type != UpdateType.CallbackQuery)
            {
                throw new ArgumentException($"Update type should be ${UpdateType.CallbackQuery}", nameof(update));
            }

            MessageResponse response = await _messageProcessor.ProcessAsync(update.CallbackQuery?.Data);

            await _botClient.SendTextMessageAsync(
                chatId: update.GetChat().Id,
                text: response.Text ?? string.Empty,
                replyMarkup: response.BuildReplyMarkup());
        }
    }
}
