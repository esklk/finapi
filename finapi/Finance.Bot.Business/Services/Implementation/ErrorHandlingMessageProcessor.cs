using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Microsoft.Extensions.Logging;

namespace Finance.Bot.Business.Services.Implementation
{
    public class ErrorHandlingMessageProcessor : IMessageProcessor
    {
        private const string MessageProcessingError = "Message processing error: {message}.";

        private readonly IMessageProcessor _messageProcessor;
        private readonly IBotMessageSender _messageSender;
        private readonly ILogger<ErrorHandlingMessageProcessor> _logger;

        public ErrorHandlingMessageProcessor(IMessageProcessor messageProcessor, IBotMessageSender messageSender, ILoggerFactory loggerFactory)
        {
            _messageProcessor = messageProcessor ?? throw new ArgumentNullException(nameof(messageProcessor));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _logger = loggerFactory.CreateLogger<ErrorHandlingMessageProcessor>();
        }

        public async Task ProcessAsync(string? text)
        {
            try
            {
                await _messageProcessor.ProcessAsync(text);
            }
            catch (CommandExecutionException ex)
            {
                string messageText = $"Seems like something went wrong: {ex.Message.TrimEnd('.')}.";
                BotMessage message = ex.RetryAvailable
                    // RetryCommand is not null when RetryAvailable is true
                    ? new BotMessage(messageText, new KeyValuePair<string, string>("Retry", ex.RetryCommand!))
                    : new BotMessage(messageText);
                await _messageSender.SendAsync(message);
                _logger.LogError(ex, MessageProcessingError, ex.Message);
            }
            catch (ArgumentException ex)
            {
                await _messageSender.SendAsync(
                    new BotMessage($"Seems like something went wrong: {ex.Message.TrimEnd('.')}. Please review your actions and try again or let's /Start from the scratch."));
                _logger.LogError(ex, MessageProcessingError, ex.Message);
            }
            catch (Exception ex)
            {
                await _messageSender.SendAsync(
                    new BotMessage($"I'm afraid I'm unable to process your request now: {ex.Message.TrimEnd('.')}. Please try again later."));
                _logger.LogError(ex, MessageProcessingError, ex.Message);
            }
        }
    }
}
