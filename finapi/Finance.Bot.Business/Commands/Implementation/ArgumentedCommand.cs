using Finance.Bot.Business.Models;
using Finance.Bot.Business.Services;

namespace Finance.Bot.Business.Commands.Implementation
{
    public abstract class ArgumentedCommand : IBotCommand
    {
        private readonly IArgumentProviderBuilder _argumentProviderBuilder;
        
        private IArgumentProvider? _argumentProvider;
        private bool _isInitialized;
        private State? _state;

        protected ArgumentedCommand(int expectedArgumentCount, IArgumentProviderBuilder argumentProviderBuilder)
        {
            _argumentProviderBuilder = argumentProviderBuilder?
                                           .SetConsumerType(GetType())
                                           .SetExpectedArgumentsCount(expectedArgumentCount)
                                       ?? throw new ArgumentNullException(nameof(argumentProviderBuilder));
        }

        protected IArgumentProvider ArgumentProvider
        {
            get
            {
                if (_argumentProvider != null)
                {
                    return _argumentProvider;
                }

                if (!_isInitialized)
                {
                    throw new InvalidOperationException(
                        $"The Argument Provider is not initialized yet. Make sure {nameof(ArgumentedCommand)}.{nameof(Initialize)} method called before you are using it.");
                }

                _argumentProvider = _argumentProviderBuilder.Build();
                return _argumentProvider;
            }
        }

        protected State State
        {
            get
            {
                if (!_isInitialized || _state == null)
                {
                    throw new InvalidOperationException(
                        $"The State is not initialized yet. Make sure {nameof(ArgumentedCommand)}.{nameof(Initialize)} method called before you are using it.");
                }

                return _state;
            }
        }

        public async Task ExecuteAsync(State state, string[] arguments)
        {
            Initialize(state, arguments);

            await ExecuteInternalAsync();
        }

        protected void Initialize(State state, string[] arguments)
        {
            _state = state;
            _argumentProviderBuilder
                .AddSource(state)
                .AddSource(arguments);
            _isInitialized = true;
        }

        protected abstract Task ExecuteInternalAsync();
    }
}
