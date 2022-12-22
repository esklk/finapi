using System.Diagnostics.CodeAnalysis;

namespace Finance.Bot.Business.Models
{
    public class State
    {
        public State(Dictionary<string, string>? data = null)
        {
            Data = data ?? new Dictionary<string, string>();
        }

        public object? this[string key]
        {
            set
            {
                var stringValue = value?.ToString();

                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    Data.Remove(key);
                }
                else
                {
                    Data[key] = stringValue;
                }
            }
        }

        public IReadOnlyCollection<string> Keys => Data.Keys;

        public Dictionary<string, string> Data { get; }

        public void Clear() => Data.Clear();

        public bool ContainsKey(string key) => Data.ContainsKey(key);

        public bool TryGetBool(string key, out bool value)
        {
            if (Data.TryGetValue(key, out var stringValue))
            {
                return bool.TryParse(stringValue, out value);
            }
            value = false;
            return false;
        }

        public bool TryGetNumber(string key, out int value)
        {
            if (Data.TryGetValue(key, out var stringValue))
            {
                return int.TryParse(stringValue, out value);
            }
            value = 0;
            return false;
        }

        public bool TryGetString(string key, [MaybeNullWhen(false)] out string value)
        {
            return Data.TryGetValue(key, out value);
        }
    }
}