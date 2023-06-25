using LondonExchange.Controllers;
using LondonExchange.Models;
using LondonExchange.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LondonExchange.Tests.ControllerTests
{
  public class LondonExchangeTransactionsControllerTests
  {
    private readonly LondonExchangeTransactionsController _controller;
    private readonly Mock<IStockDataProvider> _dataProvider = new();

    public LondonExchangeTransactionsControllerTests()
    {
      _controller = new LondonExchangeTransactionsController(_dataProvider.Object);
    }

    [Fact]
    public async void GetLondonExchangeTransactions_GivenExistingTransactions_ShouldReturnStockTransactions()
    {
      _dataProvider.Setup(dp => dp.GetAllStockTransactions())
        .ReturnsAsync(GetTestTransactions());

      var result = await _controller.GetLondonExchangeTransactions();

      Assert.NotNull(result);
      var okObject = result.Result as OkObjectResult;
      var list = okObject?.Value as List<LondonExchangeTransaction>;
      Assert.Equal(200, okObject?.StatusCode);
      Assert.Equal(4, list?.Count);
    }

    [Fact]
    public async void GetLondonExchangeTransactions_GivenNoExistingTransactions_ShouldReturnNotFound()
    {
      _dataProvider.Setup(dp => dp.GetAllStockTransactions())
        .ReturnsAsync(new List<LondonExchangeTransaction>());

      var result = await _controller.GetLondonExchangeTransactions();

      Assert.NotNull(result);
      var notFoundResult = result.Result as NotFoundResult;
      Assert.Equal(404, notFoundResult?.StatusCode);
    }

    [Fact]
    public async void GetLondonExchangeTransactions_GivenAnExceptionIsThrown_ShouldThrowException()
    {
      object value = _dataProvider.Setup(dp => dp.GetStockValue(It.IsAny<string>()))
        .ThrowsAsync(new Exception());

      await Assert.ThrowsAsync<Exception>(() => _controller.GetStocksValue(It.IsAny<string>()));
    }

    [Fact]
    public async void GetStocksValue_GivenExistingTickerSymbol_ShouldReturnRelatedStockValue()
    {
      _dataProvider.Setup(dp => dp.GetStockValue(It.IsAny<string>()))
        .ReturnsAsync(new StockValue { StockTickerSymbol = "TEST1:LN", StockTotalAmount = 17.34m });

      var result = await _controller.GetStocksValue(It.IsAny<string>());

      Assert.NotNull(result);
      var okObject = result.Result as OkObjectResult;
      var stock = okObject?.Value as StockValue;
      Assert.Equal(200, okObject?.StatusCode);
      Assert.Equal("TEST1:LN", stock?.StockTickerSymbol);
      Assert.Equal(17.34m, stock?.StockTotalAmount);
    }

    [Fact]
    public async void GetStocksValue_GivenNotExistingTickerSymbol_ShouldReturnNotFound()
    {
      object value = _dataProvider.Setup(dp => dp.GetStockValue(It.IsAny<string>()))
        .ReturnsAsync((StockValue)null);

      var result = await _controller.GetStocksValue(It.IsAny<string>());

      Assert.NotNull(result);
      var notFoundResult = result.Result as NotFoundResult;
      Assert.Equal(404, notFoundResult?.StatusCode);
    }

    [Fact]
    public async void GetStocksValue_GivenAnExceptionIsThrown_ShouldThrowException()
    {
      object value = _dataProvider.Setup(dp => dp.GetStockValue(It.IsAny<string>()))
        .ThrowsAsync(new Exception());

      await Assert.ThrowsAsync<Exception>(() => _controller.GetStocksValue(It.IsAny<string>()));
    }

    private static List<LondonExchangeTransaction> GetTestTransactions()
    {
      return new List<LondonExchangeTransaction>
      {
        new LondonExchangeTransaction{TransactionId = new Guid(), BrokerId = Guid.NewGuid(), SharesNumber = 2, ShareUnitPrice = 10.5m, StockTickerSymbol = "TEST1:LN", TransactionDate = DateTime.Now },
        new LondonExchangeTransaction{TransactionId = new Guid(), BrokerId = Guid.NewGuid(), SharesNumber = 3, ShareUnitPrice = 5.5m, StockTickerSymbol = "TEST2:LN", TransactionDate = DateTime.Now },
        new LondonExchangeTransaction{TransactionId = new Guid(), BrokerId = Guid.NewGuid(), SharesNumber = 1, ShareUnitPrice = 3.5m, StockTickerSymbol = "TEST1:LN", TransactionDate = DateTime.Now },
        new LondonExchangeTransaction{TransactionId = new Guid(), BrokerId = Guid.NewGuid(), SharesNumber = 2, ShareUnitPrice = 10.5m, StockTickerSymbol = "TEST4:LN", TransactionDate = DateTime.Now }
      };
    }
  }
}
