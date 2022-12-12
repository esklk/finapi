using System;
using Finance.Bot.Business.Services;
using Finance.Core.Practices;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
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
                UpdateType.Message => new MessageUpdateService(BuildMessageProcessor(update), _serviceProvider.GetRequiredService<ITelegramBotClient>()),
                UpdateType.CallbackQuery => new CallbackQueryUpdateService(BuildMessageProcessor(update), _serviceProvider.GetRequiredService<ITelegramBotClient>()),
                _ => throw new ArgumentException($"Update type cannot be {update.Type}.", nameof(update))
            };
        }

        private IMessageProcessor BuildMessageProcessor(Update update)
        {
            IStateService stateService = _serviceProvider
                .GetRequiredService<IFactory<IStateService, Update>>()
                .Create(update);

            return _serviceProvider
                .GetRequiredService<IFactory<IMessageProcessor, IStateService>>()
                .Create(stateService);
        }
    }
}
