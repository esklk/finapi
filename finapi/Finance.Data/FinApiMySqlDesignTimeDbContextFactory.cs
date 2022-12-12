using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Finance.Data
{
    public class FinApiMySqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<FinApiDbContext>
    {
        public const string FinapiDatabaseConnectionStringEnvVarName =
            "FINAPI_DatabaseConfiguration__ConnectionString";

        public FinApiDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable(FinapiDatabaseConnectionStringEnvVarName) ??
                                   throw new InvalidOperationException(
                                       $"Environment variable \"{FinapiDatabaseConnectionStringEnvVarName}\"is missing.");
            var optionsBuilder = new DbContextOptionsBuilder<FinApiDbContext>();
            optionsBuilder.UseSqlServer(
                connectionString);
            return new FinApiDbContext(optionsBuilder.Options);
        }
    }
}
