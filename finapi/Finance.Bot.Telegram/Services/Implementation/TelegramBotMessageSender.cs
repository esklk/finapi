using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class TelegramBotMessageSender : IBotMessageSender
    {
        private readonly ITelegramBotClient _botClient;
        private readonly long _chatId;
        private readonly string? _queryId;

        public TelegramBotMessageSender(ITelegramBotClient botClient, IUpdateProvider updateProvider)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _chatId = updateProvider.Update.GetChat().Id;
            _queryId = updateProvider.Update.CallbackQuery?.Id;
        }

        public async Task SendAsync(BotMessage message)
        {
            if (!string.IsNullOrWhiteSpace(_queryId))
            {
                // a workaround to remove a loader from callback button
                await _botClient.AnswerCallbackQueryAsync(_queryId);
            }

            await _botClient.SendTextMessageAsync(
                chatId: _chatId,
                text: message.Text,
                replyMarkup: BuildReplyMarkup(message));
        }

        public static IReplyMarkup BuildReplyMarkup(BotMessage response)
        {
            if (response.Options.Any())
            {
                return new ReplyKeyboardMarkup(response.Options.Select(x => new KeyboardButton(x)).Chunk(2))
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
            }

            if (response.TaggedOptions.Any())
            {
                return new InlineKeyboardMarkup(response.TaggedOptions
                    .Select(x => InlineKeyboardButton.WithCallbackData(x.Key, x.Value))
                    .Chunk(2));
            }

            return new ReplyKeyboardRemove();
        }
    }
}
