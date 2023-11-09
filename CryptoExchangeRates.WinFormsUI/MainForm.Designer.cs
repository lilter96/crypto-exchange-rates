namespace CryptoExchangeRates.WinFormsUI;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        binancePriceLabel = new Label();
        comboBoxPairs = new ComboBox();
        bybitPriceLabel = new Label();
        binanceLabel = new Label();
        bybitLabel = new Label();
        bitGetLabel = new Label();
        bitGetPriceLabel = new Label();
        kucoinLabel = new Label();
        kucoinPriceLabel = new Label();
        webSocketRadioButton = new RadioButton();
        restApiRadioButton = new RadioButton();
        exchangePairLabel = new Label();
        connectionLabel = new Label();
        timeIntervalLabel = new Label();
        timeIntervalTextBox = new TextBox();
        timeIntervalSubmitButton = new Button();
        SuspendLayout();
        // 
        // binancePriceLabel
        // 
        binancePriceLabel.AutoSize = true;
        binancePriceLabel.BorderStyle = BorderStyle.FixedSingle;
        binancePriceLabel.Font = new Font("Comic Sans MS", 26.25F, FontStyle.Regular, GraphicsUnit.Point);
        binancePriceLabel.Location = new Point(241, 87);
        binancePriceLabel.Name = "binancePriceLabel";
        binancePriceLabel.Size = new Size(128, 51);
        binancePriceLabel.TabIndex = 0;
        binancePriceLabel.Text = "Wait...";
        binancePriceLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // comboBoxPairs
        // 
        comboBoxPairs.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBoxPairs.Location = new Point(241, 35);
        comboBoxPairs.Name = "comboBoxPairs";
        comboBoxPairs.Size = new Size(128, 23);
        comboBoxPairs.TabIndex = 1;
        // 
        // bybitPriceLabel
        // 
        bybitPriceLabel.AutoSize = true;
        bybitPriceLabel.BorderStyle = BorderStyle.FixedSingle;
        bybitPriceLabel.Font = new Font("Comic Sans MS", 26.25F, FontStyle.Regular, GraphicsUnit.Point);
        bybitPriceLabel.Location = new Point(241, 156);
        bybitPriceLabel.Name = "bybitPriceLabel";
        bybitPriceLabel.Size = new Size(128, 51);
        bybitPriceLabel.TabIndex = 2;
        bybitPriceLabel.Text = "Wait...";
        bybitPriceLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // binanceLabel
        // 
        binanceLabel.AutoSize = true;
        binanceLabel.Font = new Font("Comic Sans MS", 40F, FontStyle.Regular, GraphicsUnit.Point);
        binanceLabel.Location = new Point(12, 67);
        binanceLabel.Name = "binanceLabel";
        binanceLabel.Size = new Size(223, 76);
        binanceLabel.TabIndex = 3;
        binanceLabel.Text = "Binance";
        // 
        // bybitLabel
        // 
        bybitLabel.AutoSize = true;
        bybitLabel.Font = new Font("Comic Sans MS", 40F, FontStyle.Regular, GraphicsUnit.Point);
        bybitLabel.Location = new Point(12, 136);
        bybitLabel.Name = "bybitLabel";
        bybitLabel.Size = new Size(166, 76);
        bybitLabel.TabIndex = 4;
        bybitLabel.Text = "Bybit";
        // 
        // bitGetLabel
        // 
        bitGetLabel.AutoSize = true;
        bitGetLabel.Font = new Font("Comic Sans MS", 40F, FontStyle.Regular, GraphicsUnit.Point);
        bitGetLabel.Location = new Point(12, 212);
        bitGetLabel.Name = "bitGetLabel";
        bitGetLabel.Size = new Size(198, 76);
        bitGetLabel.TabIndex = 6;
        bitGetLabel.Text = "BitGet";
        // 
        // bitGetPriceLabel
        // 
        bitGetPriceLabel.AutoSize = true;
        bitGetPriceLabel.BorderStyle = BorderStyle.FixedSingle;
        bitGetPriceLabel.Font = new Font("Comic Sans MS", 26.25F, FontStyle.Regular, GraphicsUnit.Point);
        bitGetPriceLabel.Location = new Point(241, 237);
        bitGetPriceLabel.Name = "bitGetPriceLabel";
        bitGetPriceLabel.Size = new Size(128, 51);
        bitGetPriceLabel.TabIndex = 5;
        bitGetPriceLabel.Text = "Wait...";
        bitGetPriceLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // kucoinLabel
        // 
        kucoinLabel.AutoSize = true;
        kucoinLabel.Font = new Font("Comic Sans MS", 40F, FontStyle.Regular, GraphicsUnit.Point);
        kucoinLabel.Location = new Point(18, 288);
        kucoinLabel.Name = "kucoinLabel";
        kucoinLabel.Size = new Size(192, 76);
        kucoinLabel.TabIndex = 8;
        kucoinLabel.Text = "Kucoin";
        // 
        // kucoinPriceLabel
        // 
        kucoinPriceLabel.AutoSize = true;
        kucoinPriceLabel.BorderStyle = BorderStyle.FixedSingle;
        kucoinPriceLabel.Font = new Font("Comic Sans MS", 26.25F, FontStyle.Regular, GraphicsUnit.Point);
        kucoinPriceLabel.Location = new Point(241, 308);
        kucoinPriceLabel.Name = "kucoinPriceLabel";
        kucoinPriceLabel.Size = new Size(128, 51);
        kucoinPriceLabel.TabIndex = 7;
        kucoinPriceLabel.Text = "Wait...";
        kucoinPriceLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // webSocketRadioButton
        // 
        webSocketRadioButton.AutoSize = true;
        webSocketRadioButton.Checked = true;
        webSocketRadioButton.Location = new Point(490, 51);
        webSocketRadioButton.Name = "webSocketRadioButton";
        webSocketRadioButton.Size = new Size(84, 19);
        webSocketRadioButton.TabIndex = 9;
        webSocketRadioButton.TabStop = true;
        webSocketRadioButton.Text = "WebSocket";
        webSocketRadioButton.UseVisualStyleBackColor = true;
        // 
        // restApiRadioButton
        // 
        restApiRadioButton.AutoSize = true;
        restApiRadioButton.Location = new Point(580, 51);
        restApiRadioButton.Name = "restApiRadioButton";
        restApiRadioButton.Size = new Size(71, 19);
        restApiRadioButton.TabIndex = 10;
        restApiRadioButton.TabStop = true;
        restApiRadioButton.Text = "REST API";
        restApiRadioButton.UseVisualStyleBackColor = true;
        // 
        // exchangePairLabel
        // 
        exchangePairLabel.AutoSize = true;
        exchangePairLabel.Font = new Font("Comic Sans MS", 20F, FontStyle.Regular, GraphicsUnit.Point);
        exchangePairLabel.Location = new Point(28, 20);
        exchangePairLabel.Name = "exchangePairLabel";
        exchangePairLabel.Size = new Size(193, 38);
        exchangePairLabel.TabIndex = 11;
        exchangePairLabel.Text = "Exchange pair";
        // 
        // connectionLabel
        // 
        connectionLabel.AutoSize = true;
        connectionLabel.Font = new Font("Comic Sans MS", 10F, FontStyle.Regular, GraphicsUnit.Point);
        connectionLabel.Location = new Point(505, 20);
        connectionLabel.Name = "connectionLabel";
        connectionLabel.Size = new Size(125, 19);
        connectionLabel.TabIndex = 12;
        connectionLabel.Text = "Choose connection";
        // 
        // timeIntervalLabel
        // 
        timeIntervalLabel.AutoSize = true;
        timeIntervalLabel.Font = new Font("Comic Sans MS", 10F, FontStyle.Regular, GraphicsUnit.Point);
        timeIntervalLabel.Location = new Point(505, 181);
        timeIntervalLabel.Name = "timeIntervalLabel";
        timeIntervalLabel.Size = new Size(140, 19);
        timeIntervalLabel.TabIndex = 13;
        timeIntervalLabel.Text = "Time interval (secs)";
        // 
        // timeIntervalTextBox
        // 
        timeIntervalTextBox.Location = new Point(545, 217);
        timeIntervalTextBox.Name = "timeIntervalTextBox";
        timeIntervalTextBox.Size = new Size(52, 23);
        timeIntervalTextBox.TabIndex = 14;
        // 
        // timeIntervalSubmitButton
        // 
        timeIntervalSubmitButton.Location = new Point(536, 260);
        timeIntervalSubmitButton.Name = "timeIntervalSubmitButton";
        timeIntervalSubmitButton.Size = new Size(75, 23);
        timeIntervalSubmitButton.TabIndex = 15;
        timeIntervalSubmitButton.Text = "Submit";
        timeIntervalSubmitButton.UseVisualStyleBackColor = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(704, 401);
        Controls.Add(timeIntervalSubmitButton);
        Controls.Add(timeIntervalTextBox);
        Controls.Add(timeIntervalLabel);
        Controls.Add(connectionLabel);
        Controls.Add(exchangePairLabel);
        Controls.Add(restApiRadioButton);
        Controls.Add(webSocketRadioButton);
        Controls.Add(kucoinLabel);
        Controls.Add(kucoinPriceLabel);
        Controls.Add(bitGetLabel);
        Controls.Add(bitGetPriceLabel);
        Controls.Add(bybitLabel);
        Controls.Add(binanceLabel);
        Controls.Add(bybitPriceLabel);
        Controls.Add(comboBoxPairs);
        Controls.Add(binancePriceLabel);
        Name = "MainForm";
        Text = "CryptoExchangeRates";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label binancePriceLabel;
    private ComboBox comboBoxPairs;
    private Label bybitPriceLabel;
    private Label binanceLabel;
    private Label bybitLabel;
    private Label bitGetLabel;
    private Label bitGetPriceLabel;
    private Label kucoinLabel;
    private Label kucoinPriceLabel;
    private RadioButton webSocketRadioButton;
    private RadioButton restApiRadioButton;
    private Label exchangePairLabel;
    private Label connectionLabel;
    private Label timeIntervalLabel;
    private TextBox timeIntervalTextBox;
    private Button timeIntervalSubmitButton;
}