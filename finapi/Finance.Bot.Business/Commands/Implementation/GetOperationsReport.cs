using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;
using Finance.Business.Services;
using System.Text;

namespace Finance.Bot.Business.Commands.Implementation
{
    public class GetOperationsReport : ArgumentedCommand
    {
        private const string DateFormat = "MMMM d, yyyy";

        private readonly IOperationService _operationService;
        private readonly IBotMessageSender _messageSender;

        public GetOperationsReport(IOperationService operationService, 
            IBotMessageSender messageSender, 
            IArgumentProviderBuilder argumentProviderBuilder) : base(2, argumentProviderBuilder)
        {
            _operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
            _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        }

        protected override async Task ExecuteInternalAsync()
        {
            if (!State.TryGetNumber(StateKeys.SelectedAccountId, out int accountId))
            {
                await _messageSender.SendAsync(new BotMessage("No account selected.",
                    new KeyValuePair<string, string>("Select an account", CommandNames.SelectAccount)));
                return;
            }

            DateTime? from = await GetStartDateAsync();
            if (!from.HasValue)
            {
                return;
            }

            DateTime? to = await GetEndDateAsync(from.Value);
            if (!to.HasValue)
            {
                return;
            }

            var operations = await _operationService
                .GetOperationsAsync(accountId, q => q
                    .Where(o => o.CreatedOn >= from.Value && o.CreatedOn <= to.Value)
                    .GroupBy(o => new { o.Category.Name, o.Category.IsIncome })
                    .Select(g => new 
                    { 
                        Category = g.Key.Name, 
                        Total = g.Key.IsIncome
                            ? g.Sum(o => o.Ammount) 
                            : g.Sum(o => o.Ammount) * -1
                    })
                    .OrderByDescending(x => x.Total)
                );

            var messageTextBuilder = new StringBuilder()
                .AppendFormat("Please see your operations report between {0:MMM dd, yyyy} and {1:MMM dd, yyyy}:", from, to)
                .AppendLine()
                .AppendFormat("Balance: {0}", operations.Sum(x => x.Total))
                .AppendLine()
                .AppendLine()
                .AppendFormat("Total income: {0}", operations.Where(x => x.Total > 0).Sum(x => x.Total))
                .AppendLine()
                .AppendFormat("Total expense: {0}", operations.Where(x => x.Total < 0).Sum(x => x.Total))
                .AppendLine();

            foreach (var operation in operations)
            {
                messageTextBuilder
                    .AppendLine()
                    .AppendFormat("{0}: {1}", operation.Category, operation.Total);
            }

            await _messageSender.SendAsync(new BotMessage(messageTextBuilder.ToString()));
        }

        private async Task<DateTime?> GetStartDateAsync()
        {
            if (ArgumentProvider.TryGetDateTime(0, out DateTime parsedValue))
            {
                return parsedValue.Date;
            }

            State[StateKeys.CommandAwaitingArguments] = CommandNames.GetOperationsReport;

            var now = DateTime.UtcNow;
            var firstDayOfCurrentMonth = new DateTime(now.Year, now.Month, 1);

            await _messageSender.SendAsync(new BotMessage("Starting from what date you want me to include operations?", 
                firstDayOfCurrentMonth.ToString(DateFormat), 
                firstDayOfCurrentMonth.AddMonths(-1).ToString(DateFormat), 
                firstDayOfCurrentMonth.AddMonths(-2).ToString(DateFormat),
                firstDayOfCurrentMonth.AddMonths(-3).ToString(DateFormat),
                firstDayOfCurrentMonth.AddMonths(-4).ToString(DateFormat),
                firstDayOfCurrentMonth.AddMonths(-5).ToString(DateFormat)));
            
            return null;
        }

        private async Task<DateTime?> GetEndDateAsync(DateTime startDate)
        {
            string text;
            if (!ArgumentProvider.TryGetDateTime(1, out DateTime parsedValue))
            {
                text = $"You want me to include operation from {startDate.ToString(DateFormat)} till when?";
            }
            else if (parsedValue < startDate)
            {
                text = $"The date must be later than {startDate.ToString(DateFormat)}.";
            }
            else
            {
                return parsedValue.Date.AddDays(1).AddSeconds(-1);
            }

            State[StateKeys.CommandAwaitingArguments] = CommandNames.GetOperationsReport;

            var now = DateTime.UtcNow;
            var firstDayOfNextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);

            await _messageSender.SendAsync(new BotMessage(
                text,
                firstDayOfNextMonth.AddDays(-1).ToString(DateFormat), 
                firstDayOfNextMonth.AddMonths(-1).AddDays(-1).ToString(DateFormat),
                firstDayOfNextMonth.AddMonths(-2).AddDays(-1).ToString(DateFormat),
                firstDayOfNextMonth.AddMonths(-3).AddDays(-1).ToString(DateFormat),
                firstDayOfNextMonth.AddMonths(-4).AddDays(-1).ToString(DateFormat),
                firstDayOfNextMonth.AddMonths(-5).AddDays(-1).ToString(DateFormat)));
            
            return null;
        }
    }
}
