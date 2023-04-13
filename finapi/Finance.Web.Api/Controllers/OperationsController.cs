using Finance.Business.Models;
using Finance.Business.Services;
using Finance.Web.Api.Extensions;
using Finance.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Web.Api.Controllers
{
    [Route("api/accounts/{accountId:int}/[controller]")]
    [Authorize]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationService _operationService;

        public OperationsController(IOperationService operationService)
        {
            _operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
        }

        [HttpGet]
        public async Task<dynamic[]> Get(int accountId, ODataQueryOptions<OperationModel> query)
        {
            //TODO: move this to some service to keep controller thin
            var result =
                await _operationService.GetOperationsAsync(accountId, x => query.ApplyTo(x) as IQueryable<dynamic>);

            if (query.SelectExpand != null)
            {
                result = result.Cast<ISelectExpandWrapper>().Select(x => x.ToDictionary()).ToArray();
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<OperationModel>> Create(int accountId, [FromBody]OperationDataModel data)
        {
            OperationModel result = await _operationService
                .CreateOperation(User.GetUserId(), accountId, data.CategoryId, data.Ammount, data.CreatedAt);

            return CreatedAtAction(nameof(Create), result);
        }
    }
}
