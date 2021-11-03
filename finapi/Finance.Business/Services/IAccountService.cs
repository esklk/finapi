using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IAccountService
    {
        Task<AccountModel> CreateAccountAsync(string name, int userId);
    }
}