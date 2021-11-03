using Finance.Business.Models;
using Finance.Business.Services;
using Finance.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new System.ArgumentNullException(nameof(accountService));
        }

        public async Task<AccountModel> Create(CreateAccountModel data)
        {
            return await _accountService.CreateAccountAsync(data.Name, CurrentUserId);
        }
    }
}
