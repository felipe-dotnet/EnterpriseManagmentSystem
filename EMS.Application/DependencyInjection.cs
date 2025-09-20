using EMS.Application.Services;
using EMS.Application.Servicesñ;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(DependencyInjection)); });
       
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
