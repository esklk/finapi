using Finance.Business.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IOperationService
    {
        Task<T[]> GetOperationsAsync<T>(int accountId, Func<IQueryable<OperationExpandedModel>, IQueryable<T>> queryAction);

        Task<OperationModel> CreateOperation(int authorId, int accountId, int categoryId, double amount, DateTime? madeAt = null);
    }
}