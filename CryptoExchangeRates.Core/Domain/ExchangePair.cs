namespace CryptoExchangeRates.Core.Domain;

public class ExchangePair
{
    public string BaseCurrency { get; }
    public string QuoteCurrency { get; }
    
    public decimal? CurrentPrice { get; set; }

    //public decimal MinimumTradeSize { get; set; }
    
    //public int PricePrecision { get; set; }
    
    private static readonly Dictionary<ExchangeType, Func<string, string, string>> SymbolFormatters =
        new()
        {
            { ExchangeType.Binance, (baseCurrency, quoteCurrency) => $"{baseCurrency}{quoteCurrency}" },
            { ExchangeType.Bybit, (baseCurrency, quoteCurrency) => $"{baseCurrency}{quoteCurrency}" },
            { ExchangeType.BitGet, (baseCurrency, quoteCurrency) => $"{baseCurrency}{quoteCurrency}" },
            { ExchangeType.Kucoin, (baseCurrency, quoteCurrency) => $"{baseCurrency}-{quoteCurrency}" },
        };

    private const string DefaultQuoteCurrency = "USDT";
    
    public ExchangePair(string baseCurrency, string quoteCurrency)
    {
        BaseCurrency = baseCurrency;
        QuoteCurrency = quoteCurrency;
    }
    
    public string GetSymbolForExchange(ExchangeType exchangeType)
    {
        if (SymbolFormatters.TryGetValue(exchangeType, out var formatter))
        {
            return formatter(BaseCurrency, QuoteCurrency);
        }

        throw new NotSupportedException($"Exchange type '{exchangeType}' is not supported.");
    }

    public static ExchangePair GetExchangePairFromSymbol(string symbol)
    {
        if (symbol[^4..] != DefaultQuoteCurrency)
        {
            throw new NotSupportedException($"Symbol QuoteCurrency isn't {DefaultQuoteCurrency}.");
        }

        var baseCurrency = string.Join(string.Empty, symbol[..^4].TakeWhile(char.IsLetterOrDigit));

        return new ExchangePair(baseCurrency, DefaultQuoteCurrency);
    }
}