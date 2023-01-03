using Finance.Bot.Business.Commands;
using Finance.Bot.Business.Constants;
using Finance.Bot.Business.Models;
using Finance.Core.Practices;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StateManagingMessageProcessor : IMessageProcessor
    {
        private const char CommandNameArgumentsSeparator = ' ';
        private const char ArgumentsSeparator = ',';

        private readonly IStateService _stateService;
        private readonly IFactory<IBotCommand, string> _commandFactory;

        public StateManagingMessageProcessor(IStateService stateService, IFactory<IBotCommand, string> commandFactory)
        {
            _stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        public async Task ProcessAsync(string? text)
        {
            State? state = await _stateService.GetStateAsync();
            if (state == null)
            {
                throw new InvalidOperationException("Cannot retrieve state to process message.");
            }

            if (!TryGetCommandAndArguments(state, text, out string command, out string[] arguments))
            {
                return;
            }


            await _commandFactory.Create(command).ExecuteAsync(state, arguments);

            await _stateService.SetStateAsync(state);
        }

        private static bool TryGetCommandAndArguments(State state, string? text, out string command, out string[] arguments)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                command = string.Empty;
                arguments = Array.Empty<string>();
                return false;
            }

            if (text.StartsWith('/'))
            {
                int commandNameLength = text.Contains(CommandNameArgumentsSeparator)
                    ? text.IndexOf(CommandNameArgumentsSeparator)
                    : text.Length;

                state[StateKeys.CommandAwaitingArguments] = null;
                command = text.Substring(0, commandNameLength);
                arguments = text.Length > commandNameLength
                    ? text.Remove(0, commandNameLength).Split(ArgumentsSeparator)
                    : Array.Empty<string>();
                return true;
            }

            if (state.TryGetString(StateKeys.CommandAwaitingArguments, out command!))
            {
                arguments = new[] { text };
                return true;
            }

            arguments = Array.Empty<string>();
            return false;
        }
    }
}
