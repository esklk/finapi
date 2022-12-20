namespace Finance.Bot.Business.Models
{
    public class State
    {
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

        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();

        public bool ContainsKey(string key) => Data.ContainsKey(key);

        public bool TryGetValue(string key, out string value)
        {
            return Data.TryGetValue(key, out value);
        }

        public bool TryGetNumber(string key, out int value)
        {
            value = 0;
            return TryGetValue(key, out var stringValue) && int.TryParse(stringValue, out value);
        }

    }
}
