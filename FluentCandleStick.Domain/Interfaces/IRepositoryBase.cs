namespace FluentCandleStick.Domain.Interfaces;

public interface IRepositoryBase
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}