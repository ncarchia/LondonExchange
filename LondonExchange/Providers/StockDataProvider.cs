using LondonExchange.DatabaseContext;
using LondonExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace LondonExchange.Services
{
  public class StockDataProvider : IStockDataProvider
  {
    private readonly DemoContext _context;

    public StockDataProvider(DemoContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<StockValue>> GetStocksValue(string? tickerSymbols = null)
    {
      try
      {
        var tickerSymbolList = GetTickerSymbols(tickerSymbols);

        var transactions = tickerSymbolList == null
          ? await GetAllTransactions()
          : await GetSpecificTransactions(tickerSymbolList.ToList());

        return transactions != null && transactions.Any()
          ? transactions
            .GroupBy(t => t.StockTickerSymbol)
            .Select(group =>
            new StockValue
            {
              StockTickerSymbol = group.Key,
              StockTotalAmount = group.Average(g => g.ShareUnitPrice * g.SharesNumber)
            })
          : (IEnumerable<StockValue>)new List<StockValue>();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<IEnumerable<LondonExchangeTransaction>?> GetAllStockTransactions()
    {
      try
      {
        return await GetAllTransactions();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<StockValue?> GetStockValue(string tickerSymbol)
    {
      try
      {
        var stockTransactions = await _context.LondonExchangeTransactions
             .Where(t => t.StockTickerSymbol == tickerSymbol).ToListAsync();

        if (stockTransactions.Any())
        {
          var stockAvarageValue = stockTransactions.Average(st => st.ShareUnitPrice * st.SharesNumber);
          return new StockValue { StockTickerSymbol = tickerSymbol, StockTotalAmount = stockAvarageValue };
        }
        return null;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<LondonExchangeTransaction>? PersistSingleTransaction(LondonExchangeTransaction transaction)
    {
      return await PersistTransaction(transaction);
      //try
      //{
      //  return await PersistTransaction(transaction);
      //}
      //catch (Exception)
      //{
      //  throw;
      //}
    }

    private async Task<IEnumerable<LondonExchangeTransaction>?> GetAllTransactions()
      => _context.LondonExchangeTransactions != null
      ? await _context.LondonExchangeTransactions.ToListAsync()
      : null;

    private async Task<IEnumerable<LondonExchangeTransaction>?> GetSpecificTransactions(List<string> tickerSymbols)
     => _context.LondonExchangeTransactions != null
     ? await _context.LondonExchangeTransactions.Where(t => tickerSymbols.Contains(t.StockTickerSymbol)).ToListAsync()
     : null;

    private static IEnumerable<string>? GetTickerSymbols(string? tickerSymbols)
      => tickerSymbols?.Split(',').ToList();

    private async Task<LondonExchangeTransaction>? PersistTransaction(LondonExchangeTransaction entity)
    {
      if (entity != null)
      {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
      }
      return null;
    }
  }
}
