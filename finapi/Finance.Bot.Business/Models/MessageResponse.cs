namespace Finance.Bot.Business.Models
{
    public class MessageResponse
    {
        public MessageResponse(string text, params string[] options)
        {
            Text = text;
            Options = options;
        }
        public MessageResponse(string text, params KeyValuePair<string, string>[] taggedOptions)
        {
            Text = text;
            TaggedOptions = taggedOptions;
        }

        public string? Text { get; set; }

        public string[]? Options { get; set; }

        public KeyValuePair<string, string>[]? TaggedOptions { get; set; }
    }
}
