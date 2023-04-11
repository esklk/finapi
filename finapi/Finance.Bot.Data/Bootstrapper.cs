using Finance.Bot.Data.Models;
using Finance.Bot.Data.Services.Implementation;
using Finance.Core.Extensions;
using Finance.Core.Practices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Bot.Data
{
    public static class Bootstrapper
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services
                .AddScoped<IRepository<StateEntity, string>, AzureTableEntityRepository<StateEntity>>(x =>
                    new AzureTableEntityRepository<StateEntity>(
                        configuration.GetRequiredValue<string>("StateStorage:ConnectionString"),
                        configuration.GetRequiredValue<string>("StateStorage:PartitionKey")));
        }
    }
}
