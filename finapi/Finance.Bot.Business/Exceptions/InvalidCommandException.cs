namespace Finance.Bot.Business.Exceptions
{
    public class InvalidCommandException : ArgumentException
    {
        public InvalidCommandException(string? command):base($"The command \"{command}\" is invalid for current state.")
        {
            Command = command;
        }

        public string? Command { get; }
    }
}
