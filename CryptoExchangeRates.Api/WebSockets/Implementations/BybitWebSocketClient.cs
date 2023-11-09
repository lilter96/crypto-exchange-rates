using Bybit.Net.Clients;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.Api.WebSockets.Implementations;

public class BybitWebSocketClient : BaseWebSocketClient<BybitSocketClient>
{
    public BybitWebSocketClient(ILogger<BybitWebSocketClient> logger) : base(new BybitSocketClient(), logger) { }

    public override ExchangeType ExchangeType => ExchangeType.Bybit;

    public override async Task ConnectAsync(ExchangePair pair)
    {
        var symbol = pair.GetSymbolForExchange(ExchangeType);
        
        await RetryPolicy.ExecuteAsync(async () =>
        {
            Logger.LogInformation("Disconnecting any existing WebSocket connection before starting a new one for symbol {Symbol}.", symbol);
            await DisconnectAsync();
            Logger.LogInformation("Subscribing to ticker updates for symbol {Symbol}.", symbol);

            var result = await SocketClient.SpotV3Api.SubscribeToTickerUpdatesAsync(
                symbol,
                dataEvent =>
                {
                    if (!CancellationTokenSource.Token.IsCancellationRequested)
                    {
                        Logger.LogDebug("Received ticker update for symbol {Symbol}: {LastPrice}", dataEvent.Data.Symbol, dataEvent.Data.LastPrice);
                        InvokePriceUpdated(dataEvent.Data.LastPrice, dataEvent.Data.Symbol);
                    }
                },
                CancellationTokenSource.Token);

            if (!result.Success)
            {
                var errorMessage = $"Failed to connect to {ExchangeType} web socket for symbol {symbol}: {result.Error}";
                Logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            Subscription = result.Data;
            Logger.LogInformation($"Successfully subscribed to {ExchangeType} WebSocket for symbol {{Symbol}}.", symbol);
        });
    }
}