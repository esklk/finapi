using Finance.Bot.Business.Commands.Implementation;
using Finance.Bot.Business.Commands;
using Finance.Bot.Business.Services.Implementation;
using Finance.Bot.Business.Services;
using Finance.Core.Practices;
using Finance.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Bot.Business.Mapping;

namespace Finance.Bot.Business
{
    public static class Bootstrapper
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services
                .AddScoped<StateManagingMessageProcessor>()
                .AddAutoMapper(typeof(DefaultMappingProfile))
                .AddScoped<IFactory<IStateService, string>, StateServiceFactory>(c =>
                    new StateServiceFactory(c.GetRequiredService<IServiceProvider>()))
                .AddScoped<IFactory<IBotCommand, string>, BotCommandFactory>()
                .AddScoped<IArgumentProviderBuilder, CommandArgumentProviderBuilder>()
                .AddScoped<CancelCommand>()
                .AddScoped<CreateAccount>()
                .AddScoped<CreateOperationCategory>()
                .AddScoped<DeleteAccount>()
                .AddScoped<GetOperationsReport>()
                .AddScoped<Help>()
                .AddScoped<ReportOperation>()
                .AddScoped<SelectAccount>();
        }
    }
}
