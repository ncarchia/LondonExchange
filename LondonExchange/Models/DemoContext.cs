using Microsoft.EntityFrameworkCore;

namespace LondonExchange.Models;

public partial class DemoContext : DbContext
{
  public DemoContext()
  {
  }

  public DemoContext(DbContextOptions<DemoContext> options)
      : base(options)
  {
  }

  public virtual DbSet<LondonExchangeTransaction> LondonExchangeTransactions { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  { }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<LondonExchangeTransaction>(entity =>
    {
      entity.HasKey(e => e.TransactionId);

      entity.Property(e => e.TransactionId).ValueGeneratedNever();
      entity.Property(e => e.ShareUnitPrice).HasColumnType("decimal(18, 0)");
      entity.Property(e => e.SharesNumber).HasColumnType("decimal(18, 0)");
      entity.Property(e => e.StockTickerSymbol)
              .HasMaxLength(50)
              .IsUnicode(false);
      entity.Property(e => e.TransactionDate).HasColumnType("date");
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
