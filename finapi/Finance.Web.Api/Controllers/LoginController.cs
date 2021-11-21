using Finance.Web.Api.Extensions;
using Finance.Web.Api.Models;
using Finance.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Finance.Web.Api.Controllers
{
    [Route("service/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IUriManager _uriManager;

        public LoginController(ILoginService loginService, IUriManager uriManager)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _uriManager = uriManager ?? throw new ArgumentNullException(nameof(uriManager));
        }

        [HttpGet]
        public IActionResult Login(string redirectUrl)
        {
            LoginOptionModel[] loginOptions = _loginService.GetLoginOptionModels(redirectUrl);

            return View(new LoginModel(loginOptions));
        }

        [HttpGet]
        [Route("redirect/google")]
        public IActionResult RedirectGoogle([Required]string code, [Required]string state)
        {
            var parameters = state.ToDictionary(";", "|", StringComparer.OrdinalIgnoreCase);
            if (parameters.TryGetValue("redirectUrl", out var redirectUrl) && !string.IsNullOrWhiteSpace(redirectUrl) 
                && parameters.TryGetValue("provider", out var provider) && !string.IsNullOrWhiteSpace(provider))
            {
                var parametrizedUrl = _uriManager.SetQueryParamaters(redirectUrl, ("provider", provider), ("token", code));
                return Redirect(parametrizedUrl);
            }

            LoginOptionModel[] loginOptions = _loginService.GetLoginOptionModels();

            return View(new LoginModel(loginOptions) { Token = code });
        }

        private IActionResult View(LoginModel model)
        {
            return View("~/Views/Login.cshtml", model);
        }
    }
}
