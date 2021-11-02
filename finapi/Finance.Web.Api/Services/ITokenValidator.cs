using Finance.Web.Api.Models;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services
{
    public interface ITokenValidator
    {
        Task<TokenValidationResult> ValidateAsync(string token);
    }
}
