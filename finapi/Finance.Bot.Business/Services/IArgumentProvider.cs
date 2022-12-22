using System.Diagnostics.CodeAnalysis;

namespace Finance.Bot.Business.Services
{
    public interface IArgumentProvider
    {
        public bool TryGetString(int index, [MaybeNullWhen(false)] out string value);
        public bool TryGetNumber(int index, out int value);
        public bool TryGetBool(int index, out bool value);
    }
}
