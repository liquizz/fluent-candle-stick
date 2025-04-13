using FluentCandleStick.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FluentCandleStick.Application.Aggregates.CandleStick.Queries;

public record GetCandleStickDataQuery : IRequest<List<CandleStickDto>>;

public class GetCandleStickDataQueryHandler : IRequestHandler<GetCandleStickDataQuery, List<CandleStickDto>>
{
    private readonly FluentCandleStickDbContext _dbContext;

    public GetCandleStickDataQueryHandler(FluentCandleStickDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CandleStickDto>> Handle(GetCandleStickDataQuery request, CancellationToken cancellationToken)
    {
        var candleSticks = await _dbContext.CandleSticks
            .OrderBy(cs => cs.Time)
            .ToListAsync(cancellationToken);

        return candleSticks.Select(cs => new CandleStickDto
        {
            Time = cs.Time,
            Open = cs.Open,
            Close = cs.Close,
            High = cs.High,
            Low = cs.Low,
            Volume = cs.Volume,
            IsUp = cs.Close > cs.Open
        }).ToList();
    }
}

public class CandleStickDto
{
    public DateTime Time { get; set; }
    public decimal Open { get; set; }
    public decimal Close { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Volume { get; set; }
    public bool IsUp { get; set; }
} 