using EFCore.BulkExtensions;
using FluentCandleStick.Domain.Aggregates.CandleStick;
using FluentCandleStick.Domain.Aggregates.CandleStick.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FluentCandleStick.Database.Repositories;

public class CandleStickRepository(FluentCandleStickDbContext dbContext) : RepositoryBase(dbContext), ICandleStickRepository
{
    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.CandleSticks.ExecuteDeleteAsync(cancellationToken: cancellationToken);
    }

    public async Task InsertRangeAsync(IEnumerable<CandleStick> candleSticks,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.BulkInsertAsync(candleSticks, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<CandleStick>> GetOrderedByTimeAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.CandleSticks
            .OrderBy(cs => cs.Time)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}