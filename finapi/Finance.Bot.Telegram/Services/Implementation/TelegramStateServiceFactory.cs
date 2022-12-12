using System;
using Finance.Bot.Business.Services;
using Finance.Business.Services;
using Finance.Core.Practices;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class TelegramStateServiceFactory : IFactory<IStateService, Update>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Type _initialProcessorType;


        public TelegramStateServiceFactory(IServiceProvider serviceProvider, Type initialProcessorType)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _initialProcessorType = initialProcessorType ?? throw new ArgumentNullException(nameof(initialProcessorType));
        }

        public IStateService Create(Update update)
        {
            IStateService internalStateService = _serviceProvider.GetRequiredService<IFactory<IStateService, string>>()
                .Create(update.GetChat().Id.ToString());
            IUserService userService = _serviceProvider.GetRequiredService<IUserService>();
            IUserLoginService userLoginService = _serviceProvider.GetRequiredService<IUserLoginService>();

            return new TelegramStateService(
                internalStateService, 
                userService,
                userLoginService,
                update.GetUser(),
                _initialProcessorType);
        }
    }
}
