using Finance.Business.Models;
using System.Threading.Tasks;

namespace Finance.Business.Services
{
    public interface IOperationCategoryService
    {
        Task<OperationCategoryModel> CreateCategoryAsync(string name, bool isIncome, int accountId);
        Task DeleteCategoryAsync(int id);
        Task<OperationCategoryModel[]> GetCategoriesAsync(int accountId);
        Task<bool> IsCategoryBelongedToAccountAsync(int categoryId, int accountId);
    }
}