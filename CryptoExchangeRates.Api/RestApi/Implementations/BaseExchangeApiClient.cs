using CryptoExchange.Net;
using CryptoExchangeRates.Api.RestApi.Abstractions;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace CryptoExchangeRates.Api.RestApi.Implementations;

public abstract class BaseExchangeApiClient<TClient> : IExchangeApiClient
    where TClient : BaseRestClient
{
    private const int MaxRetryCount = 3;
    private const int SleepTimeForRetryInSeconds = 3;

    protected readonly TClient RestClient;
    protected readonly AsyncRetryPolicy RetryPolicy;
    protected readonly ILogger Logger;

    protected CancellationTokenSource CancellationTokenSource = new();
    
    private bool _disposed;

    public abstract ExchangeType ExchangeType { get; }
    
    protected BaseExchangeApiClient(TClient socketClient, ILogger logger)
    {
        RestClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
        Logger = logger;
        
        RetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                MaxRetryCount,
                _ => TimeSpan.FromSeconds(SleepTimeForRetryInSeconds),
                (exception, timeSpan, retryCount, _) =>
                {
                    Logger.LogWarning($"Retry {retryCount} after {timeSpan} seconds due to {exception.Message}");
                });
    }
    
    public abstract Task<decimal> GetLatestPriceAsync(ExchangePair pair);

    public void Dispose()
    {
        Logger.LogInformation($"Disposing {typeof(TClient).Name} - {DateTime.UtcNow:F}");
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                RestClient?.Dispose();
                CancellationTokenSource?.Dispose();
            }

            _disposed = true;
        }
    }

    ~BaseExchangeApiClient()
    {
        Dispose(false);
    }
}