using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class TextMessageSender : IBotMessageSender
    {
        private readonly ITelegramBotClient _botClient;
        private readonly long _chatId;
        public TextMessageSender(ITelegramBotClient botClient, IUpdateProvider updateProvider)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _chatId = updateProvider.Update.GetChat().Id;
        }

        public async Task SendAsync(BotMessage message)
        {
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
