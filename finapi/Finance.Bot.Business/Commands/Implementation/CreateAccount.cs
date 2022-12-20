using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Services;
using Microsoft.Extensions.Logging;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class CreateAccount : IBotCommand
    {
        private readonly IAccountService _accountService;
        private readonly IBotMessageSender _messageSender;
        private readonly ILogger<CreateAccount> _logger;

        public CreateAccount(IAccountService accountService, IBotMessageSender messageSender, ILoggerFactory loggerFactory)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _logger = loggerFactory.CreateLogger<CreateAccount>();
        }

        public async Task ExecuteAsync(State state, string[] arguments)
        {
            if (!state.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            if (!arguments.Any() || string.IsNullOrWhiteSpace(arguments[0]))
            {
                state[StateKeys.AwaitingArguments] = CommandNames.CreateAccount;
                await _messageSender.SendAsync(new BotMessage("Please enter a name for your new account"));
                return;
            }

            string accountName = arguments[0].Trim();
            try
            {
                var newAccount = await _accountService.CreateAccountAsync(accountName, userId);

                await _messageSender.SendAsync(new BotMessage($"Account \"{newAccount.Name}\" created!",
                    new KeyValuePair<string, string>($"Select {newAccount.Name}",
                        $"{CommandNames.SelectAccount} {newAccount.Id}"),
                    new KeyValuePair<string, string>("See full account list", CommandNames.SelectAccount)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create an account.");
                await _messageSender.SendAsync(new BotMessage("Failed to create an account.", 
                    new KeyValuePair<string, string>("Try again", $"{CommandNames.CreateAccount} {accountName}")));
            }
        }
    }
}
