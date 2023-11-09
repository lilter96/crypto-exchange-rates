using System.Collections.Concurrent;
using CryptoExchangeRates.Api.RestApi.Abstractions;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.Api.RestApi;

public class ExchangesRestClientManager
{
    private readonly IEnumerable<IExchangeApiClient> _apiClients;
    private readonly ILogger<ExchangesRestClientManager> _logger;

    public ExchangesRestClientManager(
        IEnumerable<IExchangeApiClient> apiClients, 
        ILogger<ExchangesRestClientManager> logger)
    {
        _apiClients = apiClients ?? throw new ArgumentNullException(nameof(apiClients));;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Dictionary<ExchangeType, decimal>> GetLastPricesForAllExchanges(ExchangePair pair)
    {
        var result = new ConcurrentDictionary<ExchangeType, decimal>();
        var connectTasks = _apiClients.Select(apiClient =>
        {
            return apiClient.GetLatestPriceAsync(pair).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    result[apiClient.ExchangeType] = task.Result;
                    _logger.LogInformation("Received latest price for {Exchange} - {Symbol}: {Price}", apiClient.ExchangeType, pair, task.Result);
                }
                else if (task.IsFaulted)
                {
                    _logger.LogError(task.Exception, "Error getting latest price for {Exchange} - {Symbol}", apiClient.ExchangeType, pair);
                }
                else if (task.IsCanceled)
                {
                    _logger.LogWarning("Getting latest price for {Exchange} - {Symbol} was canceled", apiClient.ExchangeType, pair);
                }
            });
        });
        
        await Task.WhenAll(connectTasks);
        
        if (result.Count != _apiClients.Count())
        {
            _logger.LogWarning("Latest prices for some exchanges could not be retrieved.");
        }

        return result.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}