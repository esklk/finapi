using System;
using System.Threading.Tasks;
using Finance.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpGet]
        public async Task<object> Get(string provider, string token)
        {
            
            return await _authenticationService.AuthenticateAsync(token, provider);
        }
    }
}
