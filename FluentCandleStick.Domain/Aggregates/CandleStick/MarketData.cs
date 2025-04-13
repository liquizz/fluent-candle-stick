namespace FluentCandleStick.Domain.Aggregates.CandleStick;

public class MarketData
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
}