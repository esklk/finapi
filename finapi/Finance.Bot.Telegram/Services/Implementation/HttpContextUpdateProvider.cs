using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using Telegram.Bot.Types;

namespace Finance.Bot.Telegram.Services.Implementation
{
    public class HttpContextUpdateProvider : IUpdateProvider
    {
        private readonly Lazy<Update> _update;

        public HttpContextUpdateProvider(IHttpContextAccessor httpContextAccessor)
        {
            if(httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _update = new Lazy<Update>(() => GetUpdate(httpContextAccessor.HttpContext.Request));
        }

        public Update Update => _update.Value;

        private static Update GetUpdate(HttpRequest request)
        {
            string body = request.ReadAsStringAsync().GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new InvalidOperationException("Body is missing in the request.");
            }

            var update = JsonConvert.DeserializeObject<Update>(body);
            if(update == null)
            {
                throw new InvalidOperationException("Body does not represent a valid update.");
            }

            return update;
        }
    }
}
