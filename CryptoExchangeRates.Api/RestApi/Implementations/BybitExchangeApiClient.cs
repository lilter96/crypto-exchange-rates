using Bybit.Net.Clients;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.Api.RestApi.Implementations;

public class BybitExchangeApiClient : BaseExchangeApiClient<BybitRestClient>
{
    public BybitExchangeApiClient(ILogger<BybitExchangeApiClient> logger) : base(new BybitRestClient(), logger) { }

    public override ExchangeType ExchangeType => ExchangeType.Bybit;
    public override async Task<decimal> GetLatestPriceAsync(ExchangePair pair)
    {
        var symbol = pair.GetSymbolForExchange(ExchangeType);
        
        return await RetryPolicy.ExecuteAsync(async () =>
        {
            var result = await RestClient.SpotApiV3.ExchangeData.GetPriceAsync(symbol, CancellationTokenSource.Token);

            Logger.LogInformation($"{symbol} - {result.Data.Price} - {DateTime.UtcNow:F} - {ExchangeType}");

            return result.Data.Price;
        });
    }
}