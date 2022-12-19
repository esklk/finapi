namespace Finance.Bot.Business.Models
{
    public class MessageResponse
    {
        public MessageResponse(string text)
        {
            Text = text;
            TaggedOptions = Array.Empty<KeyValuePair<string, string>>();
            Options = Array.Empty<string>();
        }

        public MessageResponse(string text, params string[] options)
        {
            Text = text;
            Options = options;
            TaggedOptions = Array.Empty<KeyValuePair<string, string>>();
        }
        public MessageResponse(string text, params KeyValuePair<string, string>[] taggedOptions)
        {
            Text = text;
            TaggedOptions = taggedOptions;
            Options = Array.Empty<string>();
        }

        public string Text { get; set; }

        public string[] Options { get; set; }

        public KeyValuePair<string, string>[] TaggedOptions { get; set; }
    }
}
