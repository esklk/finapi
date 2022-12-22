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
            CreateMap<StateEntity, State>().ConvertUsing<StateEntityStateConverter>();
            CreateMap<State, StateEntity>().ConvertUsing<StateEntityStateConverter>();
        }
    }
}
