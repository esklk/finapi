using Azure.Data.Tables;
using Finance.Core.Practices;

namespace Finance.Bot.Data.Services.Implementation
{
    public class TableEntityRepository<TEntity> : IRepository<TEntity, string> where TEntity: class, ITableEntity, new()
    {
        private readonly TableClient _client;
        private readonly string _appName;

        public TableEntityRepository(TableClient client, string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(appName));
            }
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _appName = appName;
        }

        public async Task DeleteAsync(string id)
        {
            await _client.DeleteEntityAsync(_appName, id);
        }

        public async Task<TEntity?> GetAsync(string id)
        {
            var response = await _client.GetEntityIfExistsAsync<TEntity>(_appName, id);
            return response.HasValue ? response.Value : default;
        }

        public async Task InsertAsync(TEntity entity)
        {
            entity.PartitionKey = _appName;
            await _client.AddEntityAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            entity.PartitionKey = _appName;
            await _client.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
        }
    }
}
