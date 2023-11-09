using CryptoExchange.Net;
using CryptoExchange.Net.Sockets;
using CryptoExchangeRates.Api.WebSockets.Abstractions;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace CryptoExchangeRates.Api.WebSockets.Implementations;

public abstract class BaseWebSocketClient<TClient> : IExchangeWebSocketClient
    where TClient : BaseSocketClient
{
    private const int MaxRetryCount = 3;
    private const int SleepTimeForRetryInSeconds = 3;
    
    protected readonly TClient SocketClient;
    protected readonly AsyncRetryPolicy RetryPolicy;
    protected readonly ILogger Logger;
    
    protected UpdateSubscription Subscription;
    protected CancellationTokenSource CancellationTokenSource = new();
    
    private bool _disposed;
    
    public abstract ExchangeType ExchangeType { get; }
    public event Action<ExchangeType, decimal, string> OnPriceUpdated;

    protected BaseWebSocketClient(TClient socketClient, ILogger logger)
    {
        SocketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
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
    
    public abstract Task ConnectAsync(ExchangePair pair);
    
    public async Task DisconnectAsync()
    {
        CancellationTokenSource.Cancel();
        
        if (Subscription != null)
        {
            await UnsubscribeAsync();
            Subscription = null;
        }

        CancellationTokenSource.Dispose();
        CancellationTokenSource = new CancellationTokenSource();
    }

    private async Task UnsubscribeAsync()
    {
        await SocketClient
            .UnsubscribeAsync(Subscription)
            .ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Logger.LogWarning($"An error occurred while trying to unsubscribe from {Subscription} - {SocketClient}.");
                    return;
                }
                
                Logger.LogInformation($"Successfully unsubscribed from {Subscription} - {SocketClient}");
            });
    }

    protected void InvokePriceUpdated(decimal lastPrice, string receivedPair)
    {
        OnPriceUpdated?.Invoke(ExchangeType, lastPrice, receivedPair);
    }
    
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
                SocketClient?.Dispose();
                CancellationTokenSource?.Dispose();
            }

            _disposed = true;
        }
    }

    ~BaseWebSocketClient()
    {
        Dispose(false);
    }
}