using Bitget.Net.Clients;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.Api.RestApi.Implementations;

public class BitGetExchangeApiClient : BaseExchangeApiClient<BitgetRestClient>
{
    private const string BitGetApiPostfix = "_spbl";
    public BitGetExchangeApiClient(ILogger<BitGetExchangeApiClient> logger) : base(new BitgetRestClient(), logger) { }

    public override ExchangeType ExchangeType => ExchangeType.BitGet;
    public override async Task<decimal> GetLatestPriceAsync(ExchangePair pair)
    {
        var symbol = pair.GetSymbolForExchange(ExchangeType);
        
        return await RetryPolicy.ExecuteAsync(async () =>
        {
            var result = await RestClient.SpotApi.ExchangeData.GetTickerAsync(
                symbol + BitGetApiPostfix, 
                ct: CancellationTokenSource.Token);

            Logger.LogInformation($"{symbol} - {result.Data.ClosePrice} - {DateTime.UtcNow:F} - {ExchangeType}");

            return result.Data.ClosePrice;
        });
    }
}