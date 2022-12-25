using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Models;
using Finance.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class SelectAccount : ArgumentedCommand
    {
        private readonly IAccountService _accountService;
        private readonly IBotMessageSender _messageSender;

        public SelectAccount(IAccountService accountService,
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
            if (ArgumentProvider.TryGetInteger(0, out int accountId) && accounts.Any(x => x.Id == accountId))
            {
                AccountModel account = accounts.First(x => x.Id == accountId);
                State[StateKeys.SelectedAccountId] = accountId;
                State[StateKeys.CommandAwaitingArguments] = null;
                await _messageSender.SendAsync(new BotMessage($"Account \"{account.Name}\" selected."));
                return;
            }

            State[StateKeys.CommandAwaitingArguments] = CommandNames.SelectAccount;

            KeyValuePair<string, string>[] responseOptions = accounts
                .Select(x => new KeyValuePair<string, string>(x.Name, x.Id.ToString()))
                .Append(new KeyValuePair<string, string>("Create account", "/createAccount"))
                .ToArray();

            await _messageSender.SendAsync(new BotMessage("Please pick an account or create a new one.",
                responseOptions));
        }
    }
}
