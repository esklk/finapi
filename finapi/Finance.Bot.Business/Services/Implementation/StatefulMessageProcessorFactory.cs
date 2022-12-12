using System.Reflection;
using Finance.Bot.Business.Models;
using Finance.Core.Practices;

namespace Finance.Bot.Business.Services.Implementation
{
    public class StatefulMessageProcessorFactory : IFactory<IMessageProcessor, State>
    {
        public IMessageProcessor Create(State state)
        {
            if (string.IsNullOrWhiteSpace(state.ProcessorType))
            {
                throw new ArgumentException("Processor type cannot be null or whitespace.", nameof(state));
            }

            Type processorType = Type.GetType(state.ProcessorType) ??
                                 throw new ArgumentException(
                                     $"Could not find processor type {state.ProcessorType}.", nameof(state));
            if (!processorType.IsAssignableTo(typeof(IMessageProcessor)))
            {
                throw new ArgumentException($"Processor type does not implement {nameof(IMessageProcessor)} interface.",
                    nameof(processorType));
            }

            foreach (ConstructorInfo constructor in processorType.GetConstructors())
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(State))
                {
                    return (IMessageProcessor)constructor.Invoke(new object?[] { state });
                }
            }

            throw new ArgumentException(
                $"Processor type does not contain a constructor accepting single parameter of type {nameof(State)}.");
        }
    }
}
