using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class CreateOperationCategory : ArgumentedCommand
    {
        private readonly IOperationCategoryService _operationCategoryService;
        private readonly IBotMessageSender _messageSender;

        public CreateOperationCategory(IOperationCategoryService operationCategoryService,
            IBotMessageSender messageSender,
            IArgumentProviderBuilder argumentProviderBuilder) : base(2, argumentProviderBuilder)
        {
            _operationCategoryService = operationCategoryService ??
                                        throw new ArgumentNullException(nameof(operationCategoryService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        protected override async Task ExecuteInternalAsync()
        {
            if (!State.TryGetNumber(StateKeys.SelectedAccountId, out int selectedAccountId))
            {
                await _messageSender.SendAsync(new BotMessage("No account selected.",
                    new KeyValuePair<string, string>("Select an account", CommandNames.SelectAccount)));
                return;
            }

            if (!ArgumentProvider.TryGetBool(0, out bool isIncome))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.CreateOperationCategory;
                await _messageSender.SendAsync(new BotMessage(
                    "Is it income category?",
                    new KeyValuePair<string, string>("Yes", "true"),
                    new KeyValuePair<string, string>("No", "false")));
                return;
            }

            if (!ArgumentProvider.TryGetString(1, out string? name))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.CreateOperationCategory;
                await _messageSender.SendAsync(new BotMessage("Please enter an operation category name"));
                return;
            }

            State[StateKeys.CommandAwaitingArguments] = null;

            try
            {
                await _operationCategoryService.CreateCategoryAsync(name, isIncome, selectedAccountId);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex, "Failed to create an operation category.",
                    $"{CommandNames.CreateOperationCategory} {name},{isIncome}");
            }

            await _messageSender.SendAsync(
                new BotMessage($"Created {(isIncome ? "income" : "expense")} category: {name}."));
        }
    }
}
