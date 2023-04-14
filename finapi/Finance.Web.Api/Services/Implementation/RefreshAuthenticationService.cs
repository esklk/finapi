using System;
using System.Threading.Tasks;
using Finance.Web.Api.Exceptions;
using Finance.Web.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace Finance.Web.Api.Services.Implementation
{
    public class RefreshAuthenticationService : IAuthenticationService<AuthModel>
    {
        private readonly ITokenGenerator _accessTokenGenerator;
        private readonly ITokenValidator _accessTokenValidator;
        private readonly ITokenValidator _refreshTokenValidator;

        public RefreshAuthenticationService(ITokenGenerator accessTokenGenerator, ITokenValidator accessTokenValidator,
            ITokenValidator refreshTokenValidator)
        {
            _accessTokenGenerator =
                accessTokenGenerator ?? throw new ArgumentNullException(nameof(accessTokenGenerator));
            _accessTokenValidator =
                accessTokenValidator ?? throw new ArgumentNullException(nameof(accessTokenValidator));
            _refreshTokenValidator =
                refreshTokenValidator ?? throw new ArgumentNullException(nameof(refreshTokenValidator));
        }

        public Task<AuthModel> AuthenticateAsync(AuthModel data)
        {
            try
            {
                if (IsAccessTokenExpired(data.AccessToken))
                {
                    var claims = _refreshTokenValidator.Validate(data.RefreshToken);
                    var freshData = data with { AccessToken = _accessTokenGenerator.Generate(claims) };

                    return new ValueTask<AuthModel>(freshData).AsTask();
                }
            }
            catch (SecurityTokenValidationException ex)
            {
                throw new AuthenticationFailedException(ex);
            }

            return new ValueTask<AuthModel>(data).AsTask();
        }

        private bool IsAccessTokenExpired(string token)
        {
            try
            {
                _accessTokenValidator.Validate(token);
                return false;
            }
            catch (SecurityTokenExpiredException)
            {
                return true;
            }
        }
    }
}
