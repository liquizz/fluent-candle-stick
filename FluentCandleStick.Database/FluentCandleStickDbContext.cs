using FluentCandleStick.Domain.Aggregates.CandleStick;
using FluentCandleStick.Domain.Aggregates.MarketData;
using Microsoft.EntityFrameworkCore;

namespace FluentCandleStick.Database;

public class FluentCandleStickDbContext : DbContext
{
    public FluentCandleStickDbContext(DbContextOptions<FluentCandleStickDbContext> options) : base(options)
    {
    }
    
    public DbSet<MarketData> MarketData { get; set; }
    public DbSet<CandleStick> CandleSticks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MarketData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Time).IsRequired();
            entity.Property(e => e.Quantity).HasPrecision(18, 6).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 6).IsRequired();
        });
        
        modelBuilder.Entity<CandleStick>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Time).IsRequired();
            entity.Property(e => e.Open).HasPrecision(18, 6).IsRequired();
            entity.Property(e => e.Close).HasPrecision(18, 6).IsRequired();
            entity.Property(e => e.High).HasPrecision(18, 6).IsRequired();
            entity.Property(e => e.Low).HasPrecision(18, 6).IsRequired();
            entity.Property(e => e.Volume).HasPrecision(18, 6).IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
} 