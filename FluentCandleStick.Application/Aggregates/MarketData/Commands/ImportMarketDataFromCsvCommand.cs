using System.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using System.Globalization;
using FluentCandleStick.Domain.Aggregates.CandleStick.Interfaces;
using FluentCandleStick.Domain.Aggregates.MarketData.Interfaces;

namespace FluentCandleStick.Application.Aggregates.MarketData.Commands;

public record ImportMarketDataFromCsvCommand(Stream CsvFileStream) : IRequest<bool>;

public class ImportMarketDataFromCsvCommandHandler(
    IMarketDataRepository marketDataRepository,
    ICandleStickRepository candleStickRepository
    )
    : IRequestHandler<ImportMarketDataFromCsvCommand, bool>
{
    public async Task<bool> Handle(ImportMarketDataFromCsvCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Clear existing data
            await marketDataRepository.ClearAsync(cancellationToken);
            await candleStickRepository.ClearAsync(cancellationToken);
            await marketDataRepository.SaveChangesAsync(cancellationToken);
            
            // Read CSV data
            using var reader = new StreamReader(request.CsvFileStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            });
            
            csv.Context.RegisterClassMap<MarketDataMap>();
            var records = csv.GetRecords<Domain.Aggregates.MarketData.MarketData>().ToList();
            
            // Add data to database
            await marketDataRepository.InsertRangeAsync(records, cancellationToken);
            await marketDataRepository.SaveChangesAsync(cancellationToken);
            
            await CalculateCandleSticks(cancellationToken);
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    private async Task CalculateCandleSticks(CancellationToken cancellationToken)
    {
        // Get all market data ordered by time
        var marketData = (await marketDataRepository.GetOrderedByTimeAsync(cancellationToken)).ToList();
        
        if (marketData.Count == 0)
            return;
        
        // Group market data by minute
        var groupedByMinute = marketData
            .GroupBy(md => new DateTime(md.Time.Year, md.Time.Month, md.Time.Day, md.Time.Hour, md.Time.Minute, 0));
        
        var candleSticks = new List<Domain.Aggregates.CandleStick.CandleStick>();
        
        foreach (var group in groupedByMinute)
        {
            var minuteData = group.ToList();
            
            if (minuteData.Count == 0)
                continue;
            
            var candleStick = new Domain.Aggregates.CandleStick.CandleStick
            {
                Time = group.Key,
                Open = minuteData.First().Price,
                Close = minuteData.Last().Price,
                High = minuteData.Max(md => md.Price),
                Low = minuteData.Min(md => md.Price),
                Volume = minuteData.Sum(md => md.Quantity)
            };
            
            candleSticks.Add(candleStick);
        }
        
        await candleStickRepository.InsertRangeAsync(candleSticks, cancellationToken);
        await candleStickRepository.SaveChangesAsync(cancellationToken);
    }
}

public class MarketDataMap : ClassMap<Domain.Aggregates.MarketData.MarketData>
{
    public MarketDataMap()
    {
        Map(m => m.Id).Ignore();
        Map(m => m.Time).Name("TIME").TypeConverterOption.Format("dd/MM/yyyy HH:mm:ss.fff");
        Map(m => m.Quantity).Name("QUANTITY");
        Map(m => m.Price).Name("PRICE");
    }
} 