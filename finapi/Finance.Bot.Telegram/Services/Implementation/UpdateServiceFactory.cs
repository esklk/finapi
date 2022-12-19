using Finance.Core.Practices;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Finance.Bot.Telegram.Services.Implementation
{
    internal class UpdateServiceFactory : IFactory<IUpdateService, Update>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IUpdateService Create(Update update)
        {
            return update.Type switch
            {
                UpdateType.Message => _serviceProvider.GetRequiredService<MessageUpdateService>(),
                UpdateType.CallbackQuery => _serviceProvider.GetRequiredService<CallbackQueryUpdateService>(),
                _ => throw new ArgumentException($"UpdateHandling type cannot be {update.Type}.", nameof(update))
            };
        }
    }
}
