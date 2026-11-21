using HrSystem.Application.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace HrSystem.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers application services (use cases). Each functional module
    /// (time tracking, absences, projects, ...) registers its services here.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<EmployeeService>();

        return services;
    }
}
