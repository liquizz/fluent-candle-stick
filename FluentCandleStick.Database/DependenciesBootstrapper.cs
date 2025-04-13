using FluentCandleStick.Database.Repositories;
using FluentCandleStick.Domain.Aggregates.CandleStick.Interfaces;
using FluentCandleStick.Domain.Aggregates.MarketData.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCandleStick.Database;

public static class DependenciesBootstrapper
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FluentCandleStickDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ICandleStickRepository, CandleStickRepository>();
        services.AddScoped<IMarketDataRepository, MarketDataRepository>();
        
        return services;
    }
} 