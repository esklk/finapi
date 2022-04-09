using Finance.Access.Exceptions;
using Finance.Access.Models;
using Finance.Business.Models;
using Finance.Business.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Access.Services.Implementation
{
    public class OperationAccessControlService : IOperationService
    {
        private readonly AccessDataModel accessData;
        private readonly IOperationService operationService;

        public OperationAccessControlService(IAccessDataProvider accessDataProvider, IOperationService operationService)
        {
            if (accessDataProvider is null)
            {
                throw new ArgumentNullException(nameof(accessDataProvider));
            }

            this.accessData = accessDataProvider.GetData() ?? throw new ArgumentException("Access data provided must not be null.", nameof(accessDataProvider));
            this.operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
        }

        public async Task<OperationModel> CreateOperation(int authorId, int accountId, int categoryId, double ammount, DateTime? madeAt = null)
        {
            if (accessData.Accounts.Any(x => x.Id == accountId && x.OperationCategoriesIds.Contains(categoryId))){
                return await operationService.CreateOperation(authorId, accountId, categoryId, ammount, madeAt);
            }

            throw new ResourceAccessViolationException();
        }

        public async Task<T[]> QueryOperations<T>(int accountId, Func<IQueryable<OperationModel>, IQueryable<T>> queryAction)
        {
            if(accessData.Accounts.Any(x=>x.Id == accountId))
            {
                return await operationService.QueryOperations(accountId, queryAction);
            }

            throw new ResourceAccessViolationException();
        }
    }
}
