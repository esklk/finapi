using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IOperationCategoryService
    {
        Task<OperationCategoryModel> CreateCategory(string name, bool isIncome, int accountId);
        Task DeleteCategoryAsync(int id);
        Task<OperationCategoryModel[]> GetCategories(int accountId);
    }
}