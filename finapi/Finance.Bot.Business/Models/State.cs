using Finance.Bot.Business.Services;

namespace Finance.Bot.Business.Models
{
    public class State
    {
        public State()
        {
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
            ProcessorType = processorType.AssemblyQualifiedName;
        }

        public Dictionary<string, string> Data { get; set; }
        public string ProcessorType { get; set; }

    }
}
