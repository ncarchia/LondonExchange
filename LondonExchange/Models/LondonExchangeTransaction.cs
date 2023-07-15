using LondonExchange.Converters;
using System.Text.Json.Serialization;

namespace LondonExchange.Models;

public partial class LondonExchangeTransaction
{
  [JsonConverter(typeof(StringToGuidConverter))]
  public Guid TransactionId { get; set; }

  public string StockTickerSymbol { get; set; } = null!;

  public decimal ShareUnitPrice { get; set; }

  public decimal SharesNumber { get; set; }

  [JsonConverter(typeof(StringToGuidConverter))]
  public Guid BrokerId { get; set; }

  public DateTime TransactionDate { get; set; }
}
