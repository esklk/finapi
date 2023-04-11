using Finance.Business.Services.Implementation;
using Finance.Business.Services;
using Finance.Business.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Business
{
    public static class Bootstrapper
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(DefaultMappingProfile))
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IOperationCategoryService, OperationCategoryService>()
                .AddScoped<IOperationService, OperationService>()
                .AddScoped<IUserLoginService, UserLoginService>()
                .AddScoped<IUserService, UserService>();
        }
    }
}
