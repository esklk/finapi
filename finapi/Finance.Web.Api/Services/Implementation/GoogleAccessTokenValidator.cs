using Finance.Business.Exceptions;
using Finance.Web.Api.Configuration.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TokenValidationResult = Finance.Web.Api.Models.TokenValidationResult;

namespace Finance.Web.Api.Services.Implementation
{
    public class GoogleAccessTokenValidator : ITokenValidator
    {
        public const string AccessTokenUrl = "https://oauth2.googleapis.com/token";
        private readonly IHttpClientFactory _clientFactory;
        private readonly OAuthConfiguration _oAuthConfiguration;
        private readonly IJwtTokenManager _jwtTokenManager;

        public GoogleAccessTokenValidator(IHttpClientFactory clientFactory, IJwtTokenManager jwtTokenManager, IOauthConfigurationProvider oauthConfigurationProvider)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _jwtTokenManager = jwtTokenManager ?? throw new ArgumentNullException(nameof(jwtTokenManager));
            _oAuthConfiguration = oauthConfigurationProvider?.GetRelated(GetType()) ?? throw new ArgumentNullException(nameof(oauthConfigurationProvider));
        }

        public async Task<TokenValidationResult> ValidateAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullOrWhitespaceStringException(nameof(token));
            }

            var message = new HttpRequestMessage(HttpMethod.Post, AccessTokenUrl);
            message.Content = JsonContent.Create(new
            {
                grant_type = "authorization_code",
                code = token,
                client_secret = _oAuthConfiguration.Secret,
                client_id = _oAuthConfiguration.Parameters.GetValueOrDefault("client_id") ?? throw new InvalidOperationException("Can not find client_id in OAuth configuration parameters."),
                redirect_uri = _oAuthConfiguration.Parameters.GetValueOrDefault("redirect_uri") ?? throw new InvalidOperationException("Can not find redirect_uri in OAuth configuration parameters.")
            });

            HttpResponseMessage response = await _clientFactory.CreateClient().SendAsync(message);
            
            Stream content = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(content);
            if (!response.IsSuccessStatusCode)
            {
                return new TokenValidationResult(data.TryGetValue("error_description", out var error) 
                    ? error.ToString() 
                    : response.ReasonPhrase);
            }

            var idToken = data["id_token"].ToString();
            return new TokenValidationResult(_jwtTokenManager.GetPayload(idToken));
        }
    }
}
