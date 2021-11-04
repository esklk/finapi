using Finance.Business.Services;
using Finance.Web.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Finance.Web.Api.Authorization.Requirements
{
    public class AccountOwnerRequirement : IHttpAuthorizationRequirement
    {
        public async Task<bool> FulfillAsync(HttpContext context)
        {
            if (!int.TryParse(context.GetRouteValue("accountId")?.ToString(), out var accountId))
            {
                return false;
            }

            int? userId = context.User.GetUserId();
            if (!userId.HasValue)
            {
                return false;
            }

            return await context.RequestServices.GetRequiredService<IAccountService>().IsAccountOwnedByUser(accountId, userId.Value);
        }
    }
}
