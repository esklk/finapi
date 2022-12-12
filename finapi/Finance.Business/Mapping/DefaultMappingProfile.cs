using AutoMapper;
using Finance.Business.Models;
using Finance.Data.Models;

namespace Finance.Business.Mapping
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<UserLogin, UserLoginModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Account, AccountModel>();
            CreateMap<OperationCategory, OperationCategoryModel>();
            CreateMap<Operation, OperationModel>();
        }
    }
}
