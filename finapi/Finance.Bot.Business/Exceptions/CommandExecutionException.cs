namespace Finance.Bot.Business.Exceptions
{
    public class CommandExecutionException : Exception
    {
        private readonly string? _retryCommand;

        public CommandExecutionException(string message, string? retryCommand = null) : base(message)
        {
            _retryCommand = retryCommand;
        }

        public CommandExecutionException(Exception ex, string message, string? retryCommand = null) : base(message, ex)
        {
            _retryCommand = retryCommand;
        }

        public bool RetryAvailable => !string.IsNullOrWhiteSpace(_retryCommand);

        public string? RetryCommand => _retryCommand;
    }
}
