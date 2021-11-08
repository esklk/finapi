using Finance.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Web.Api.Services.Implementation
{
    public class OperationCategoryNestingChecker : INestingChecker
    {
        private readonly IOperationCategoryService _operationCategoryService;

        public OperationCategoryNestingChecker(IOperationCategoryService operationCategoryService)
        {
            _operationCategoryService = operationCategoryService ?? throw new ArgumentNullException(nameof(operationCategoryService));
        }

        public async Task<bool> IsResourceNestedToParentAsync(int resourceId, int parentId)
        {
            return await _operationCategoryService.IsCategoryBelongedToAccountAsync(resourceId, parentId);
        }
    }
}
