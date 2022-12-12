using AutoMapper;
using Newtonsoft.Json;

namespace Finance.Bot.Business.Mapping.Converters
{
    public class JsonDictionaryConverter : ITypeConverter<string, Dictionary<string, string>>, ITypeConverter<Dictionary<string, string>, string>
    {
        public Dictionary<string, string> Convert(string source, Dictionary<string, string> destination, ResolutionContext context)
        {
            return string.IsNullOrWhiteSpace(source)
                ? new Dictionary<string, string>()
                : JsonConvert.DeserializeObject<Dictionary<string, string>>(source);
        }

        public string Convert(Dictionary<string, string> source, string destination, ResolutionContext context)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}
