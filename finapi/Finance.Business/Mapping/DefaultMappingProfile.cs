using AutoMapper;
using Finance.Business.Models;
using Finance.Data.Models;
using System.Linq;

namespace Finance.Business.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<Account, AccountModel>();
            CreateMap<OperationCategory, OperationCategoryModel>();
        }
    }
}
