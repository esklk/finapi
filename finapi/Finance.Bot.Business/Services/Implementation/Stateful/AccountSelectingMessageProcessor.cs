using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Business.Exceptions;
using Finance.Business.Models;
using Finance.Business.Services;
using Microsoft.Extensions.Logging;

namespace Finance.Bot.Business.Services.Implementation.Stateful
{
    public class AccountSelectingMessageProcessor : IStatefulMessageProcessor
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountSelectingMessageProcessor> _logger;

        public AccountSelectingMessageProcessor(IAccountService accountService, ILoggerFactory loggerFactory)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _logger = loggerFactory.CreateLogger<AccountSelectingMessageProcessor>();
        }

        public async Task<MessageResponse> ProcessAsync(State state, string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(text));
            }

            if (text.StartsWith(Commands.CreateAccount))
            {
                return await HandleCreateAccountAsync(state, text.Replace(Commands.CreateAccount, string.Empty).Trim());
            }

            if (text.StartsWith(Commands.SelectAccount))
            {
                return await HandleSelectAccountAsync(state, text.Replace(Commands.SelectAccount, string.Empty).Trim());
            }

            throw new InvalidCommandException(text);
        }

        public async Task<MessageResponse> HandleCreateAccountAsync(State state, string arguments)
        {
            if (!state.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            if (string.IsNullOrWhiteSpace(arguments))
            {
                state.ProcessorType = typeof(AccountCreatingMessageProcessor);
                return new MessageResponse("Please enter a name for your new account");
            }

            try
            {
                var newAccount = await _accountService.CreateAccountAsync(arguments, userId);
                state[StateKeys.SelectedAccountId] = newAccount.Id;
                state.ProcessorType = typeof(AccountSelectedMessageProcessor);
                return new MessageResponse($"Account \"{newAccount.Name}\" created and selected!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to an create account.");
                return new MessageResponse("Failed to create an account. Please try again.");
            }
        }

        public async Task<MessageResponse> HandleSelectAccountAsync(State state, string arguments)
        {
            if (!state.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            AccountModel[] accounts = await _accountService.GetAccountsAsync(userId);
            if (int.TryParse(arguments, out int accountId) && accounts.Any(x => x.Id == accountId))
            {
                state.ProcessorType = typeof(AccountSelectedMessageProcessor);
                state[StateKeys.SelectedAccountId] = accountId;
                return new MessageResponse("Account successfully selected.");
            }

            KeyValuePair<string, string>[] responseOptions = accounts
                .Select(x => new KeyValuePair<string, string>(x.Name, $"{Commands.SelectAccount} {x.Id}"))
                .Append(new KeyValuePair<string, string>("Create account", "/createAccount"))
                .ToArray();

            return new MessageResponse("Please pick an account or create a new one.", responseOptions);

        }
    }
}
