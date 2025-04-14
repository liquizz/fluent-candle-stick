namespace FluentCandleStick.Application.Aggregates.CandleStick.Models;

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