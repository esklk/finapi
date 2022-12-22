using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services
{
    public interface IArgumentProviderBuilder
    {
        IArgumentProviderBuilder AddSource(string[] source);
        IArgumentProviderBuilder AddSource(State source);
        IArgumentProvider Build();
        IArgumentProviderBuilder SetConsumerType(Type type);
        IArgumentProviderBuilder SetExpectedArgumentsCount(int value);
    }
}
