using FluentCandleStick.Domain.Interfaces;

namespace FluentCandleStick.Domain.Aggregates.CandleStick.Interfaces;

public interface ICandleStickRepository : IRepositoryBase
{
    Task ClearAsync(CancellationToken cancellationToken = default);
    
    Task InsertRangeAsync(IEnumerable<CandleStick> candleSticks, CancellationToken cancellationToken = default);
}