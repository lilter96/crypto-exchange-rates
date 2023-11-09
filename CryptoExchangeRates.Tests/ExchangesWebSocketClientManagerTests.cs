using CryptoExchangeRates.Api.WebSockets;
using CryptoExchangeRates.Api.WebSockets.Abstractions;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace CryptoExchangeRates.Tests;

public class ExchangesWebSocketClientManagerTests
{
    private readonly Mock<IExchangeWebSocketClient> _mockWebSocket;
    private readonly Mock<ILogger<ExchangesWebSocketManager>> _mockLogger;
    private readonly ExchangesWebSocketManager _manager;
    private readonly ExchangePair _testPair;

    public ExchangesWebSocketClientManagerTests()
    {
        _mockWebSocket = new Mock<IExchangeWebSocketClient>();
        _mockLogger = new Mock<ILogger<ExchangesWebSocketManager>>();
        var webSockets = new List<IExchangeWebSocketClient> { _mockWebSocket.Object };
        _manager = new ExchangesWebSocketManager(webSockets, _mockLogger.Object);
        _testPair = new ExchangePair("BTC", "USD");
    }

    [Fact]
    public async Task StartListening_StartsWebSocketsAndLogsInformation()
    {
        // Arrange
        _mockWebSocket.Setup(ws => ws.ConnectAsync(_testPair)).Returns(Task.CompletedTask);

        // Act
        await _manager.StartListening(_testPair);

        // Assert
        _mockWebSocket.Verify(ws => ws.ConnectAsync(_testPair), Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task StopListening_StopsWebSocketsClearsSubscriptionsAndLogsInformation()
    {
        // Arrange
        _mockWebSocket.Setup(ws => ws.DisconnectAsync()).Returns(Task.CompletedTask);

        // Act
        await _manager.StopListening();

        // Assert
        _mockWebSocket.Verify(ws => ws.DisconnectAsync(), Times.Once);
        // Verify that log information was called twice (disconnecting and clearing subscriptions)
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Exactly(2));
    }

    [Fact]
    public void HandlePriceUpdated_RaisesOnPriceUpdatedEvent()
    {
        // Arrange
        var eventRaised = false;
        _manager.OnPriceUpdated += (_, _, _) => { eventRaised = true; };

        // Act
        _manager.GetType().GetMethod("HandlePriceUpdated", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
               .Invoke(_manager, new object[] { ExchangeType.Binance, 100m, "BTCUSD" });

        // Assert
        Assert.True(eventRaised);
    }
}