using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    //TODO: merge with user service
    public interface IUserLoginService
    {
        Task<UserLoginModel> CreateUserLoginAsync(int userId, string loginProvider, string loginIdentifier);

        Task<UserLoginModel?> GetUserLoginAsync(string loginProvider, string loginIdentifier);
    }
}
