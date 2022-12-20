using Finance.Bot.Telegram.Services;
using Finance.Core.Practices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Functions
{
    public class UpdateHandling
    {
        private readonly IFactory<IUpdateService, Update> _updateServiceFactory;
        private readonly IUpdateProvider _updateProvider;
        private readonly ILogger<UpdateHandling> _logger;

        public UpdateHandling(IFactory<IUpdateService, Update> updateServiceFactory, IUpdateProvider updateProvider, ILoggerFactory loggerFactory)
        {
            _updateServiceFactory = updateServiceFactory ?? throw new ArgumentNullException(nameof(updateServiceFactory));
            _updateProvider = updateProvider ?? throw new ArgumentNullException(nameof(updateProvider));
            _logger = loggerFactory.CreateLogger<UpdateHandling>();
        }

        [Function(FunctionNames.Update)]
        public async Task UpdateAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                await _updateServiceFactory.Create(_updateProvider.Update).HandleAsync(_updateProvider.Update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update handling failed.\n{await req.ReadAsStringAsync()}");
            }
        }
    }
}
