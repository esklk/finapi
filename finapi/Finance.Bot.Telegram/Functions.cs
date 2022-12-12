using System;
using System.Threading.Tasks;
using Finance.Bot.Telegram.Services;
using Finance.Core.Practices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram
{
    public class Functions
    {
        private const string Start = "Start";
        private const string Stop = "Stop";
        private const string Update = "Update";

        private readonly ITelegramBotClient _botClient;
        private readonly IFactory<IUpdateService, Update> _updateServiceFactory;

        public Functions(ITelegramBotClient botClient, IFactory<IUpdateService, Update> updateServiceFactory)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _updateServiceFactory = updateServiceFactory ?? throw new ArgumentNullException(nameof(updateServiceFactory));
        }

        [FunctionName(Start)]
        public async Task<IActionResult> StartAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var handleUpdateFunctionUrl =
                req.GetEncodedUrl().Replace(Start, Update, StringComparison.OrdinalIgnoreCase);
            await _botClient.SetWebhookAsync(handleUpdateFunctionUrl);

            log.LogInformation($"Bot started listening on \"{handleUpdateFunctionUrl}\".");

            return new OkResult();
        }

        [FunctionName(Stop)]
        public async Task<IActionResult> StopAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            WebhookInfo webhookStatus = await _botClient.GetWebhookInfoAsync();
            if (!string.IsNullOrEmpty(webhookStatus.Url))
            {
                await _botClient.DeleteWebhookAsync();
                log.LogInformation($"Bot stopped listening on \"{webhookStatus.Url}\".");
            }

            return new OkResult();
        }

        [FunctionName(Update)]
        public async Task UpdateAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string request = await req.ReadAsStringAsync();
            var update = JsonConvert.DeserializeObject<Update>(request);
            if (update != null)
            {
                await _updateServiceFactory.Create(update).HandleAsync(update);
            }
        }
    }
}
