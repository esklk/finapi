using System;
using System.Threading.Tasks;
using Finance.Web.Api.Exceptions;
using Finance.Web.Api.Models;
using Google.Apis.Auth;

namespace Finance.Web.Api.Services.Implementation
{
    public class GoogleAuthenticationService : IAuthenticationService<GoogleAuthenticationData>
    {
        private readonly IAuthenticationService<UserAuthenticationData> _userAuthenticationService;

        public GoogleAuthenticationService(IAuthenticationService<UserAuthenticationData> userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService ?? throw new ArgumentNullException(nameof(userAuthenticationService));
        }

        public async Task<AuthModel> AuthenticateAsync(GoogleAuthenticationData data)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(data.Credential);
                return await _userAuthenticationService.AuthenticateAsync(new UserAuthenticationData
                {
                    LoginIdentifier = payload.Email,
                    LoginProvider = "Google",
                    Name = payload.Name
                });
            }
            catch (InvalidJwtException ex)
            {
                throw new AuthenticationFailedException(ex);
            }
        }
    }
}
