using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class TelegramBotMessageSender : IBotMessageSender
    {
        private static IReplyMarkup? _defaultReplyMarkup;
        private readonly ITelegramBotClient _botClient;
        private readonly long _chatId;

        public TelegramBotMessageSender(ITelegramBotClient botClient, IUpdateProvider updateProvider)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _chatId = updateProvider.Update.GetChat().Id;
        }

        private static IReplyMarkup DefaultReplyMarkup
        {
            get
            {
                if (_defaultReplyMarkup != null)
                {
                    return _defaultReplyMarkup;
                }

                _defaultReplyMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { CommandNames.SelectAccount, CommandNames.CreateAccount, CommandNames.DeleteAccount },
                    new KeyboardButton[] { CommandNames.Start, CommandNames.Help }
                })
                {
                    ResizeKeyboard = true
                };

                return _defaultReplyMarkup;
            }
        }

        public async Task SendAsync(BotMessage message)
        {
            await _botClient.SendTextMessageAsync(
                chatId: _chatId,
                text: message.Text,
                replyMarkup: BuildReplyMarkup(message));
        }

        public static IReplyMarkup? BuildReplyMarkup(BotMessage response)
        {
            if (response.Options.Any())
            {
                return new ReplyKeyboardMarkup(new[]
                {
                    response.Options.Select(x => new KeyboardButton(x))
                });
            }

            if (response.TaggedOptions.Any())
            {
                return new InlineKeyboardMarkup(new[]
                    { response.TaggedOptions.Select(x => InlineKeyboardButton.WithCallbackData(x.Key, x.Value)) });
            }

            return DefaultReplyMarkup;
        }
    }
}
