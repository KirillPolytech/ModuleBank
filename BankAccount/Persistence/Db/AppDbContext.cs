using BankAccount.Features.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Persistence.Db
{
    public class AppDbContext : DbContext
    {   
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .Property(p => p.Version)
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }
}