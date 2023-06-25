using Microsoft.AspNetCore.Mvc;
using LondonExchange.Models;
using LondonExchange.Services;

namespace LondonExchange.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LondonExchangeTransactionsController : ControllerBase
  {
    private readonly IStockDataProvider _stockDataProvider;

    public LondonExchangeTransactionsController(IStockDataProvider stockDataProvider)
    {     
      _stockDataProvider = stockDataProvider;
    }

    // GET: api/LondonExchangeTransactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LondonExchangeTransaction>>> GetLondonExchangeTransactions()
    {
      var allStockTransactions = await _stockDataProvider.GetAllStockTransactions();
      if (allStockTransactions == null || !allStockTransactions.Any())
      {
        return NotFound();
      }
      return Ok(allStockTransactions);
    }

    // GET: api/LondonExchangeTransactions/AllStocksValue
    [HttpGet]
    [Route("AllStocksValue")]
    public async Task<ActionResult<IEnumerable<StockValue>>> GetAllStocksValue()
    {
      var allStocksValue = await _stockDataProvider.GetStocksValue();
      if (allStocksValue.Any())
      {
        return Ok(allStocksValue);
      }
      return NotFound();
    }

    // GET: api/LondonExchangeTransactions/SingleStockValue/{tickerSymbol}
    [HttpGet()]
    [Route("SingleStockValue/{tickerSymbol}")]
    public async Task<ActionResult<IEnumerable<StockValue>>> GetStocksValue(string tickerSymbol)
    {
      var stockValue = await _stockDataProvider.GetStockValue(tickerSymbol);
      if (stockValue != null)
      {
        return Ok(stockValue);
      }
      return NotFound();
    }

    // GET: api/LondonExchangeTransactions/SingleStockValue/{tickerSymbol}
    [HttpGet()]
    [Route("StocksValue/{tickerSymbols}")]
    public async Task<ActionResult<IEnumerable<StockValue>>> GetSpecificStocksValue(string tickerSymbols)
    {
      var stocksValue = await _stockDataProvider.GetStocksValue(tickerSymbols);
      if (stocksValue != null && stocksValue.Any())
      {
        return Ok(stocksValue);
      }
      return NotFound();
    }
  }
}
