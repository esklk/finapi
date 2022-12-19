using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Functions
{
    public class Management
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<Management> _logger;

        public Management(ITelegramBotClient botClient, ILoggerFactory loggerFactory)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _logger = loggerFactory?.CreateLogger<Management>() ??
                      throw new ArgumentNullException(nameof(loggerFactory));
        }

        [Function(FunctionNames.Start)]
        public async Task<HttpResponseData> StartAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var handleUpdateFunctionUrl =
                req.Url.AbsoluteUri.Replace(FunctionNames.Start, FunctionNames.Update, StringComparison.OrdinalIgnoreCase);
            await _botClient.SetWebhookAsync(handleUpdateFunctionUrl);

            _logger.LogInformation($"Bot started listening on \"{handleUpdateFunctionUrl}\".");

            return req.CreateResponse(HttpStatusCode.Accepted);
        }

        [Function(FunctionNames.Stop)]
        public async Task<HttpResponseData> StopAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            ILogger log)
        {
            WebhookInfo webhookStatus = await _botClient.GetWebhookInfoAsync();
            if (!string.IsNullOrEmpty(webhookStatus.Url))
            {
                await _botClient.DeleteWebhookAsync();
                log.LogInformation($"Bot stopped listening on \"{webhookStatus.Url}\".");
            }
            return req.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
