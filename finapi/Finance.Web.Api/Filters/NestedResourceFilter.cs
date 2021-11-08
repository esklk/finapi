using Finance.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Finance.Web.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NestedResourceFilter : Attribute, IAsyncResourceFilter
    {
        public string ParentResourceIdentifier { get; set; }
        
        public string NestedResourceIdentifier { get; set; }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if(!int.TryParse(context.HttpContext.GetRouteValue(ParentResourceIdentifier)?.ToString(), out var parentResourceId))
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (!int.TryParse(context.HttpContext.GetRouteValue(NestedResourceIdentifier)?.ToString(), out var nestedResourceId))
            {
                context.Result = new BadRequestResult();
                return;
            }

            if(await context.HttpContext.RequestServices.GetRequiredService<INestingCheckerFactory>()
                .Create(NestedResourceIdentifier).IsResourceNestedToParentAsync(nestedResourceId, parentResourceId))
            {
                await next();
                return;
            }

            context.Result = new NotFoundResult();
        }
    }
}
