using Azure;
using Azure.Data.Tables;

namespace Finance.Bot.Data.Models
{
    public class StateEntity : ITableEntity
    {
        public string AppName
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public long ChatId
        {
            get => long.Parse(RowKey);
            set => RowKey = value.ToString();
        }

        public Dictionary<string, string> Data { get; } = new();
        public ETag ETag { get; set; }
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = "0";
        public DateTimeOffset? Timestamp { get; set; }
        public string Type { get; } = string.Empty;
    }
}
