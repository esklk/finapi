using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IUserLoginService
    {
        Task<UserLoginModel> CreateUserLoginAsync(int userId, string loginProvider, string loginIdentifier);

        Task<UserLoginModel?> GetUserLoginAsync(string loginProvider, string loginIdentifier);
    }
}
