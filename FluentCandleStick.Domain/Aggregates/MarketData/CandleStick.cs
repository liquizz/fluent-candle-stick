namespace FluentCandleStick.Domain.Aggregates.MarketData;

public class CandleStick
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public decimal Open { get; set; }
    public decimal Close { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Volume { get; set; }
}