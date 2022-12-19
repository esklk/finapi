﻿using System;
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
using Finance.Bot.Business.Services.Implementation.Stateful;
using Functions.Worker.ContextAccessor;

//[assembly: FunctionsStartup(typeof(Finance.Bot.Telegram.Startup))]

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
                .AddScoped<IMessageProcessor, StatefulMessageProcessor>()
                .AddAutoMapper(typeof(BotBusinessDefaultMappingProfile))
                .AddScoped<IFactory<IStateService, string>, StateServiceFactory>(c => new StateServiceFactory(c.GetRequiredService<IServiceProvider>(), typeof(TelegramStartedMessageProcessor)))
                .AddScoped<IFactory<IStatefulMessageProcessor, Type>, ServiceProviderFactory<IStatefulMessageProcessor>>()
                .AddScoped<SignedInMessageProcessor>();

            builder.Services
                .AddFunctionContextAccessor()
                .AddSingleton<IUpdateProvider, HttpContextUpdateProvider>()
                .AddSingleton<ITelegramBotClient>(new TelegramBotClient(GetSetting(TelegramBotToken)))
                .AddScoped<TelegramChatStateServiceFactory>()
                .AddScoped<IStateService>(c => c.GetRequiredService<TelegramChatStateServiceFactory>().Create())
                .AddScoped<IFactory<IUpdateService, Update>, UpdateServiceFactory>()
                .AddScoped<MessageUpdateService>()
                .AddScoped<CallbackQueryUpdateService>()
                .AddScoped<TelegramStartedMessageProcessor>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);
        }

        private static string GetSetting(string key)
        {
            return Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ??
                   throw new InvalidOperationException($"Environment variable \"{key}\" is missing.");
        }
    }
}
