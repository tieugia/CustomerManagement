using CudstomerManagement.Application.DTOs;
using CudstomerManagement.Application.Interfaces;
using CusstomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces.Repositories;
using CustomerManagement.Application.Interfaces.Services;
using CustomerManagement.Common.Constants;
using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICacheService _cacheService;
    public CustomerService(ICustomerRepository customerRepository, ICacheService cacheService)
    {
        _customerRepository = customerRepository;
        _cacheService = cacheService;
    }

    public async Task<CustomerDto?> CreateAsync(AddCustomerDto addCustomerDto)
    {
        if(await _customerRepository.EmailExistsAsync(addCustomerDto.Email))
            return null;

        var customer = new Customer
        {
            Id = addCustomerDto.Id,
            FirstName = addCustomerDto.FirstName,
            MiddleName = addCustomerDto.MiddleName,
            LastName = addCustomerDto.LastName,
            Email = addCustomerDto.Email
        };

        var createdCustomer = await _customerRepository.AddAsync(customer);
        if (createdCustomer == null)
            return null;

        _cacheService.Remove(CacheKeys.AllCustomers);

        return new CustomerDto
        {
            Id = createdCustomer.Id,
            Name = createdCustomer.FirstName + " " + createdCustomer.MiddleName + " " + createdCustomer.LastName,
            Email = createdCustomer.Email,
            RowVersion = createdCustomer.RowVersion
        };

    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        await _customerRepository.DeleteAsync(customer);

        _cacheService.Remove(CacheKeys.Customer(id));
        _cacheService.Remove(CacheKeys.AllCustomers);
    }

    public async Task<IEnumerable<CustomerDto>?> GetAllAsync()
    {
        if (_cacheService.TryGet(CacheKeys.AllCustomers, out IEnumerable<CustomerDto>? cachedCustomers) && cachedCustomers != null)
            return cachedCustomers;

        var customers = await _customerRepository.GetAllAsync();
        if (customers == null || !customers.Any())
            return null;

        var customerDtos = customers.Select(customer => new CustomerDto
        {
            Id = customer.Id,
            Name = string.Join(" ", customer.FirstName, customer.MiddleName, customer.LastName),
            Email = customer.Email,
            RowVersion = customer.RowVersion
        });

        _cacheService.Set(CacheKeys.AllCustomers, customerDtos, TimeSpan.FromHours(1));
        return customerDtos;
    }


    public async Task<CustomerDto> GetByIdAsync(Guid id)
    {
        string customerCacheKey = CacheKeys.Customer(id);
        if (_cacheService.TryGet(customerCacheKey, out CustomerDto? cachedCustomer) && cachedCustomer != null)
        {
            return cachedCustomer;
        }

        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        var customerDto = new CustomerDto
        {
            Id = customer.Id,
            Name = string.Join(" ", customer.FirstName, customer.MiddleName, customer.LastName),
            Email = customer.Email,
            RowVersion = customer.RowVersion
        };

        _cacheService.Set(customerCacheKey, customerDto, TimeSpan.FromHours(1));
        return customerDto;
    }


    public async Task UpdateAsync(UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _customerRepository.GetByIdAsync(updateCustomerDto.Id);
        if (customer == null) 
            throw new KeyNotFoundException("Customer not found");

        customer.FirstName = updateCustomerDto.FirstName;
        customer.MiddleName = updateCustomerDto.MiddleName;
        customer.LastName = updateCustomerDto.LastName;
        customer.RowVersion = updateCustomerDto.RowVersion;

        await _customerRepository.UpdateAsync(customer);

        _cacheService.Remove(CacheKeys.Customer(updateCustomerDto.Id));
        _cacheService.Remove(CacheKeys.AllCustomers);
    }
}
