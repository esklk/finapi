namespace Finance.Bot.Business.Models
{
    public class State
    {
        public State()
        {
            if (Data == null)
            {
                Data = new Dictionary<string, string>();
            }
        }

        public State(Type processorType, Dictionary<string, string>? data = null)
        {
            if (processorType == null)
            {
                throw new ArgumentNullException(nameof(processorType));
            }

            if (string.IsNullOrWhiteSpace(processorType.AssemblyQualifiedName))
            {
                throw new ArgumentException(
                    $"Value's ${nameof(processorType.AssemblyQualifiedName)} member cannot be null or whitespace.",
                    nameof(processorType));
            }

            Data = data ?? new Dictionary<string, string>();
            ProcessorType = processorType;
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

        public Dictionary<string, string> Data { get; set; }

        public Type ProcessorType { get; set; }

        public bool ContainsKey(string key) => Data.ContainsKey(key);

        public bool TryGetValue(string key, out string? value)
        {
            return Data.TryGetValue(key, out value);
        }

    }
}
