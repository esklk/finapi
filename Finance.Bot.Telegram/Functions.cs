using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Finance.Bot.Telegram
{
    public class Functions
    {
        private const string Start = "Start";
        private const string Stop = "Stop";
        private const string Update = "Update";

        private readonly ITelegramBotClient _botClient;

        public Functions(ITelegramBotClient botClient)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
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
            var request = await req.ReadAsStringAsync();
            var update = JsonConvert.DeserializeObject<Update>(request);

            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
            {
                return;
            }

            await _botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Received a message: {update.Message.Text}.");

        }
    }
}
