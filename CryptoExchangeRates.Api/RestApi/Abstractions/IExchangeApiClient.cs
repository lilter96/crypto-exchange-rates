using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;

namespace CryptoExchangeRates.Api.RestApi.Abstractions;

public interface IExchangeApiClient : IDisposable
{
    ExchangeType ExchangeType { get; }

    Task<decimal> GetLatestPriceAsync(ExchangePair pair);
}