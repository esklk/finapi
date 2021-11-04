using System;
using System.Threading.Tasks;
using Finance.Web.Api.Models;
using Finance.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpGet]
        public async Task<AuthModel> Get(string provider, string token)
        {   
            return await _authenticationService.AuthenticateAsync(token, provider);
        }
    }
}
