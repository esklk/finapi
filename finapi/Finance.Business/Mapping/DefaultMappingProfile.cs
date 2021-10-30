using AutoMapper;
using Finance.Business.Models;
using Finance.Data.Models;

namespace Finance.Business.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<User, UserModel>();
        }
    }
}
