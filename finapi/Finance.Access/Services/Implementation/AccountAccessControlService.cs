using Finance.Access.Models;
using Finance.Business.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Access.Services.Implementation
{
    public class AccountAccessControlService : IAccountService
    {
        private readonly AccessDataModel accessData;
        private readonly IAccountService accountService;

        public AccountAccessControlService(IAccessDataProvider accessDataProvider, IAccountService accountService)
        {
            if (accessDataProvider is null)
            {
                throw new ArgumentNullException(nameof(accessDataProvider));
            }

            this.accessData = accessDataProvider.GetData() ?? throw new ArgumentException("Access data provided must not be null.", nameof(accessDataProvider));
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public async Task<Business.Models.AccountModel> CreateAccountAsync(string name, int userId)
        {
            return await accountService.CreateAccountAsync(name, userId);
        }

        public async Task DeleteAccountAsync(int id)
        {
            if (accessData.Accounts.Any(x => x.Id == id))
            {
                await accountService.DeleteAccountAsync(id);
                return;
            }
        }

        public async Task<Business.Models.AccountModel[]> GetAccountsAsync(int userId)
        {
            return await accountService.GetAccountsAsync(userId);
        }
    }
}
