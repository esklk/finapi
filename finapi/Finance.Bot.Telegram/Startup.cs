using System;
using Finance.Bot.Business.Models;
using Finance.Bot.Data.Models;
using Finance.Business.Services;
using Finance.Business.Services.Implementation;
using Finance.Core.Practices;
using Finance.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
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

[assembly: FunctionsStartup(typeof(Finance.Bot.Telegram.Startup))]

namespace Finance.Bot.Telegram
{
    public class Startup : FunctionsStartup
    {
        private const string AzureStorageConnectionString = "AzureStorageConnectionString";
        private const string TelegramAppName = "Telegram";
        private const string TelegramBotToken = "TelegramBotToken";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<FinApiDbContext>(x =>
                x.UseSqlServer(
                    GetSetting(FinApiMySqlDesignTimeDbContextFactory.FinapiDatabaseConnectionStringEnvVarName)));

            builder.Services
                .AddAutoMapper(typeof(BusinessDefaultMappingProfile))
                .AddScoped<IUserService, UserService>()
                .AddScoped<IUserLoginService, UserLoginService>();

            builder.Services
                .AddScoped<IRepository<StateEntity, string>, AzureTableEntityRepository<StateEntity>>(x =>
                new AzureTableEntityRepository<StateEntity>(GetSetting(AzureStorageConnectionString), TelegramAppName));

            builder.Services
                .AddAutoMapper(typeof(BotBusinessDefaultMappingProfile))
                .AddScoped<IFactory<IStateService, string>, StateServiceFactory>()
                .AddScoped<IFactory<IMessageProcessor, State>, StatefulMessageProcessorFactory>()
                .AddScoped<IFactory<IMessageProcessor, IStateService>, StateServiceStatefulMessageProcessorFactory>();

            builder.Services
                .AddSingleton<ITelegramBotClient>(new TelegramBotClient(GetSetting(TelegramBotToken)))
                .AddScoped<IFactory<IUpdateService, Update>, UpdateServiceFactory>()
                .AddScoped<IFactory<IStateService, Update>, TelegramStateServiceFactory>(x =>
                    new TelegramStateServiceFactory(x, typeof(StartedStateMessageProcessor)));
        }

        private static string GetSetting(string key)
        {
            return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ??
                   throw new InvalidOperationException($"Environment variable \"{key}\" is missing.");
        }
    }
}
