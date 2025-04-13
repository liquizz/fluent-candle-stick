using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FluentCandleStick.Application;

public static class DependenciesBootstrapper
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        return services;
    }
} 