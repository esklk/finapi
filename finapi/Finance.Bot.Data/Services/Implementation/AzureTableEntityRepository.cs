using Azure.Data.Tables;
using Finance.Core.Practices;

namespace Finance.Bot.Data.Services.Implementation
{
    public class AzureTableEntityRepository<TEntity> : IRepository<TEntity, string> where TEntity: class, ITableEntity, new()
    {
        private readonly TableClient _client;
        private readonly string _appName;

        public AzureTableEntityRepository(string connectionString, string appName) : this(
            new TableClient(connectionString, TableName), appName)
        {
        }

        public AzureTableEntityRepository(TableClient client, string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(appName));
            }
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.CreateIfNotExists();
            _appName = appName;
        }

        private static string TableName => typeof(TEntity).Name.Replace("Entity", string.Empty) + "Entities";

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
            await _client.UpsertEntityAsync(entity, TableUpdateMode.Replace);
        }
    }
}
