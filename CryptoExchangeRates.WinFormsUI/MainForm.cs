using CryptoExchangeRates.Api.RestApi;
using CryptoExchangeRates.Api.WebSockets;
using CryptoExchangeRates.Core;
using CryptoExchangeRates.Core.Domain;
using Microsoft.Extensions.Logging;

namespace CryptoExchangeRates.WinFormsUI;

public partial class MainForm : Form
{
    private readonly ExchangesWebSocketManager _webSocketManager;
    private readonly ExchangesRestClientManager _restClientManager;
    private readonly Dictionary<ExchangeType, decimal> _latestPrices = new();
    private readonly ILogger _logger;

    private Dictionary<ExchangeType, Label> _exchangeLabels;
    private System.Windows.Forms.Timer _uiUpdateTimer;
    private ExchangePair _selectedExchangePair;

    private const int InitialUiTimerIntervalMs = 5000;
    public MainForm(
        ExchangesWebSocketManager webSocketManager,
        ExchangesRestClientManager restClientManager,
        ILogger logger)
    {
        _webSocketManager = webSocketManager ?? throw new ArgumentNullException(nameof(webSocketManager));
        _restClientManager = restClientManager ?? throw new ArgumentNullException(nameof(restClientManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        InitializeComponent();
        InitializeCustomComponents();
    }

    private void InitializeCustomComponents()
    {
        Load += async (sender, e) => await InitializeWebSocketConnection();
        FormClosing += async (sender, e) => await _webSocketManager.StopListening();

        InitializeExchangeLabels();
        InitializeTimer();
        InitializeComboBox();
        InitializeRadioButtons();
        InitializeTimeIntervalTextBox();
        InitializeTimeIntervalSubmitButton();

        _webSocketManager.OnPriceUpdated += WebSocketManager_OnPriceUpdated;
    }

    private void InitializeExchangeLabels()
    {
        _exchangeLabels = new Dictionary<ExchangeType, Label>
        {
            { ExchangeType.Binance, binancePriceLabel },
            { ExchangeType.Bybit, bybitPriceLabel },
            { ExchangeType.BitGet, bitGetPriceLabel },
            { ExchangeType.Kucoin, kucoinPriceLabel }
        };
    }

    private void InitializeTimer()
    {
        _uiUpdateTimer = new System.Windows.Forms.Timer
        {
            Interval = InitialUiTimerIntervalMs,
            Enabled = true
        };
        _uiUpdateTimer.Tick += async (_, _) => await UiUpdateTimer_Tick();
    }

    private void InitializeComboBox()
    {
        comboBoxPairs.Items.AddRange(new object[] { "BTCUSDT", "ETHUSDT", "XRPUSDT" });
        comboBoxPairs.SelectedIndex = 0;
        _selectedExchangePair = ExchangePair.GetExchangePairFromSymbol("BTCUSDT");
        comboBoxPairs.SelectedIndexChanged += async (sender, e) => await ComboBoxPairs_SelectedIndexChanged();
    }

    private void InitializeRadioButtons()
    {
        webSocketRadioButton.CheckedChanged += async (_, _) => await ToggleConnectionMode();
        restApiRadioButton.CheckedChanged += async (_, _) => await ToggleConnectionMode();
    }

    private async Task InitializeWebSocketConnection()
    {
        await _webSocketManager.StartListening(_selectedExchangePair);
    }

    private void InitializeTimeIntervalTextBox()
    {
        timeIntervalTextBox.Text = (_uiUpdateTimer.Interval / 1000).ToString();
        timeIntervalTextBox.KeyPress += TimeIntervalTextBox_KeyPress;
    }
    private void InitializeTimeIntervalSubmitButton()
    {
        timeIntervalSubmitButton.Click += TimeIntervalSubmitButton_Click;
    }

    private void WebSocketManager_OnPriceUpdated(ExchangeType exchangeType, decimal price, string symbol)
    {
        if (_selectedExchangePair.GetSymbolForExchange(exchangeType) != symbol)
        {
            _logger.LogInformation($"Mismatch symbol for {exchangeType}: {symbol} at {DateTime.UtcNow:f}");
            return;
        }

        _latestPrices[exchangeType] = price;
        _logger.LogInformation($"{symbol} - {price} - {DateTime.UtcNow:f} - {exchangeType}");
    }

    private async Task UiUpdateTimer_Tick()
    {
        try
        {
            if (restApiRadioButton.Checked)
            {
                var prices = await _restClientManager.GetLastPricesForAllExchanges(_selectedExchangePair);
                foreach (var (exchangeType, price) in prices)
                {
                    _latestPrices[exchangeType] = price;
                    UpdateLabel(exchangeType, price);
                }

                return;
            }

            foreach (var exchange in _exchangeLabels.Keys)
            {
                if (_latestPrices.TryGetValue(exchange, out var price))
                {
                    UpdateLabel(exchange, price);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during UI update: {ex.Message}");
        }
    }

    private void UpdateLabel(ExchangeType exchangeType, decimal price)
    {
        if (_exchangeLabels.TryGetValue(exchangeType, out var label))
        {
            label.Text = price.ToString("0.0000##");
        }
    }

    private async Task ComboBoxPairs_SelectedIndexChanged()
    {
        var selectedSymbol = comboBoxPairs.SelectedItem.ToString();
        _selectedExchangePair = ExchangePair.GetExchangePairFromSymbol(selectedSymbol!);
        if (webSocketRadioButton.Checked)
        {
            await _webSocketManager.StopListening();
            await _webSocketManager.StartListening(_selectedExchangePair);
        }
    }

    private async Task ToggleConnectionMode()
    {
        restApiRadioButton.Checked = !webSocketRadioButton.Checked;

        if (webSocketRadioButton.Checked)
        {
            await _webSocketManager.StartListening(_selectedExchangePair);
        }
        else
        {
            await _webSocketManager.StopListening();
        }
    }

    private void TimeIntervalTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void TimeIntervalSubmitButton_Click(object sender, EventArgs e)
    {
        if (int.TryParse(timeIntervalTextBox.Text, out int seconds) && seconds > 0)
        {
            _uiUpdateTimer.Interval = seconds * 1000;
            _logger.LogInformation($"Update interval set to {seconds} seconds.");
        }
        else
        {
            MessageBox.Show("Please enter a valid number of seconds (greater than 0).");
            timeIntervalTextBox.SelectAll();
        }
    }
}