using Finance.Bot.Telegram.Configuration.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class TelegramBotHostedService : IHostedService
    {
        private readonly ITelegramBotClient botClient;
        private readonly TelegramBotConfiguration configuration;

        public TelegramBotHostedService(IServiceProvider serviceProvider, TelegramBotConfiguration configuration)
        {
            if(serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.botClient = serviceProvider?.CreateScope().ServiceProvider.GetRequiredService<ITelegramBotClient>();

            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await botClient.SetWebhookAsync(
                url: configuration.WebhookUrl,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
