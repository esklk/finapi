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

        public AuthenticationController(IAuthenticationService<GoogleAuthenticationData> googleAuthenticationService)
        {
            _googleAuthenticationService = googleAuthenticationService ?? throw new ArgumentNullException(nameof(googleAuthenticationService));
        }

        [HttpPost]
        [Route("google")]
        public async Task<AuthModel> AuthenticateWithGoogle(GoogleAuthenticationData data)
        {
            return await _googleAuthenticationService.AuthenticateAsync(data);
        }
    }
}
