using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<bool> EmailExistsAsync(string email);
    }
}
