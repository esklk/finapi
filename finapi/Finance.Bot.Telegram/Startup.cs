using Finance.Bot.Telegram.Configuration.Implementation;
using Finance.Bot.Telegram.Services.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Finance.Bot.Telegram
{
    public class Startup
    {
        private readonly TelegramBotConfiguration botConfiguration;

        public Startup(IConfiguration configuration)
        {
            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            botConfiguration = configuration.Get<TelegramBotConfiguration>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(botConfiguration);

            services.AddHostedService<TelegramBotHostedService>();

            services.AddHttpClient("tgwebhook")
                    .AddTypedClient<ITelegramBotClient>(httpClient
                        => new TelegramBotClient(botConfiguration.Token, httpClient));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting()
                .UseCors()
                .UseEndpoints(endpoints => endpoints.MapPost(botConfiguration.WebhookUrl.PathAndQuery, OnUpdate));
        }

        private static async Task OnUpdate(HttpContext context)
        {
            await context.Response.WriteAsync("Hello World!");
        }
    }
}
