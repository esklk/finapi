using Finance.Business.Models;
using Finance.Business.Services;
using Finance.Web.Api.Authorization;
using Finance.Web.Api.Filters;
using Finance.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Finance.Web.Api.Controllers
{
    [Route("api/accounts/{accountId:int}/[controller]")]
    [Authorize(Policies.AccountOwner)]
    public class OperationCategoriesController : ControllerBase
    {
        private readonly IOperationCategoryService _operationCategoryService;

        public OperationCategoriesController(IOperationCategoryService operationCategoryService)
        {
            _operationCategoryService = operationCategoryService ?? throw new System.ArgumentNullException(nameof(operationCategoryService));
        }
        
        [HttpGet]
        public async Task<OperationCategoryModel[]> Get(int accountId)
        {
            return await _operationCategoryService.GetCategoriesAsync(accountId);
        }

        [HttpPost]
        public async Task<OperationCategoryModel> Create(int accountId, [FromBody]OperationCategoryDataModel data)
        {
            return await _operationCategoryService.CreateCategoryAsync(data.Name, data.IsIncome, accountId);
        }

        [Route("{operationCategoryId}")]
        [NestedResourceFilter(ParentResourceIdentifier = "accountId", NestedResourceIdentifier = "operationCategoryId")]
        [HttpDelete]
        public async Task Delete(int operationCategoryId)
        {
            await _operationCategoryService.DeleteCategoryAsync(operationCategoryId);
        }
    }
}
