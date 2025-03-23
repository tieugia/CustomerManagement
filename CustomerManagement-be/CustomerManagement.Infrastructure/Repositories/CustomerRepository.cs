using CustomerManagement.Application.Interfaces.Repositories;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(CustomerManagementContext context) : base(context) { }

    public Task<bool> EmailExistsAsync(string email) 
        => _context.Customers.AsNoTracking().AnyAsync(e => e.Email == email);
}
