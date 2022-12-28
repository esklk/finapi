using System.Diagnostics.CodeAnalysis;
using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services.Implementation
{
    public class CommandArgumentProvider : IArgumentProvider
    {
        private readonly State _state;
        private readonly string _stateKeyPrefix;
        private readonly string[] _arguments;
        private readonly int _lastArgumentIndex;

        public CommandArgumentProvider(State state, string stateKeyPrefix, string[] arguments, int expectedArgumentsCount)
        {
            if (expectedArgumentsCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedArgumentsCount), "Value cannot be less then 1.");
            }

            if (string.IsNullOrWhiteSpace(stateKeyPrefix))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(stateKeyPrefix));
            }

            _state = state ?? throw new ArgumentNullException(nameof(state));
            _stateKeyPrefix = stateKeyPrefix;
            _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            _lastArgumentIndex = expectedArgumentsCount - 1;
        }

        public bool TryGetString(int index, [MaybeNullWhen(false)] out string value)
        {
            return TryGetArgumentWithCleanup(index, out value);
        }

        public bool TryGetInteger(int index, out int value)
        {
            return TryGetArgumentWithCleanup(index, out value);
        }

        public bool TryGetBool(int index, out bool value)
        {
            return TryGetArgumentWithCleanup(index, out value);
        }

        public bool TryGetDouble(int index, out double value)
        {
            return TryGetArgumentWithCleanup(index, out value);
        }

        public bool TryGetDateTime(int index, out DateTime value)
        {
            return TryGetArgumentWithCleanup(index, out value);

        }

        private string AwaitedArgumentNameStateKey => $"{_stateKeyPrefix}_AwaitedArgumentName";

        private bool TryGetArgumentWithCleanup<T>(int index, [MaybeNullWhen(false)] out T value)
        {
            if (!TryGetArgument(index, out value))
            {
                return false;
            }

            CleanupIfNeeded(index);
            return true;
        }

        private void CleanupIfNeeded(int index)
        {
            if (index != _lastArgumentIndex)
            {
                return;
            }

            // Remove saved parameters when the last parameter acquired successfully
            foreach (string key in _state.Keys.Where(x => x.StartsWith(_stateKeyPrefix)))
            {
                _state[key] = null;
            }
        }

        private bool TryGetArgument<T>(int index, [MaybeNullWhen(false)] out T value)
        {
            string requestedArgumentName = $"{_stateKeyPrefix}_arg_{index}";

            // 1. Try get from 0 position for the case if awaited
            if (_state.TryGetString(AwaitedArgumentNameStateKey, out string? awaitedArgumentName)
                && awaitedArgumentName == requestedArgumentName
                && TryGetFromArguments(0, out value))
            {
                _state[AwaitedArgumentNameStateKey] = null;
                _state[requestedArgumentName] = value;
                return true;
            }

            // 2. Try get from state for the case it was provided with previous requests, but not with the current one
            // last argument will never be state since state is cleaned up after last argument successfully received
            if (index < _lastArgumentIndex && TryGetFromState(requestedArgumentName, out value))
            {
                return true;
            }

            // 3. Try get from expected position for the case arguments are passed along with command
            if (TryGetFromArguments(index, out value))
            {
                _state[requestedArgumentName] = value;
                return true;
            }

            _state[AwaitedArgumentNameStateKey] = requestedArgumentName;

            value = default;
            return false;
        }

        private bool TryGetFromState<T>(string key, [MaybeNullWhen(false)] out T value)
        {
            if (_state.TryGetString(key, out string? stringValue))
            {
                return TryConvert(stringValue, out value);
            }

            value = default;
            return false;

        }

        private bool TryGetFromArguments<T>(int index, [MaybeNullWhen(false)] out T value)
        {
            return TryConvert(_arguments.ElementAtOrDefault(index), out value);
        }

        private static bool TryConvert<T>(string? element, [MaybeNullWhen(false)] out T value)
        {
            if (string.IsNullOrWhiteSpace(element))
            {
                value = default;
                return false;
            }

            try
            {
                value = (T)Convert.ChangeType(element, typeof(T));
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }
    }
}
