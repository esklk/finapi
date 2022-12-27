using System.Globalization;
using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Services;
using System.Text;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class GetOperationsReport : ArgumentedCommand
    {
        private readonly IOperationService _operationService;
        private readonly IBotMessageSender _messageSender;

        public GetOperationsReport(IOperationService operationService, 
            IBotMessageSender messageSender, 
            IArgumentProviderBuilder argumentProviderBuilder) : base(2, argumentProviderBuilder)
        {
            _operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(operationService));
        }

        protected override async Task ExecuteInternalAsync()
        {
            if (!State.TryGetNumber(StateKeys.SelectedAccountId, out int accountId))
            {
                await _messageSender.SendAsync(new BotMessage("No account selected.",
                    new KeyValuePair<string, string>("Select an account", CommandNames.SelectAccount)));
                return;
            }

            if(!ArgumentProvider.TryGetDateTime(0, out DateTime from))
            {
                var now = DateTime.UtcNow;
                var startOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
                State[StateKeys.CommandAwaitingArguments] = CommandNames.GetOperationsReport;
                await _messageSender.SendAsync(new BotMessage(
                    "Starting from what date you want the report to contain operations?",
                    new KeyValuePair<string, string>("Start of current month",
                        startOfCurrentMonth.ToString("yyyy-MM-dd")),
                    new KeyValuePair<string, string>("Start of last month",
                        startOfCurrentMonth.AddMonths(-1).ToString("yyyy-MM-dd"))));
                return;
            }

            if (!ArgumentProvider.TryGetDateTime(1, out DateTime to))
            {
                State[StateKeys.CommandAwaitingArguments] = CommandNames.GetOperationsReport;
                await _messageSender.SendAsync(new BotMessage(
                    $"In the report you want to see operation from {from:MMM dd, yyyy} to what date?",
                    new KeyValuePair<string, string>("Today", DateTime.UtcNow.ToString("yyyy-MM-dd"))));
                return;
            }

            var operations = await _operationService
                .GetOperationsAsync(accountId, q => q
                .Where(o => o.CreatedOn > from.Date && o.CreatedOn < to.Date.AddDays(1))
                .GroupBy(o => new { o.Category.Name, o.Category.IsIncome })
                .Select(g => new
                {
                    Category = g.Key.Name,
                    Total = g.Key.IsIncome? g.Sum(o => o.Ammount) : g.Sum(o => o.Ammount) * -1
                })
                .OrderByDescending(x => x.Total));

            var messageTextBuilder = new StringBuilder()
                .AppendFormat("Please see your operations report between {0:MMM dd, yyyy} and {1:MMM dd, yyyy}:", from, to)
                .AppendLine()
                .AppendFormat("Balance: {0}", operations.Sum(x => x.Total))
                .AppendLine()
                .AppendFormat("Total income: {0}", operations.Where(x => x.Total > 0).Sum(x => x.Total))
                .AppendLine()
                .AppendFormat("Total expense: {0}", operations.Where(x => x.Total < 0).Sum(x => x.Total))
                .AppendLine();

            foreach (var operation in operations)
            {
                messageTextBuilder
                    .AppendFormat("{0}: {1}", operation.Category, operation.Total)
                    .AppendLine();
            }

            await _messageSender.SendAsync(new BotMessage(messageTextBuilder.ToString()));
        }
    }
}
