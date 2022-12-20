using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Bot.Business.Services.Implementation;
using Finance.Business.Models;
using Finance.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class SelectAccount : IBotCommand
    {
        private readonly IAccountService _accountService;
        private readonly IBotMessageSender _messageSender;

        public SelectAccount(IAccountService accountService, IBotMessageSender messageSender)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        public async Task ExecuteAsync(State state, string[] arguments)
        {
            if (!state.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            AccountModel[] accounts = await _accountService.GetAccountsAsync(userId);
            if (arguments.Any() && int.TryParse(arguments[0], out int accountId) && accounts.Any(x => x.Id == accountId))
            {
                state[StateKeys.SelectedAccountId] = accountId;
                await _messageSender.SendAsync(new BotMessage("Account selected."));
                return;
            }

            KeyValuePair<string, string>[] responseOptions = accounts
                .Select(x => new KeyValuePair<string, string>(x.Name, $"{CommandNames.SelectAccount} {x.Id}"))
                .Append(new KeyValuePair<string, string>("Create account", "/createAccount"))
                .ToArray();

            await _messageSender.SendAsync(new BotMessage("Please pick an account or create a new one.",
                responseOptions));
        }
    }
}
