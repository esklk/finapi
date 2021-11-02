using Finance.Business.Exceptions;
using Finance.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services.Implementation
{
    public class GoogleAccessTokenValidator : ITokenValidator
    {
        public const string UrlTemplate = "https://www.googleapis.com/oauth2/v3/userinfo?access_token={0}";
        private readonly IHttpClientFactory _clientFactory;

        public GoogleAccessTokenValidator(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<TokenValidationResult> ValidateAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(token));
            }

            var message = new HttpRequestMessage(HttpMethod.Get, string.Format(UrlTemplate, token));
            
            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return new TokenValidationResult(response.ReasonPhrase);
            }

            Stream content = await response.Content.ReadAsStreamAsync();
            var payload = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(content);

            return new TokenValidationResult(payload.ToDictionary(x => x.Key, x => x.Value.ToString(), StringComparer.OrdinalIgnoreCase));
        }
    }
}
