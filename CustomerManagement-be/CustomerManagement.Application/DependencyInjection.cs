using CustomerManagement.Application.Interfaces.Services;
using CustomerManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register customer services
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}
