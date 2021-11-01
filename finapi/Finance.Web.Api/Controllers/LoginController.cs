using Finance.Web.Api.Models;
using Finance.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Finance.Web.Api.Controllers
{
    [Route("service/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        [HttpGet]
        public IActionResult Get(string redirectUrl)
        {
            LoginOptionModel[] loginOptions = _loginService.GetLoginOptionModels();
            return View("~/Views/Login.cshtml", new LoginModel(loginOptions) { RedirectUrl = redirectUrl });
        }
    }
}
