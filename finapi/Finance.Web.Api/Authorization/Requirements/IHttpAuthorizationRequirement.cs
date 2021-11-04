using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Finance.Web.Api.Authorization.Requirements
{
    public interface IHttpAuthorizationRequirement : IAuthorizationRequirement
    {
        Task<bool> FulfillAsync(HttpContext context);
    }
}
