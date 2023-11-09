using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Kucoin.Net.Clients;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.Api.RestApi.Implementations;

public class KucoinExchangeApiClient : BaseExchangeApiClient<KucoinRestClient>
{
    public KucoinExchangeApiClient(ILogger<KucoinExchangeApiClient> logger) : base(new KucoinRestClient(), logger) { }

    public override ExchangeType ExchangeType => ExchangeType.Kucoin;
    public override async Task<decimal> GetLatestPriceAsync(ExchangePair pair)
    {
        var symbol = pair.GetSymbolForExchange(ExchangeType);
        
        return await RetryPolicy.ExecuteAsync(async () =>
        {
            var result = await RestClient.SpotApi.ExchangeData.GetTickerAsync(symbol, CancellationTokenSource.Token);

            if (result.Data.LastPrice == null)
            {
                Logger.LogWarning($"Received null value for {symbol} from {ExchangeType} at {DateTime.UtcNow:F}");
                return 0m;
            }
            
            Logger.LogInformation($"{symbol} - {result.Data.LastPrice} - {DateTime.UtcNow:F} - {ExchangeType}");
            
            return result.Data.LastPrice.Value;
        });
    }
}