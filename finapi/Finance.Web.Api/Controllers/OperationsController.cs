using Finance.Business.Models;
using Finance.Business.Services;
using Finance.Web.Api.Authorization;
using Finance.Web.Api.Extensions;
using Finance.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Web.Api.Controllers
{
    [Route("api/accounts/{accountId:int}/[controller]")]
    [Authorize(Policies.AccountOwner)]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationService _operationService;

        public OperationsController(IOperationService operationService)
        {
            _operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
        }

        [HttpGet]
        public IQueryable Get(int accountId, ODataQueryOptions<OperationModel> query)
        {
            return query.ApplyTo(_operationService.QueryOperations(accountId));
        }

        [HttpPost]
        public async Task<OperationModel> Create(int accountId, [FromBody]OperationDataModel data)
        {
            return await _operationService.CreateOperation(User.GetUserId(), accountId, data.CategoryId, data.Ammount, data.CreatedAt);
        }
    }
}
