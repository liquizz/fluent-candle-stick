using FluentCandleStick.Domain.Interfaces;

namespace FluentCandleStick.Domain.Aggregates.MarketData.Interfaces;

public interface IMarketDataRepository : IRepositoryBase
{
    Task InsertRangeAsync(IEnumerable<MarketData> marketData, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<MarketData>> GetOrderedByTimeAsync(CancellationToken cancellationToken = default);
    
    Task ClearAsync(CancellationToken cancellationToken = default);
}