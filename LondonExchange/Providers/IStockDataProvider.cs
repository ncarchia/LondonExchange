using LondonExchange.Models;

namespace LondonExchange.Services
{
  public interface IStockDataProvider
  {
    Task<IEnumerable<LondonExchangeTransaction>?> GetAllStockTransactions();
    Task<StockValue?> GetStockValue(string tickerSymbol);
    Task<IEnumerable<StockValue>> GetStocksValue(string? tickerSymbols = null);
  }
}
