using Finance.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Data
{
    public class FinApiDbContext : DbContext
    {
        public FinApiDbContext(DbContextOptions<FinApiDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<OperationCategory> OperationCategories { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserLogin> UserLogins { get; set; }

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
