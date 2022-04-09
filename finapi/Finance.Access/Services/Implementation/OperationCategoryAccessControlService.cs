using Finance.Access.Exceptions;
using Finance.Access.Models;
using Finance.Business.Models;
using Finance.Business.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Access.Services.Implementation
{
    class OperationCategoryAccessControlService : IOperationCategoryService
    {
        private readonly AccessDataModel accessData;
        private readonly IOperationCategoryService operationCategoryService;

        public OperationCategoryAccessControlService(IAccessDataProvider accessDataProvider, IOperationCategoryService operationCategoryService)
        {
            if (accessDataProvider is null)
            {
                throw new ArgumentNullException(nameof(accessDataProvider));
            }

            this.accessData = accessDataProvider.GetData() ?? throw new ArgumentException("Access data provided must not be null.", nameof(accessDataProvider));
            this.operationCategoryService = operationCategoryService ?? throw new ArgumentNullException(nameof(operationCategoryService));
        }

        public async Task<OperationCategoryModel> CreateCategoryAsync(string name, bool isIncome, int accountId)
        {
            if (accessData.Accounts.Any(x => x.Id == accountId))
            {
                return await operationCategoryService.CreateCategoryAsync(name, isIncome, accountId);
            }
            
            throw new ResourceAccessViolationException();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            if (accessData.Accounts.Any(x => x.OperationCategoriesIds.Contains(id)))
            {
                await operationCategoryService.DeleteCategoryAsync(id);
                return;
            }

            throw new ResourceAccessViolationException();
        }

        public async Task<OperationCategoryModel[]> GetCategoriesAsync(int accountId)
        {
            if (accessData.Accounts.Any(x => x.Id == accountId))
            {
                return await operationCategoryService.GetCategoriesAsync(accountId);
            }

            throw new ResourceAccessViolationException();
        }
    }
}
