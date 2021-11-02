using Finance.Web.Api.Models;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services
{
    public interface IAuthenticationService
    {
        Task<AuthModel> AuthenticateAsync(string token, string provider);
    }
}