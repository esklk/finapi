using Finance.Bot.Business.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Finance.Bot.Telegram.Services.Implementation
{
    internal class CallbackQueryUpdateService : IUpdateService
    {
        private readonly IMessageProcessor _messageProcessor;

        public CallbackQueryUpdateService(IMessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
        }

        public async Task HandleAsync(Update update)
        {
            if (update.Type != UpdateType.CallbackQuery)
            {
                throw new ArgumentException($"UpdateHandling type should be ${UpdateType.CallbackQuery}", nameof(update));
            }

            await _messageProcessor.ProcessAsync(update.CallbackQuery?.Data);
        }
    }
}
