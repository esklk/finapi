using Finance.Business.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IOperationService
    {
        Task<T[]> QueryOperations<T>(int accountId, Func<IQueryable<OperationModel>, IQueryable<T>> queryAction);

        Task<OperationModel> CreateOperation(int authorId, int accountId, int categoryId, double amount, DateTime? madeAt = null);
    }
}