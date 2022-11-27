using AutoMapper;
using Finance.Bot.Business.Models;
using Finance.Bot.Data.Models;

namespace Finance.Bot.Business.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<State, StateEntity>().ReverseMap();
        }
    }
}
