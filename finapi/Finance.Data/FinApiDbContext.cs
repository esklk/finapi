using Finance.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Data
{
    public class FinApiDbContext : DbContext
    {
        public FinApiDbContext(DbContextOptions<FinApiDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserLogin>().HasKey(x => new
            {
                x.UserId,
                x.Provider,
                x.Identifier
            });
        }
    }
}
