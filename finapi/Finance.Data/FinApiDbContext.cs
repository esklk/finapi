using Finance.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Data
{
    public class FinApiDbContext : DbContext
    {
        public FinApiDbContext(DbContextOptions<FinApiDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = default!;

        public DbSet<OperationCategory> OperationCategories { get; set; } = default!;

        public DbSet<Operation> Operations { get; set; } = default!;

        public DbSet<User> Users { get; set; } = default!;

        public DbSet<UserLogin> UserLogins { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserLogin>().HasKey(x => new
            {
                x.Provider,
                x.Identifier
            });
        }
    }
}
