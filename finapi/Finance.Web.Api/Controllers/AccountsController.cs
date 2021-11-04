using Finance.Business.Models;
using Finance.Business.Services;
using Finance.Web.Api.Authorization;
using Finance.Web.Api.Extensions;
using Finance.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new System.ArgumentNullException(nameof(accountService));
        }

        [HttpPost]
        public async Task<AccountModel> Create(CreateAccountModel data)
        {
            return await _accountService.CreateAccountAsync(data.Name, User.GetUserId().Value);
        }

        [Route("{accountId}")]
        [Authorize(Policies.AccountOwner)]
        [HttpDelete]
        public async Task Delete(int accountId)
        {
            await _accountService.DeleteAccountAsync(accountId);
        }
    }
}
