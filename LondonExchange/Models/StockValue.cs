namespace LondonExchange.Models
{
  public class StockValue
  {
    public string StockTickerSymbol { get; set; } = null!;

    public decimal StockTotalAmount { get; set; }
  }
}
