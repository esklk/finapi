using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Exceptions;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Models;
using Finance.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class ReportOperation : ArgumentedCommand
    {
        private readonly IOperationService _operationService;
        private readonly IOperationCategoryService _operationCategoryService;
        private readonly IBotMessageSender _messageSender;

        public ReportOperation(IOperationService operationService, 
            IOperationCategoryService operationCategoryService, 
            IBotMessageSender messageSender, 
            IArgumentProviderBuilder argumentProviderBuilder) : base(4, argumentProviderBuilder)
        {
            _operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
            _operationCategoryService = operationCategoryService ?? throw new ArgumentNullException(nameof(operationCategoryService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        protected override async Task ExecuteInternalAsync()
        {
            if (!State.TryGetNumber(StateKeys.UserId, out int userId))
            {
                throw new MissingStateKeyException(StateKeys.UserId);
            }

            if (!State.TryGetNumber(StateKeys.SelectedAccountId, out int selectedAccountId))
            {
                await _messageSender.SendAsync(new BotMessage("No account selected.",
                    new KeyValuePair<string, string>("Select an account", CommandNames.SelectAccount)));
                return;
            }

            if (!ArgumentProvider.TryGetDouble(0, out double amount))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.ReportOperation;
                await _messageSender.SendAsync(new BotMessage("Please enter amount"));
                return;
            }

            if (!ArgumentProvider.TryGetBool(1, out bool isIncome))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.ReportOperation;
                await _messageSender.SendAsync(new BotMessage(
                    "Is it income?",
                    new KeyValuePair<string, string>("Yes", "true"),
                    new KeyValuePair<string, string>("No", "false")));
                return;
            }

            OperationCategoryModel[] operationCategories =
                await _operationCategoryService.GetCategoriesAsync(selectedAccountId);
            if (!ArgumentProvider.TryGetInteger(2, out int categoryId) 
                || operationCategories.All(x => x.Id != categoryId))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.ReportOperation;
                string messageText = isIncome
                    ? $"What is the source of your {amount} income?"
                    : $"What you spent your {amount} on?";

                KeyValuePair<string, string>[] categories = operationCategories.Where(x => x.IsIncome == isIncome)
                    .Select(x => new KeyValuePair<string, string>(x.Name, x.Id.ToString()))
                    .Append(new KeyValuePair<string, string>($"New {(isIncome ? "income" : "expense")} category",
                        $"{CommandNames.CreateOperationCategory} {isIncome}"))
                    .ToArray();

                await _messageSender.SendAsync(new BotMessage(messageText, categories));
                return;
            }

            if (!ArgumentProvider.TryGetDateTime(3, out DateTime madeAt))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.ReportOperation;
                var categoryName = operationCategories.First(x => x.Id == categoryId).Name;
                string messageText = isIncome
                    ? $"When did you get {amount} as {categoryName}?"
                    : $"When did you spend {amount} for {categoryName}?";
                await _messageSender.SendAsync(new BotMessage(messageText,
                    new KeyValuePair<string, string>("Now", DateTime.UtcNow.ToString("O"))));
                return;
            }

            State[StateKeys.CommandAwaitingArguments] = null;

            try
            {
                await _operationService.CreateOperation(userId, selectedAccountId, categoryId, amount, madeAt);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex, "Failed to report an operation.",
                    $"{CommandNames.ReportOperation} {amount},{isIncome},{categoryId},{madeAt:O}");
            }

            await _messageSender.SendAsync(new BotMessage(
                string.Format("Reported {0} of {1} {2} {3}.", 
                    isIncome ? "gaining" : "spending", 
                    amount, 
                    isIncome ? "as" : "for", 
                    operationCategories.First(x => x.Id == categoryId).Name)));
        }
    }
}
