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
        private const string DateFormat = "dddd, MMMM d, yyyy";

        private readonly IOperationService _operationService;
        private readonly IOperationCategoryService _operationCategoryService;
        private readonly IBotMessageSender _messageSender;

        public ReportOperation(IOperationService operationService, 
            IOperationCategoryService operationCategoryService, 
            IBotMessageSender messageSender, 
            IArgumentProviderBuilder argumentProviderBuilder) : base(3, argumentProviderBuilder)
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

            OperationCategoryModel? selectedCategory = await GetOperationCategory(selectedAccountId);
            if (selectedCategory == null)
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.ReportOperation;
                string messageText = "What kind of operation you would like to report?";

                OperationCategoryModel[] operationCategories =
                    await _operationCategoryService.GetCategoriesAsync(selectedAccountId);

                KeyValuePair<string, string>[] categories = operationCategories
                    .OrderBy(x=>x.IsIncome)
                    .ThenBy(x=>x.Name)
                    .Select(x => new KeyValuePair<string, string>(x.Name, x.Id.ToString()))
                    .Append(new KeyValuePair<string, string>("New category",
                        $"{CommandNames.CreateOperationCategory}"))
                    .ToArray();

                await _messageSender.SendAsync(new BotMessage(messageText, categories));
                return;
            }


            if (!ArgumentProvider.TryGetDouble(1, out double amount))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.ReportOperation;
                await _messageSender.SendAsync(new BotMessage("Please enter amount"));
                return;
            }

            if (!ArgumentProvider.TryGetDateTime(2, out DateTime madeAt))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.ReportOperation;
                string messageText = selectedCategory.IsIncome
                    ? $"When did you get {amount} as {selectedCategory.Name}?"
                    : $"When did you spend {amount} for {selectedCategory.Name}?";
                var today = DateTime.UtcNow.Date;
                await _messageSender.SendAsync(new BotMessage(messageText,
                    today.ToString(DateFormat), today.AddDays(-1).ToString(DateFormat),
                    today.AddDays(-2).ToString(DateFormat), today.AddDays(-3).ToString(DateFormat),
                    today.AddDays(-4).ToString(DateFormat), today.AddDays(-5).ToString(DateFormat),
                    today.AddDays(-6).ToString(DateFormat), today.AddDays(-7).ToString(DateFormat)));
                return;
            }

            State[StateKeys.CommandAwaitingArguments] = null;

            try
            {
                await _operationService.CreateOperation(userId, selectedAccountId, selectedCategory.Id, amount, madeAt);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex, "Failed to report an operation.",
                    $"{CommandNames.ReportOperation} {selectedCategory.Id},{amount},{madeAt:O}");
            }

            BotMessage message = new BotMessage(
                $"Reported {(selectedCategory.IsIncome ? "gaining" : "spending")} of {amount} {(selectedCategory.IsIncome ? "as" : "for")} {selectedCategory.Name}.",
                new KeyValuePair<string, string>(
                    $"Report more {(selectedCategory.IsIncome ? "income" : "expenses")} to {selectedCategory.Name}",
                    $"{CommandNames.ReportOperation} {selectedCategory.Id}")
            );

            await _messageSender.SendAsync(message);
        }

        private async Task<OperationCategoryModel?> GetOperationCategory(int accountId)
        {
            return ArgumentProvider.TryGetInteger(0, out int categoryId)
                ? await _operationCategoryService.GetCategoryAsync(categoryId)
                : null;
        }
    }
}
