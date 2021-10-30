using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IUserService
    {
        Task<UserModel> CreateUserAsync(string name, string loginProvider, string loginIdentifier);
        Task<UserModel> GetUserAsync(string loginProvider, string loginIdentifier);
    }
}