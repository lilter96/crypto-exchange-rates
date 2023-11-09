using Binance.Net.Clients;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.Api.RestApi.Implementations;

public class BinanceExchangeApiClient : BaseExchangeApiClient<BinanceRestClient>
{
    public BinanceExchangeApiClient(ILogger<BinanceExchangeApiClient> logger) : base(new BinanceRestClient(), logger) { }

    public override ExchangeType ExchangeType => ExchangeType.Binance;
    public override async Task<decimal> GetLatestPriceAsync(ExchangePair pair)
    {
        var symbol = pair.GetSymbolForExchange(ExchangeType);

        return await RetryPolicy.ExecuteAsync(async () =>
        {
            var result = await RestClient.SpotApi.ExchangeData.GetPriceAsync(symbol, CancellationTokenSource.Token);
            
            Logger.LogInformation($"{symbol} - {result.Data.Price} - {DateTime.UtcNow:F} - {ExchangeType}");
            
            return result.Data.Price;
        });
    }
}