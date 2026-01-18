using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RenaultSmartCenter.Application.Common.Mappings;
using System.Reflection;

namespace RenaultSmartCenter.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Application Services
        services.AddScoped<Features.Customers.ICustomerService, Features.Customers.CustomerService>();
        services.AddScoped<Features.Vehicles.IVehicleService, Features.Vehicles.VehicleService>();
        services.AddScoped<Features.ServiceOrders.IServiceOrderService, Features.ServiceOrders.ServiceOrderService>();
        services.AddScoped<Features.Appointments.IAppointmentService, Features.Appointments.AppointmentService>();
        services.AddScoped<Features.Inventory.ISparePartService, Features.Inventory.SparePartService>();
        services.AddScoped<Features.Dashboard.IDashboardService, Features.Dashboard.DashboardService>();

        return services;
    }
}
