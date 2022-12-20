using Finance.Bot.Business.Commands;
using Finance.Bot.Business.Commands.Implementation;
using Finance.Core.Practices;
using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Bot.Business.Services.Implementation
{
    public class BotCommandFactory : IFactory<IBotCommand, string>
    {
        private readonly IServiceProvider _serviceProvider;

        public BotCommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IBotCommand Create(string commandName) => commandName switch
        {
            CommandNames.CreateAccount => _serviceProvider.GetRequiredService<CreateAccount>(),
            CommandNames.DeleteAccount => _serviceProvider.GetRequiredService<DeleteAccount>(),
            CommandNames.Help => _serviceProvider.GetRequiredService<Help>(),
            CommandNames.SelectAccount => _serviceProvider.GetRequiredService<SelectAccount>(),
            CommandNames.Start => _serviceProvider.GetRequiredService<Start>(),
            _ => throw new InvalidCommandException(commandName)
        };

    }
}
