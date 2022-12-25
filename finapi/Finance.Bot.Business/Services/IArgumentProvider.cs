using System.Diagnostics.CodeAnalysis;

namespace Finance.Bot.Business.Services
{
    public interface IArgumentProvider
    {
        public bool TryGetString(int index, [MaybeNullWhen(false)] out string value);
        public bool TryGetInteger(int index, out int value);
        public bool TryGetBool(int index, out bool value);
        public bool TryGetDouble(int index, out double value);
        public bool TryGetDateTime(int index, out DateTime value);
    }
}
