using Finance.Business.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IOperationService
    {
        IQueryable<OperationModel> QueryOperations(int accountId);

        Task<OperationModel> CreateOperation(int authorId, int accountId, int categoryId, double ammount, DateTime? madeAt = null);
    }
}