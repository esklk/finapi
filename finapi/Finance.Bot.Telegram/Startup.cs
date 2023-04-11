using Finance.Bot.Business.Commands.Implementation;
using Finance.Bot.Business.Services;
using Finance.Bot.Business.Services.Implementation;
using Finance.Bot.Telegram.Services;
using Finance.Bot.Telegram.Services.Implementation;
using Finance.Business.Services;
using Finance.Core.Extensions;
using Finance.Core.Practices;
using Functions.Worker.ContextAccessor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram
{
    public static class Startup
    {
        private const string TelegramAppName = "Telegram";
        private const string TelegramBotTokenKey = "TelegramBotToken";

        public static void ConfigureServices(HostBuilderContext builderContext, IServiceCollection services)
        {
            Finance.Data.Bootstrapper.ConfigureServices(builderContext.Configuration, services);
            Finance.Business.Bootstrapper.ConfigureServices(builderContext.Configuration, services);

            builderContext.Configuration["StateStorage:PartitionKey"] = TelegramAppName;
            Finance.Bot.Data.Bootstrapper.ConfigureServices(builderContext.Configuration, services);
            Finance.Bot.Business.Bootstrapper.ConfigureServices(builderContext.Configuration, services);

            services
                .AddScoped<IMessageProcessor>(MessageProcessorFactory)
                .AddScoped<Start>(TelegramStartFactory);

            services
                .AddFunctionContextAccessor()
                .AddSingleton<ITelegramBotClient>(new TelegramBotClient(builderContext.Configuration.GetRequiredValue<string>(TelegramBotTokenKey)))
                .AddScoped<IUpdateProvider, FunctionContextUpdateProvider>()
                .AddScoped<TelegramChatStateServiceFactory>()
                .AddScoped<IStateService>(c => c.GetRequiredService<TelegramChatStateServiceFactory>().Create())
                .AddScoped<IFactory<IUpdateService, Update>, UpdateServiceFactory>()
                .AddScoped<MessageUpdateService>()
                .AddScoped<CallbackQueryUpdateService>()
                .AddScoped<TextMessageSender>()
                .AddScoped<IBotMessageSender>(BotMessageSenderFactory);
        }

        private static Start TelegramStartFactory(IServiceProvider serviceProvider)
        {
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var userLoginService = serviceProvider.GetRequiredService<IUserLoginService>();
            var botMessageSender = serviceProvider.GetRequiredService<IBotMessageSender>();
            User telegramUser = serviceProvider.GetRequiredService<IUpdateProvider>().Update.GetUser();

            return new Start(userService, userLoginService, botMessageSender, telegramUser.FirstName,
                telegramUser.Id.ToString(), TelegramAppName);
        }

        private static IMessageProcessor MessageProcessorFactory(IServiceProvider serviceProvider)
        {
            var messageProcessor = serviceProvider.GetRequiredService<StateManagingMessageProcessor>();
            var messageSender = serviceProvider.GetRequiredService<IBotMessageSender>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            return new ErrorHandlingMessageProcessor(messageProcessor, messageSender, loggerFactory);
        }

        private static IBotMessageSender BotMessageSenderFactory(IServiceProvider serviceProvider)
        {
            var botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();
            var updateProvider = serviceProvider.GetRequiredService<IUpdateProvider>();
            var messageSender = serviceProvider.GetRequiredService<TextMessageSender>();

            return new CallbackQueryDeleteMessageSender(botClient, updateProvider, messageSender);
        }
    }
}
