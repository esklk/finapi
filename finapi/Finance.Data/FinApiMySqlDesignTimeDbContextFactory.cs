using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Finance.Data
{
    public class FinApiMySqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<FinApiDbContext>
    {
        public FinApiDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinApiDbContext>();
            optionsBuilder.UseMySql($"server={GetValue("Server")};port={GetValue("Port")};database={GetValue("Database")};UserId={GetValue("UserId")};Password={GetValue("Password")}", new MySqlServerVersion("8.0.21"));

            return new FinApiDbContext(optionsBuilder.Options);
        }

        private static string GetValue(string name)
        {
            var fullName = $"FINAPI_DatabaseConfiguration__FinaApiDb__{name}";

            return Environment.GetEnvironmentVariable(fullName) ?? throw new InvalidOperationException($"Failed to get environment variable: {fullName}.");
        }
    }
}
