using Finance.Bot.Telegram;
using Functions.Worker.ContextAccessor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddEnvironmentVariables("FINAPI_"))
    .ConfigureFunctionsWorkerDefaults(applicationBuilder => applicationBuilder.UseFunctionContextAccessor())
    .ConfigureServices(Startup.Configure)
    .UseDefaultServiceProvider((_, options) =>
    {
        options.ValidateScopes = true;
        options.ValidateOnBuild = true;
    })
    .Build();

host.Run();
