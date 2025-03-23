using CustomerManagement.Domain.Entities;
using CustomerManagement.Infrastructure.Persistences;
using CustomerManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Tests.Repositories;

public class CustomerRepositoryTests
{
    private readonly CustomerManagementContext _context;
    private readonly CustomerRepository _repository;
    private readonly string MemoryDatabaseName = $"CustomerDb_Tests_{Guid.NewGuid()}";

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CustomerManagementContext>()
            .UseInMemoryDatabase(MemoryDatabaseName)
            .Options;

        _context = new CustomerManagementContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCustomer()
    {
        // Arrange
        var customer = new Customer { Id = Guid.NewGuid(), FirstName = "Gia", LastName = "Bui", Email = "gia@example.com" };

        // Act
        await _repository.AddAsync(customer);
        var result = await _repository.GetByIdAsync(customer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Gia", result!.FirstName);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCustomers()
    {
        // Arrange
        var customer1 = new Customer { Id = Guid.NewGuid(), FirstName = "A", LastName = "B", Email = "test@a.com" };
        var customer2 = new Customer { Id = Guid.NewGuid(), FirstName = "C", LastName = "D", Email = "test@b.com" };
        await _repository.AddAsync(customer1);
        await _repository.AddAsync(customer2);

        // Act
        var result = (await _repository.GetAllAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectCustomer()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer { Id = id, FirstName = "Test", LastName = "User", Email = "test@c.com" };
        await _repository.AddAsync(customer);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "Old",
            LastName = "Name",
            Email = "test@d.com",
            RowVersion = [1, 2, 3]
        };
        await _repository.AddAsync(customer);

        // Act
        customer.FirstName = "New";
        await _repository.UpdateAsync(customer);
        var updated = await _repository.GetByIdAsync(customer.Id);

        // Assert
        Assert.Equal("New", updated!.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_WhenRowVersionConflict_ThrowsDbUpdateConcurrencyException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "concurrency@example.com";
        var customer = new Customer { Id = id, FirstName = "Original", LastName = "User", Email = email };
        await _repository.AddAsync(customer);

        customer.FirstName = "Modified";
        await _repository.UpdateAsync(customer);

        var staleContextOptions = new DbContextOptionsBuilder<CustomerManagementContext>()
            .UseInMemoryDatabase(MemoryDatabaseName) // reuse same DB
            .Options;

        await using var staleContext = new CustomerManagementContext(staleContextOptions);
        var staleRepo = new CustomerRepository(staleContext);
        var staleEntity = new Customer
        {
            Id = id,
            FirstName = "Stale",
            LastName = "User",
            Email = email,
            RowVersion = [1, 2, 3]
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => staleRepo.UpdateAsync(staleEntity));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCustomer()
    {
        // Arrange
        var customer = new Customer { Id = Guid.NewGuid(), FirstName = "ToBe", LastName = "Deleted", Email = "test@e.com" };
        await _repository.AddAsync(customer);

        // Act
        await _repository.DeleteAsync(customer);
        var result = await _repository.GetByIdAsync(customer.Id);

        // Assert
        Assert.Null(result);
    }
}
