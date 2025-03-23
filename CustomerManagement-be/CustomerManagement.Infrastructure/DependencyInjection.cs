using CudstomerManagement.Application.Interfaces;
using CustomerManagement.Application.Interfaces.Repositories;
using CustomerManagement.Application.Interfaces.Services;
using CustomerManagement.Infrastructure.Persistences;
using CustomerManagement.Infrastructure.Repositories;
using CustomerManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SympliSearch.Infrastructure.Services;

namespace CustomerManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<CustomerManagementContext>(opt => opt.UseInMemoryDatabase("CustomerDb_Dev"));

        // Register caching services
        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();

        // Register authentication services
        services.AddScoped<IAuthService, AuthService>();

        // Register customer repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
