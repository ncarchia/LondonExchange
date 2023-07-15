using FluentValidation;
using LondonExchange.Models;

namespace LondonExchange.Validators
{
  public class TransactionValidator : AbstractValidator<LondonExchangeTransaction>
  {
    public TransactionValidator()
    {
      RuleFor(txn => txn.TransactionId).NotEmpty().Must(BeAValidGuid)
          .WithMessage("Validation failed: TransactionId must be a valid guid.");
      RuleFor(txn => txn.StockTickerSymbol).NotEmpty()
          .WithMessage("Validation failed: TickerSymbol must not be empty.");
      RuleFor(txn => txn.SharesNumber).NotEqual(0)
        .WithMessage("Validation failed: SharesNumber must not be 0.");
      //More validation rules can be added 
    }

    private static bool BeAValidGuid(Guid guidValue)
      => Guid.TryParse(guidValue.ToString(), out Guid value) ? true : false;
  }
}
