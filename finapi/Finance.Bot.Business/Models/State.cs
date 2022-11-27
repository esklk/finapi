namespace Finance.Bot.Business.Models
{
    public class State
    {
        public Dictionary<string, string> Data { get; } = new();
        public string Type { get; } = string.Empty;

    }
}
