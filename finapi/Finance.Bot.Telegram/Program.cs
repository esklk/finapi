using Finance.Bot.Telegram;
using Functions.Worker.ContextAccessor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(configurationBuilder => configurationBuilder
        .AddEnvironmentVariables("FINAPI_")
        .AddJsonFile("local.settings.json", true))
    .ConfigureFunctionsWorkerDefaults(applicationBuilder => applicationBuilder.UseFunctionContextAccessor())
    .ConfigureServices(Startup.ConfigureServices)
    .UseDefaultServiceProvider((_, options) =>
    {
        options.ValidateScopes = true;
        options.ValidateOnBuild = true;
    })
    .Build();

host.Run();
