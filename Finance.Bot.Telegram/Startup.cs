using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

[assembly: FunctionsStartup(typeof(Finance.Bot.Telegram.Startup))]

namespace Finance.Bot.Telegram
{
    public class Startup : FunctionsStartup
    {
        private const string TelegramBotToken = "TelegramBotToken";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(
                Environment.GetEnvironmentVariable(TelegramBotToken, EnvironmentVariableTarget.Process) ?? throw new InvalidOperationException($"Environment variable \"{TelegramBotToken}\" is missing.")));
        }
    }
}
