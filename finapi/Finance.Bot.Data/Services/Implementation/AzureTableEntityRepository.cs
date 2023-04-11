using Azure.Data.Tables;
using Finance.Core.Practices;

namespace Finance.Bot.Data.Services.Implementation
{
    public class AzureTableEntityRepository<TEntity> : IRepository<TEntity, string> where TEntity: class, ITableEntity, new()
    {
        private readonly TableClient _client;
        private readonly string _partitionKey;

        public AzureTableEntityRepository(string connectionString, string partitionKey) : this(
            new TableClient(connectionString, TableName), partitionKey)
        {
        }

        public AzureTableEntityRepository(TableClient client, string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(partitionKey));
            }
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.CreateIfNotExists();
            _partitionKey = partitionKey;
        }

        private static string TableName => typeof(TEntity).Name.Replace("Entity", string.Empty) + "Entities";

        public async Task DeleteAsync(string id)
        {
            await _client.DeleteEntityAsync(_partitionKey, id);
        }

        public async Task<TEntity?> GetAsync(string id)
        {
            var response = await _client.GetEntityIfExistsAsync<TEntity>(_partitionKey, id);
            return response.HasValue ? response.Value : default;
        }

        public async Task InsertAsync(TEntity entity)
        {
            entity.PartitionKey = _partitionKey;
            await _client.AddEntityAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            entity.PartitionKey = _partitionKey;
            await _client.UpsertEntityAsync(entity, TableUpdateMode.Replace);
        }
    }
}
