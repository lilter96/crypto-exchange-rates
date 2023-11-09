using CryptoExchangeRates.Api.RestApi;
using CryptoExchangeRates.Api.RestApi.Abstractions;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace CryptoExchangeRates.Tests;

public class ExchangesRestClientManagerTests
{
    [Fact]
    public async Task GetLastPricesForAllExchanges_ReturnsCorrectPrices()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExchangesRestClientManager>>();
        var mockApiClient = new Mock<IExchangeApiClient>();
        const ExchangeType exchangeType = ExchangeType.Binance;
        const decimal expectedPrice = 100m;
        var pair = new ExchangePair("BTC", "USD");

        mockApiClient.Setup(x => x.GetLatestPriceAsync(pair)).ReturnsAsync(expectedPrice);
        mockApiClient.Setup(x => x.ExchangeType).Returns(exchangeType);

        var manager = new ExchangesRestClientManager(new[] { mockApiClient.Object }, mockLogger.Object);

        // Act
        var result = await manager.GetLastPricesForAllExchanges(pair);

        // Assert
        Assert.True(result.ContainsKey(exchangeType));
        Assert.Equal(expectedPrice, result[exchangeType]);
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                ((Func<It.IsAnyType, Exception, string>)It.IsAny<object>())!),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetLastPricesForAllExchanges_LogsErrorWhenExceptionOccurs()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExchangesRestClientManager>>();
        var mockApiClient = new Mock<IExchangeApiClient>();
        var pair = new ExchangePair("BTC", "USD");

        mockApiClient.Setup(x => x.GetLatestPriceAsync(pair)).ThrowsAsync(new InvalidOperationException());
        mockApiClient.Setup(x => x.ExchangeType).Returns(ExchangeType.Binance);

        var manager = new ExchangesRestClientManager(new[] { mockApiClient.Object }, mockLogger.Object);

        // Act
        var prices = await manager.GetLastPricesForAllExchanges(pair);

        // Assert
        Assert.True(!prices.Any());
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ),
            Times.AtLeastOnce);
    }
}