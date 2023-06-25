namespace LondonExchange.Models;

public partial class LondonExchangeTransaction
{
  public Guid TransactionId { get; set; }

  public string StockTickerSymbol { get; set; } = null!;

  public decimal ShareUnitPrice { get; set; }

  public decimal SharesNumber { get; set; }

  public Guid BrokerId { get; set; }

  public DateTime TransactionDate { get; set; }
}
