using CudstomerManagement.Application.DTOs;
using CusstomerManagement.Application.DTOs;

namespace CustomerManagement.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>?> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(Guid id);
    Task<CustomerDto?> CreateAsync(AddCustomerDto addCustomerDto);
    Task UpdateAsync(UpdateCustomerDto updateCustomerDto);
    Task DeleteAsync(Guid id);
}
