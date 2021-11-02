using Finance.Business.Models;
using Finance.Business.Services;
using Finance.Web.Api.Models;
using Finance.Web.Api.Services.Tokens.PayloadMapping;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IPayloadMapperFactory _payloadMapperFactory;
        private readonly ITokenValidatorFactory _tokenValidatorFactory;
        private readonly IUserService _userService;

        public AuthenticationService(IAccessTokenGenerator accessTokenGenerator, IPayloadMapperFactory payloadMapperFactory, ITokenValidatorFactory tokenValidatorFactory, IUserService userService)
        {
            _accessTokenGenerator = accessTokenGenerator ?? throw new ArgumentNullException(nameof(accessTokenGenerator));
            _payloadMapperFactory = payloadMapperFactory ?? throw new ArgumentNullException(nameof(payloadMapperFactory));
            _tokenValidatorFactory = tokenValidatorFactory ?? throw new ArgumentNullException(nameof(tokenValidatorFactory));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<AuthModel> AuthenticateAsync(string token, string provider)
        {
            TokenValidationResult validationResult = await _tokenValidatorFactory.Create(provider).ValidateAsync(token);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException($"Token validation failed: {validationResult.Error}", nameof(token));
            }

            LoginPayloadModel payload = _payloadMapperFactory.Create(provider).Map<LoginPayloadModel>(validationResult.Payload);
            UserModel user = await _userService.GetUserAsync(provider, payload.LoginIdentifier);
            if (user == null)
            {
                user = await _userService.CreateUserAsync(payload.Name, provider, payload.LoginIdentifier);
            }

            string accessToken = _accessTokenGenerator.Generate(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            });

            return new AuthModel { AccessToken = accessToken };
        }
    }
}
