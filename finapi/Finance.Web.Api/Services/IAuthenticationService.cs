using Finance.Web.Api.Models;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services
{
    public interface IAuthenticationService<in T>
    {
        Task<AuthModel> AuthenticateAsync(T data);
    }
}