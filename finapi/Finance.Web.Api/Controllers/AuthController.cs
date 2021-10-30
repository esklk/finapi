using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<object> Get()
        {
            return "Hello world!";
        }
    }
}
