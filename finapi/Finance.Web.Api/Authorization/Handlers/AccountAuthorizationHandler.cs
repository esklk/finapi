using Finance.Web.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Finance.Web.Api.Authorization.Handlers
{
    public class AccountAuthorizationHandler : AuthorizationHandler<IHttpAuthorizationRequirement, HttpContext>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IHttpAuthorizationRequirement requirement, HttpContext resource)
        {
            if (context.User.Identity.IsAuthenticated && await requirement.FulfillAsync(resource))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}
