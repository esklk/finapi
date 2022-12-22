using System.Diagnostics.CodeAnalysis;
using Finance.Bot.Business.Constants;
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

        public bool TryGetNumber(int index, out int value)
        {
            return TryGetArgumentWithCleanup(index, out value);
        }

        public bool TryGetBool(int index, out bool value)
        {
            return TryGetArgumentWithCleanup(index, out value);
        }

        private bool TryGetArgumentWithCleanup<T>(int index, [MaybeNullWhen(false)] out T value)
        {
            if (!TryGetArgument(index, out value))
            {
                return false;
            }

            if (index != _lastArgumentIndex)
            {
                return true;
            }

            // Remove saved parameters when the last parameter acquired successfully
            foreach (string key in _state.Keys.Where(x=>x.StartsWith(_stateKeyPrefix)))
            {
                _state[key] = null;
            }

            return true;
        }

        private bool TryGetArgument<T>(int index, [MaybeNullWhen(false)] out T value)
        {
            string stateKey = $"{_stateKeyPrefix}_arg_{index}";

            // When missing parameters requested
            // 1. Try get from state -> might be set during previous requests
            // 2. Try get from arguments at 0 position
            if (_state.ContainsKey(StateKeys.CommandAwaitingArguments))
            {
                // Skip for last argument, because last parameter always acquired from arguments
                if (index < _lastArgumentIndex && TryGetFromState(stateKey, out value))
                {
                    return true;
                }

                // Requested one by one parameters always come as the first argument
                if (TryGetFromArguments(0, out value))
                {
                    _state[stateKey] = value;
                    return true;
                }
            }
            // When command came along with parameters
            // Try get from arguments at its expected position
            else if (TryGetFromArguments(index, out value))
            {
                _state[stateKey] = value;
                return true;
            }

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
