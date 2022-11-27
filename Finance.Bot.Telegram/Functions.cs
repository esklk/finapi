using System;
using System.Linq;
using System.Threading.Tasks;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Bot.Business.Services.Implementation;
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
using Telegram.Bot.Types.ReplyMarkups;

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

            long? chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id;
            if (chatId == null)
            {
                return;
            }

            string commandText;
            switch (update.Type)
            {
                case UpdateType.Message:
                    commandText = update.Message.Text;
                    break;
                case UpdateType.CallbackQuery:
                    commandText = update.CallbackQuery?.Data;
                    break;
                default:
                    await _botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "ERROR: Update type is not supported.");
                    return;
            }

            if (string.IsNullOrWhiteSpace(commandText))
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "ERROR: Message is empty.");
                return;
            }


            MessageResponse response = await new StartedStateMessageProcessor(new State()).ProcessAsync(commandText);

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: response.Text,
                replyMarkup: GetReplyMarkup(response));
        }

        IReplyMarkup? GetReplyMarkup(MessageResponse response)
        {
            if (response.Options != null && response.Options.Any())
            {
                return new ReplyKeyboardMarkup(new[]
                {
                    response.Options.Select(x => new KeyboardButton(x))

                });
            }

            if (response.TaggedOptions != null && response.TaggedOptions.Any())
            {
                return new InlineKeyboardMarkup(new[]
                    { response.TaggedOptions.Select(x => InlineKeyboardButton.WithCallbackData(x.Key, x.Value)) });
            }

            return null;
        }
    }
}
