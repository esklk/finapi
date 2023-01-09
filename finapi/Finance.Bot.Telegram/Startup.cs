using Finance.Bot.Business.Commands;
using Finance.Bot.Business.Commands.Implementation;
using Finance.Bot.Data.Models;
using Finance.Business.Services;
using Finance.Business.Services.Implementation;
using Finance.Core.Practices;
using Finance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

using BusinessDefaultMappingProfile = Finance.Business.Mapping.DefaultMappingProfile;
using BotBusinessDefaultMappingProfile = Finance.Bot.Business.Mapping.DefaultMappingProfile;
using Finance.Bot.Data.Services.Implementation;
using Finance.Bot.Business.Services;
using Finance.Bot.Business.Services.Implementation;
using Finance.Bot.Telegram.Services;
using Finance.Bot.Telegram.Services.Implementation;
using Telegram.Bot.Types;
using Functions.Worker.ContextAccessor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Finance.Bot.Telegram
{
    public static class Startup
    {
        private const string AzureStorageConnectionString = "AzureWebJobsStorage";
        private const string TelegramAppName = "Telegram";
        private const string TelegramBotToken = "TelegramBotToken";

        public static void Configure(HostBuilderContext builderContext, IServiceCollection services)
        {
            if (builderContext.HostingEnvironment.IsDevelopment())
            {
                services.AddDbContext<FinApiDbContext>(opt => opt.UseInMemoryDatabase("Finance"));
            }
            else
            {
                services.AddDbContext<FinApiDbContext>(opt =>
                    opt.UseSqlServer(
                        builderContext.Configuration["DatabaseConfiguration:ConnectionString"]!,
                        options => options.EnableRetryOnFailure()));
            }

            services
                .AddAutoMapper(typeof(BusinessDefaultMappingProfile))
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IOperationCategoryService, OperationCategoryService>()
                .AddScoped<IOperationService, OperationService>()
                .AddScoped<IUserLoginService, UserLoginService>()
                .AddScoped<IUserService, UserService>();

            services
                .AddScoped<IRepository<StateEntity, string>, AzureTableEntityRepository<StateEntity>>(x =>
                    new AzureTableEntityRepository<StateEntity>(
                        builderContext.Configuration[AzureStorageConnectionString]!, TelegramAppName));

            services
                .AddScoped<StateManagingMessageProcessor>()
                .AddScoped<IMessageProcessor>(MessageProcessorFactory)
                .AddAutoMapper(typeof(BotBusinessDefaultMappingProfile))
                .AddScoped<IFactory<IStateService, string>, StateServiceFactory>(c =>
                    new StateServiceFactory(c.GetRequiredService<IServiceProvider>()))
                .AddScoped<IFactory<IBotCommand, string>, BotCommandFactory>()
                .AddScoped<IArgumentProviderBuilder, CommandArgumentProviderBuilder>()
                .AddScoped<CancelCommand>()
                .AddScoped<CreateAccount>()
                .AddScoped<CreateOperationCategory>()
                .AddScoped<DeleteAccount>()
                .AddScoped<GetOperationsReport>()
                .AddScoped<Help>()
                .AddScoped<ReportOperation>()
                .AddScoped<SelectAccount>()
                .AddScoped<Start>(TelegramStartFactory);

            services
                .AddFunctionContextAccessor()
                .AddSingleton<ITelegramBotClient>(new TelegramBotClient(builderContext.Configuration[TelegramBotToken]!))
                .AddScoped<IUpdateProvider, FunctionContextUpdateProvider>()
                .AddScoped<TelegramChatStateServiceFactory>()
                .AddScoped<IStateService>(c => c.GetRequiredService<TelegramChatStateServiceFactory>().Create())
                .AddScoped<IFactory<IUpdateService, Update>, UpdateServiceFactory>()
                .AddScoped<MessageUpdateService>()
                .AddScoped<CallbackQueryUpdateService>()
                .AddScoped<IBotMessageSender, TelegramBotMessageSender>();
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
    }
}
