using Finance.Bot.Business.Models;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Finance.Bot.Telegram
{
    public static class Extensions
    {
        public static IReplyMarkup? BuildReplyMarkup(this MessageResponse response)
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

            return null;
        }

        public static Chat GetChat(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                case UpdateType.InlineQuery:
                case UpdateType.ChosenInlineResult:
                case UpdateType.ShippingQuery:
                case UpdateType.PreCheckoutQuery:
                case UpdateType.Poll:
                case UpdateType.PollAnswer:
                case UpdateType.MyChatMember:
                case UpdateType.ChatMember:
                case UpdateType.ChatJoinRequest:
                    throw new ArgumentException($"Update type cannot be {update.Type}.", nameof(update));
                case UpdateType.Message:
                case UpdateType.EditedMessage:
                    return update.Message?.Chat ??
                           throw new ArgumentException($"Message cannot be null for update type {update.Type}.", nameof(update));
                case UpdateType.CallbackQuery:
                    return update.Message?.Chat ??
                           throw new ArgumentException($"Callback Query and Message cannot be null for update type {update.Type}.", nameof(update));
                case UpdateType.ChannelPost:
                case UpdateType.EditedChannelPost:
                    return update.ChannelPost?.Chat ??
                           throw new ArgumentException($"Channel Post cannot be null for update type {update.Type}.", nameof(update));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static User GetUser(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                case UpdateType.InlineQuery:
                case UpdateType.ChosenInlineResult:
                case UpdateType.ShippingQuery:
                case UpdateType.PreCheckoutQuery:
                case UpdateType.Poll:
                case UpdateType.PollAnswer:
                case UpdateType.MyChatMember:
                case UpdateType.ChatMember:
                case UpdateType.ChatJoinRequest:
                    throw new ArgumentException($"Update type cannot be {update.Type}.", nameof(update));
                case UpdateType.Message:
                case UpdateType.EditedMessage:
                    return update.Message?.From ??
                           throw new ArgumentException($"Message cannot be null for update type {update.Type}.", nameof(update));
                case UpdateType.CallbackQuery:
                    return update.Message?.From ??
                           throw new ArgumentException($"Callback Query and Message cannot be null for update type {update.Type}.", nameof(update));
                case UpdateType.ChannelPost:
                case UpdateType.EditedChannelPost:
                    return update.ChannelPost?.From ??
                           throw new ArgumentException($"Channel Post cannot be null for update type {update.Type}.", nameof(update));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
