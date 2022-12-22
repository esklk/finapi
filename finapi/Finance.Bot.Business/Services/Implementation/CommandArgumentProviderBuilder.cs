using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services.Implementation
{
    public class CommandArgumentProviderBuilder : IArgumentProviderBuilder
    {
        private string[]? _arguments;
        private int _expectedArgumentsCount = 1;
        private State? _state;
        private string? _stateKeyPrefix;

        public IArgumentProviderBuilder AddSource(string[] source)
        {
            _arguments ??= source;

            return this;
        }

        public IArgumentProviderBuilder AddSource(State state)
        {
            _state ??= state;

            return this;
        }

        public IArgumentProvider Build()
        {
            return new CommandArgumentProvider(_state, _stateKeyPrefix, _arguments, _expectedArgumentsCount);
        }

        public IArgumentProviderBuilder SetConsumerType(Type type)
        {
            _stateKeyPrefix = type.Name;

            return this;
        }

        public IArgumentProviderBuilder SetExpectedArgumentsCount(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _expectedArgumentsCount = value;

            return this;
        }
    }
}
