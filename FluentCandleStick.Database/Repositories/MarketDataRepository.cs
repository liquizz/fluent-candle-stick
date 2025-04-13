using EFCore.BulkExtensions;
using FluentCandleStick.Domain.Aggregates.MarketData;
using FluentCandleStick.Domain.Aggregates.MarketData.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FluentCandleStick.Database.Repositories;

public class MarketDataRepository(FluentCandleStickDbContext context) : RepositoryBase(context), IMarketDataRepository
{
    public async Task InsertRangeAsync(IEnumerable<MarketData> marketData, CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkInsertAsync(marketData, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<MarketData>> GetOrderedByTimeAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.MarketData
            .OrderBy(md => md.Time)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.MarketData.ExecuteDeleteAsync(cancellationToken: cancellationToken);
    }
}