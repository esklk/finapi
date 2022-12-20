using Finance.Bot.Business.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Finance.Bot.Telegram.Services.Implementation
{
    internal class MessageUpdateService : IUpdateService
    {
        private readonly IMessageProcessor _messageProcessor;

        public MessageUpdateService(IMessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
        }

        public async Task HandleAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                throw new ArgumentException($"UpdateHandling type should be ${UpdateType.Message}", nameof(update));
            }

            await _messageProcessor.ProcessAsync(update.Message?.Text);
        }
    }
}
