using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IUserService
    {
        Task<UserModel> CreateUserAsync(UserModel user);
        
        Task<UserModel> GetUserAsync(int id);
    }
}