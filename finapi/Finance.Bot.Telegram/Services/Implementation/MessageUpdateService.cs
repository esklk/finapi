using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Finance.Bot.Telegram.Services.Implementation
{
    internal class MessageUpdateService : IUpdateService
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly ITelegramBotClient _botClient;

        public MessageUpdateService(IMessageProcessor messageProcessor, ITelegramBotClient botClient)
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        }

        public async Task HandleAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                throw new ArgumentException($"Update type should be ${UpdateType.Message}", nameof(update));
            }

            MessageResponse response = await _messageProcessor.ProcessAsync(update.Message?.Text);

            await _botClient.SendTextMessageAsync(
                chatId: update.GetChat().Id,
                text: response.Text ?? string.Empty,
                replyMarkup: response.BuildReplyMarkup());
        }
    }
}
