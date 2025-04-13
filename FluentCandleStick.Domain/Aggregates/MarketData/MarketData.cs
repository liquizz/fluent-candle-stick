namespace FluentCandleStick.Domain.Aggregates.MarketData;

public class MarketData
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
}