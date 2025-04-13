using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCandleStick.Database;

public static class DependenciesBootstrapper
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FluentCandleStickDbContext>(options =>
            options.UseSqlite(connectionString));
        
        return services;
    }
} 