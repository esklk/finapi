using System.Threading.Tasks;

namespace Finance.Core.Practices
{
    public interface IRepository<TEntity, in TKey>
    {
        Task DeleteAsync(TKey id);
        Task<TEntity?> GetAsync(TKey id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
    }
}
