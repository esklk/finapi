using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Business.Models;
using Finance.Business.Services;

namespace Finance.Bot.Business.Services.Implementation.Stateful
{
    public class SignedInMessageProcessor : IStatefulMessageProcessor
    {
        private readonly IAccountService _accountService;

        public SignedInMessageProcessor(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public async Task<MessageResponse> ProcessAsync(State state, string? text)
        {
            switch (text)
            {
                case Commands.SelectAccount:
                    return await HandleSelectAccountAsync(state);
                case Commands.Help:
                    return new MessageResponse("No content here so far.");
                default:
                    throw new InvalidCommandException(text);
            }
        }

        private async Task<MessageResponse> HandleSelectAccountAsync(State state)
        {
            if (!state.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            AccountModel[] accounts = await _accountService.GetAccountsAsync(userId);

            KeyValuePair<string, string>[] responseOptions = accounts
                .Select(x => new KeyValuePair<string, string>(x.Name, $"{Commands.SelectAccount} {x.Id}"))
                .Append(new KeyValuePair<string, string>("Create account", "/createAccount"))
                .ToArray();

            state.ProcessorType = typeof(AccountSelectingMessageProcessor);

            return new MessageResponse("Please pick an account or create a new one.", responseOptions);
        }
    }
}
