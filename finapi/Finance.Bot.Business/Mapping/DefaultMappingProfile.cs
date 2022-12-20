using AutoMapper;
using Finance.Bot.Business.Mapping.Converters;
using Finance.Bot.Business.Models;
using Finance.Bot.Data.Models;

namespace Finance.Bot.Business.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<string, Dictionary<string, string>>().ConvertUsing<JsonDictionaryConverter>();
            CreateMap<Dictionary<string, string>, string>().ConvertUsing<JsonDictionaryConverter>();

            CreateMap<StateEntity, State>()
                .ForMember(d => d.Data, m => m.MapFrom(s => s.DataDictionary))
                .ReverseMap();
        }
    }
}
