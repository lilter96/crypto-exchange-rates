using CryptoExchangeRates.Api.WebSockets.Abstractions;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.Api.WebSockets;

public class ExchangesWebSocketManager
{
    private readonly IEnumerable<IExchangeWebSocketClient> _webSockets;
    private readonly ILogger<ExchangesWebSocketManager> _logger;

    public event Action<ExchangeType, decimal, string> OnPriceUpdated;

    public ExchangesWebSocketManager(
        IEnumerable<IExchangeWebSocketClient> webSockets,
        ILogger<ExchangesWebSocketManager> logger)
    {
        _webSockets = webSockets ?? throw new ArgumentNullException(nameof(webSockets));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task StartListening(ExchangePair pair)
    {
        AddPriceUpdatedSubscriptions();
        var connectTasks = _webSockets.Select(webSocket => webSocket.ConnectAsync(pair));
        await Task.WhenAll(connectTasks);
        _logger.LogInformation("All web sockets are now listening.");
    }

    public async Task StopListening()
    {
        var connectTasks = _webSockets.Select(webSocket => webSocket.DisconnectAsync());
        await Task.WhenAll(connectTasks);
        _logger.LogInformation("All web sockets have been disconnected.");
        
        ClearPriceUpdatedSubscriptions();
        _logger.LogInformation("All price updated subscriptions have been cleared.");
    }
    
    private void HandlePriceUpdated(ExchangeType name, decimal price, string pair)
    {
        OnPriceUpdated?.Invoke(name, price, pair);
    }

    private void AddPriceUpdatedSubscriptions()
    {
        foreach (var ws in _webSockets)
        {
            ws.OnPriceUpdated += HandlePriceUpdated;
        }
    }
    
    private void ClearPriceUpdatedSubscriptions()
    {
        foreach (var ws in _webSockets)
        {
            ws.OnPriceUpdated -= HandlePriceUpdated;
        }
    }
}