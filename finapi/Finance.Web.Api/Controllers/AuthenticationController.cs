using System;
using Finance.Web.Api.Models;
using Finance.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService<GoogleAuthenticationData> _googleAuthenticationService;
        private readonly IAuthenticationService<AuthModel> _refreshAuthenticationService;

        public AuthenticationController(IAuthenticationService<GoogleAuthenticationData> googleAuthenticationService, IAuthenticationService<AuthModel> refreshAuthenticationService)
        {
            _googleAuthenticationService = googleAuthenticationService ?? throw new ArgumentNullException(nameof(googleAuthenticationService));
            _refreshAuthenticationService = refreshAuthenticationService ?? throw new ArgumentNullException(nameof(refreshAuthenticationService));
        }

        [HttpPost]
        [Route("google")]
        public async Task<AuthModel> AuthenticateWithGoogle(GoogleAuthenticationData data)
        {
            return await _googleAuthenticationService.AuthenticateAsync(data);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<AuthModel> RefreshToken(AuthModel data)
        {
            return await _refreshAuthenticationService.AuthenticateAsync(data);
        }
    }
}
