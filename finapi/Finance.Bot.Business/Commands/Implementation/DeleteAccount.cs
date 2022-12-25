using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Models;
using Finance.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class DeleteAccount : ArgumentedCommand
    {
        private readonly IAccountService _accountService;
        private readonly IBotMessageSender _messageSender;

        public DeleteAccount(IAccountService accountService, 
            IBotMessageSender messageSender, 
            IArgumentProviderBuilder argumentProviderBuilder) : base(1, argumentProviderBuilder)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        protected override async Task ExecuteInternalAsync()
        {
            if (!State.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            AccountModel[] accounts = await _accountService.GetAccountsAsync(userId);

            if (!ArgumentProvider.TryGetInteger(0, out int accountId) || accounts.All(x => x.Id != accountId))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.DeleteAccount;

                KeyValuePair<string, string>[] options = accounts.Select(x =>
                    new KeyValuePair<string, string>(x.Name, x.Id.ToString())).ToArray();
                await _messageSender.SendAsync(new BotMessage("Pick an account to delete", options));
                return;
            }

            State[StateKeys.CommandAwaitingArguments] = null;

            try
            {
                await _accountService.DeleteAccountAsync(accountId);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex, "Failed to delete an account.",
                    $"{CommandNames.CreateAccount} {accountId}");
            }

            await _messageSender.SendAsync(new BotMessage("Account successfully deleted."));
        }
    }
}
