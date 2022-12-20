using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Models;
using Finance.Business.Services;
using Microsoft.Extensions.Logging;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class DeleteAccount : IBotCommand
    {
        private readonly IAccountService _accountService;
        private readonly IBotMessageSender _messageSender;
        private readonly ILogger<DeleteAccount> _logger;

        public DeleteAccount(IAccountService accountService, IBotMessageSender messageSender, ILoggerFactory loggerFactory)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            _logger = loggerFactory.CreateLogger<DeleteAccount>();
        }
        public async Task ExecuteAsync(State state, string[] arguments)
        {
            if (!state.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            AccountModel[] accounts = await _accountService.GetAccountsAsync(userId);

            if (!arguments.Any() 
                || !int.TryParse(arguments[0], out int accountId) 
                || accounts.All(x => x.Id != accountId))
            {
                state[StateKeys.AwaitingArguments] = CommandNames.DeleteAccount;

                KeyValuePair<string, string>[] options = accounts.Select(x =>
                    new KeyValuePair<string, string>(x.Name, x.Id.ToString())).ToArray();
                await _messageSender.SendAsync(new BotMessage("Pick an account to delete", options));
                return;
            }

            try
            {
                await _accountService.DeleteAccountAsync(accountId);
                await _messageSender.SendAsync(new BotMessage("Account successfully deleted."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete an account.");
                await _messageSender.SendAsync(new BotMessage("Failed to delete an account.",
                    new KeyValuePair<string, string>("Try again", $"{CommandNames.CreateAccount} {accountId}")));
            }
            finally
            {
                state.Data.Remove(StateKeys.AwaitingArguments);
            }
        }
    }
}
