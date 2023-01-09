using System.Net;
using Finance.Bot.Business.Constants;
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
            await _botClient.SetMyCommandsAsync(new[]
            {
                new BotCommand { Command = CommandNames.ReportOperation, Description = "Report an operation" },
                new BotCommand { Command = CommandNames.GetOperationsReport, Description = "Get operations report" },
                new BotCommand { Command = CommandNames.Help, Description = "Get help" },
                new BotCommand { Command = CommandNames.SelectAccount, Description = "Select an account" },
                new BotCommand { Command = CommandNames.CreateAccount, Description = "Create an account" },
                new BotCommand { Command = CommandNames.DeleteAccount, Description = "Delete an account" },
                new BotCommand { Command = CommandNames.CreateOperationCategory, Description = "Create an operation category" },
                new BotCommand { Command = CommandNames.Start, Description = "Restart" }
            });
            await _botClient.SetChatMenuButtonAsync(null, new MenuButtonCommands());

            _logger.LogInformation($"Bot started listening on \"{handleUpdateFunctionUrl}\".");
            
            return req.CreateResponse(HttpStatusCode.Accepted);
        }

        [Function(FunctionNames.Stop)]
        public async Task<HttpResponseData> StopAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            WebhookInfo webhookStatus = await _botClient.GetWebhookInfoAsync();
            if (string.IsNullOrEmpty(webhookStatus.Url))
            {
                _logger.LogInformation("Bot is not listening to any urls.");
                return req.CreateResponse(HttpStatusCode.Accepted);
            }

            await _botClient.DeleteWebhookAsync();
            _logger.LogInformation($"Bot stopped listening on \"{webhookStatus.Url}\".");
            return req.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
