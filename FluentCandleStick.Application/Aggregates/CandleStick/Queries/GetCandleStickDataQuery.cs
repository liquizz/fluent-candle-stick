using FluentCandleStick.Application.Aggregates.CandleStick.Models;
using FluentCandleStick.Domain.Aggregates.CandleStick.Interfaces;
using MediatR;

namespace FluentCandleStick.Application.Aggregates.CandleStick.Queries;

public record GetCandleStickDataQuery : IRequest<List<CandleStickDto>>;

public class GetCandleStickDataQueryHandler(ICandleStickRepository candleStickRepository)
    : IRequestHandler<GetCandleStickDataQuery, List<CandleStickDto>>
{
    public async Task<List<CandleStickDto>> Handle(GetCandleStickDataQuery request, CancellationToken cancellationToken)
    {
        var candleSticks = await candleStickRepository.GetOrderedByTimeAsync(cancellationToken);

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