using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;

namespace CryptoExchangeRates.Api.WebSockets.Abstractions;

public interface IExchangeWebSocketClient : IDisposable
{
    event Action<ExchangeType, decimal, string> OnPriceUpdated;

    ExchangeType ExchangeType { get; }
    
    Task ConnectAsync(ExchangePair newPair);
    
    Task DisconnectAsync();
}