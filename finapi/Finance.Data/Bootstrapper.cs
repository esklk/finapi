using Finance.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Data
{
    public static class Bootstrapper
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            if (bool.TryParse(configuration.GetValue<string>("DatabaseConfiguration:UseInMemory"), out var useInMemory) && useInMemory)
            {
                services.AddDbContext<FinApiDbContext>(opt => opt.UseInMemoryDatabase("Finance"));
            }
            else
            {
                services.AddDbContext<FinApiDbContext>(opt =>
                    opt.UseSqlServer(
                        configuration.GetRequiredValue<string>("DatabaseConfiguration:ConnectionString"),
                        options => options.EnableRetryOnFailure()));
            }
        }
    }
}
