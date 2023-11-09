# Crypto Exchange Rates

A Windows Forms application that provides real-time cryptocurrency prices from multiple exchanges via WebSocket and REST API.

## Features

- Live cryptocurrency prices from exchanges like Binance, Bybit, BitGet, and Kucoin.
- User can select to receive updates via WebSocket or REST API.
- Prices are updated every 5 seconds when using REST API.
- Support for cryptocurrency pairs such as BTC/USDT, ETH/USDT, and XRP/USDT.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

What things you need to install the software and how to install them:

- .NET 7 or later

### Installing

A step by step series of examples that tell you how to get a development environment running:

1. Clone the repository to your local machine.
git clone https://github.com/yourusername/crypto-exchange-rates.git
2. Open the solution file `CryptoExchangeRates.sln` in Visual Studio.
3. Restore NuGet packages.
4. Build the solution.

## Usage

Run the application from Visual Studio by pressing `F5` or `Start Debugging`. Select your preferred update method (WebSocket or REST API) and the cryptocurrency pair from the drop-down menu.

## Built With

- [Windows Forms](https://devblogs.microsoft.com/dotnet/winforms-enhancements-in-dotnet-7/) - The GUI framework
- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - Programming language used


## Authors

- **Terentiy Gatsukov** - [lilter96](https://github.com/lilter96)
