using CustomerManagement.Common.Extensions;
using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Persistences;

public class CustomerManagementContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public CustomerManagementContext(DbContextOptions<CustomerManagementContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var rowVersion = modelBuilder.Entity<Customer>()
                                     .Property(c => c.RowVersion)
                                     .IsRowVersion();
        if (Database.IsInMemory())
            rowVersion.HasValueGenerator<InMemoryRowVersionGenerator>();

        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
