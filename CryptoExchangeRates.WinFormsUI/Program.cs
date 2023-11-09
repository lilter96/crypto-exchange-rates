using CryptoExchangeRates.Api.RestApi;
using CryptoExchangeRates.Api.RestApi.Abstractions;
using CryptoExchangeRates.Api.RestApi.Implementations;
using CryptoExchangeRates.Api.WebSockets;
using CryptoExchangeRates.Api.WebSockets.Abstractions;
using CryptoExchangeRates.Api.WebSockets.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.WinFormsUI;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var host = CreateHostBuilder().Build();

        var mainForm = host.Services.GetRequiredService<MainForm>();
        
        Application.Run(mainForm);
    }

    private static IHostBuilder CreateHostBuilder()
    {
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = factory.CreateLogger("Program");
        return Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) => {
                services.AddSingleton<IExchangeWebSocketClient, BinanceWebSocketClient>();
                services.AddSingleton<IExchangeWebSocketClient, BybitWebSocketClient>();
                services.AddSingleton<IExchangeWebSocketClient, BitGetWebSocketClient>();
                services.AddSingleton<IExchangeWebSocketClient, KucoinWebSocketClient>();
                services.AddSingleton<ExchangesWebSocketManager>();

                services.AddSingleton<IExchangeApiClient, BinanceExchangeApiClient>();
                services.AddSingleton<IExchangeApiClient, BybitExchangeApiClient>();
                services.AddSingleton<IExchangeApiClient, BitGetExchangeApiClient>();
                services.AddSingleton<IExchangeApiClient, KucoinExchangeApiClient>();
                services.AddSingleton<ExchangesRestClientManager>();

                services.AddSingleton(x => logger);
                services.AddTransient<MainForm>();
            });
    }
}