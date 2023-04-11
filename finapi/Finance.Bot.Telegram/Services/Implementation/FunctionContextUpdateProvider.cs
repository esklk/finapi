using Functions.Worker.ContextAccessor;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class FunctionContextUpdateProvider : IUpdateProvider
    {
        private readonly Lazy<Update> _update;

        public FunctionContextUpdateProvider(IFunctionContextAccessor functionContextAccessor)
        {
            if (functionContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(functionContextAccessor));
            }

            _update = new Lazy<Update>(() =>
                GetUpdateAsync(functionContextAccessor.FunctionContext).GetAwaiter().GetResult());
        }

        public Update Update => _update.Value;

        private static async Task<Update> GetUpdateAsync(FunctionContext functionContext)
        {
            var requestData = await functionContext.GetHttpRequestDataAsync();
            if (requestData == null)
            {
                throw new InvalidOperationException("Cannot retrieve HTTP request data.");
            }

            string? updateString = await requestData.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(updateString))
            {
                throw new InvalidOperationException("Body is empty.");
            }

            var update = JsonConvert.DeserializeObject<Update>(updateString);
            if (update == null)
            {
                throw new InvalidOperationException("Body does not represent a valid update.");
            }

            return update;
        }
    }
}
