using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class CreateAccount : ArgumentedCommand
    {
        private readonly IAccountService _accountService;
        private readonly IBotMessageSender _messageSender;

        public CreateAccount(IAccountService accountService, 
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

            if (!ArgumentProvider.TryGetString(0, out string? accountName))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.CreateAccount;
                await _messageSender.SendAsync(new BotMessage("Please enter a name for your new account"));
                return;
            }

            State[StateKeys.CommandAwaitingArguments] = null;

            try
            {
                var newAccount = await _accountService.CreateAccountAsync(accountName, userId);

                await _messageSender.SendAsync(new BotMessage($"Account \"{newAccount.Name}\" created.",
                    new KeyValuePair<string, string>($"Select {newAccount.Name}", $"{CommandNames.SelectAccount} {newAccount.Id}"),
                    new KeyValuePair<string, string>("See full account list", CommandNames.SelectAccount)));
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex, "Failed to create an account.",
                    $"{CommandNames.CreateAccount} {accountName}");
            }
        }
    }
}
