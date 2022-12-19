using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Business.Services;
using Microsoft.Extensions.Logging;

namespace Finance.Bot.Business.Services.Implementation.Stateful
{
    public class AccountCreatingMessageProcessor : IStatefulMessageProcessor
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountCreatingMessageProcessor> _logger;

        public AccountCreatingMessageProcessor(IAccountService accountService, ILoggerFactory loggerFactory)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _logger = loggerFactory.CreateLogger<AccountCreatingMessageProcessor>();
        }

        public async Task<MessageResponse> ProcessAsync(State state, string? text)
        {
            if (!state.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return new MessageResponse("Please enter a name for your new account");
            }

            try
            {
                var newAccount = await _accountService.CreateAccountAsync(text.Trim(), userId);
                state[StateKeys.SelectedAccountId] = newAccount.Id;
                state.ProcessorType = typeof(AccountSelectedMessageProcessor);
                return new MessageResponse($"Account \"{newAccount.Name}\" created and selected!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create an account.");
                return new MessageResponse("Failed to create an account. Please try again.");
            }
        }
    }
}
