using CudstomerManagement.Application.DTOs;
using CudstomerManagement.Application.Interfaces;
using CusstomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces.Repositories;
using CustomerManagement.Application.Services;
using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CustomerManagement.Application.Tests.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerIsInCache_ReturnsFromCache()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var cachedCustomer = new CustomerDto { Id = customerId, Name = "Cached User", Email = "cached@test.com" };
        _cacheServiceMock.Setup(c => c.TryGet($"customer_{customerId}", out cachedCustomer)).Returns(true);

        // Act
        var result = await _customerService.GetByIdAsync(customerId);

        // Assert
        Assert.Equal(cachedCustomer.Id, result.Id);
        Assert.Equal(cachedCustomer.Name, result.Name);
        _customerRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerExistsInDb_ReturnsMappedDto_AndCachesIt()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            FirstName = "Gia",
            MiddleName = "Tieu",
            LastName = "Bui",
            Email = "gia@example.com",
            RowVersion = [1, 2, 3]
        };
        _cacheServiceMock.Setup(c => c.TryGet($"customer_{customerId}", out It.Ref<CustomerDto?>.IsAny)).Returns(false);
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);

        // Act
        var result = await _customerService.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Gia Tieu Bui", result.Name);
        Assert.Equal("gia@example.com", result.Email);
        _cacheServiceMock.Verify(c => c.Set($"customer_{customerId}", It.IsAny<CustomerDto>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerNotFound_ThrowsException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _cacheServiceMock.Setup(c => c.TryGet($"customer_{customerId}", out It.Ref<CustomerDto?>.IsAny)).Returns(false);
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.GetByIdAsync(customerId));
    }

    [Fact]
    public async Task UpdateAsync_WhenCustomerExists_Updates_AndRemovesCache()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var existingCustomer = new Customer { Id = customerId, FirstName = "Old", LastName = "Name", RowVersion = [1] };
        var dto = new UpdateCustomerDto { Id = customerId, FirstName = "New", LastName = "Name", RowVersion = [1] };

        _customerRepositoryMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(existingCustomer);
        _customerRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        // Act
        await _customerService.UpdateAsync(dto);

        // Assert
        _customerRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Customer>(c => c.FirstName == "New")), Times.Once);
        _cacheServiceMock.Verify(c => c.Remove($"customer_{customerId}"), Times.Once);
        _cacheServiceMock.Verify(c => c.Remove("all_customers"), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenCustomerNotFound_ThrowsException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var dto = new UpdateCustomerDto { Id = customerId, FirstName = "Missing", LastName = "User" };
        _customerRepositoryMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.UpdateAsync(dto));
    }
}
