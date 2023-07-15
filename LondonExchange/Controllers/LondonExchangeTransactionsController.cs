using Microsoft.AspNetCore.Mvc;
using LondonExchange.Models;
using LondonExchange.Services;
using FluentValidation;
using FluentValidation.Results;

namespace LondonExchange.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LondonExchangeTransactionsController : ControllerBase
  {
    private readonly IStockDataProvider _stockDataProvider;
    private readonly IValidator<LondonExchangeTransaction> _validator;
    private readonly ILogger<LondonExchangeTransactionsController> _logger;

    public LondonExchangeTransactionsController(IStockDataProvider stockDataProvider,
      IValidator<LondonExchangeTransaction> validator, ILogger<LondonExchangeTransactionsController> logger)
    {
      _stockDataProvider = stockDataProvider;
      _validator = validator;
      _logger = logger;
    }

    // GET: api/LondonExchangeTransactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LondonExchangeTransaction>>> GetLondonExchangeTransactions()
    {
      _logger.LogInformation("Getting all the London Exchange Transactions...");
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
      _logger.LogInformation("Getting the London Exchange stocks value...");

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
      _logger.LogInformation($"Getting the London Exchange stock value with symbol: {tickerSymbol}");
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
      _logger.LogInformation($"Getting the London Exchange stocks value with symbols: {tickerSymbols.Split(',')}");

      if (stocksValue != null && stocksValue.Any())
      {
        return Ok(stocksValue);
      }
      return NotFound();
    }

    // POST: api/LondonExchangeTransactions/PersistStock
    [HttpPost]
    [Route("PersistStock")]
    public async Task<ActionResult<IEnumerable<StockValue>>> PersistStock(LondonExchangeTransaction transaction)
    {
      var validationResults = _validator.Validate(transaction);

      if (validationResults.IsValid)
      {
        var stock = await _stockDataProvider.PersistSingleTransaction(transaction);
        _logger.LogInformation("Persisting stock...");
        return Ok(stock);
      }

      return BadRequest(validationResults.Errors);

    }

    // POST: api/LondonExchangeTransactions/PersistStocks
    [HttpPost]
    [Route("PersistStocks")]
    public async Task<ActionResult<IEnumerable<StockValue>>> PersistStocks(List<LondonExchangeTransaction> transactions)
    {
      (var failedTransactionsCount, var validationResults) = await ValidateAndPersistStocks(transactions);

      if (failedTransactionsCount > 0)
      {
        _logger.LogError($"Failed to persist transactions batch as there are {failedTransactionsCount} invalid transactions.");
        return BadRequest(validationResults.Errors);
      }

      return Ok(transactions);
    }

    private async Task<(int, ValidationResult)> ValidateAndPersistStocks(List<LondonExchangeTransaction> transactions)
    {
      ValidationResult validationResults = new();
      var failedTransactionsCount = 0;

      foreach (var transaction in transactions)
      {
        var validation = _validator.Validate(transaction);
        if (validation.IsValid)
        {
          var stock = await _stockDataProvider.PersistSingleTransaction(transaction);
          _logger.LogInformation($"Persisting stock with ticker {transaction.StockTickerSymbol} ...");
        }
        else
        {
          failedTransactionsCount++;
          validationResults.Errors.AddRange(validation.Errors);
        }
      }
      return (failedTransactionsCount, validationResults);
    }
  }
}
