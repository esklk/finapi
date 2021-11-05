using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IAccountService
    {
        Task<AccountModel> CreateAccountAsync(string name, int userId);

        Task<AccountModel[]> GetAccountsAsync(int userId);

        Task DeleteAccountAsync(int id);

        Task<bool> IsAccountOwnedByUser(int accountId, int userId);
    }
}