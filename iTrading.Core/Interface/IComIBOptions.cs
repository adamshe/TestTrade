namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.IBOptions" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("46E3ED3E-83DA-457c-B05D-503085CD6A72")]
    public interface IComIBOptions
    {
        /// <summary>
        /// Used for friends and family accounts only. Due to a defect in IB API, FA accounts have to be updated
        /// sequentially. This is done once on startup and after every recept on an execution.
        /// TradeMagic waits <see cref="P:iTrading.Core.Kernel.IBOptions.AccountsUpdateSeconds" /> to receive account update and position
        /// update events after initial request.
        /// </summary>
        int AccountsUpdateSeconds { get; set; }
        /// <summary>
        /// Base currency of all accounts of this actual connection.
        /// Default = <see cref="F:iTrading.Core.Kernel.CurrencyId.UsDollar" />.
        /// </summary>
        CurrencyId BaseCurrency { get; set; }
        /// <summary>
        /// Turn of check for market data on order submission / order change.
        /// </summary>
        bool CheckForMarketData { get; set; }
        /// <summary>
        /// Used to establish an Interactive Brokers connection with a specific client id. 
        /// Default = 0.
        /// </summary>
        int ClientId { get; set; }
        /// <summary>
        /// Connects to an already running TWS. If no TWS is running a new instance of TWS will be started.
        /// </summary>
        bool Connect2RunningTws { get; set; }
        /// <summary>
        /// Default host for establishing an Interactive Brokers connection using TradeMagic lite.
        /// Value = "localhost".
        /// </summary>
        string DefaultHost { get; }
        /// <summary>
        /// Default port for establishing an Interactive Brokers connection using TradeMagic lite.
        /// Value = 7496.
        /// </summary>
        int DefaultPort { get; }
        /// <summary>
        /// Password for <see cref="F:iTrading.Core.Kernel.ProviderTypeId.InteractiveBrokers" /> demo connection.
        /// </summary>
        string DemoPassword { get; }
        /// <summary>
        /// User for <see cref="F:iTrading.Core.Kernel.ProviderTypeId.InteractiveBrokers" /> demo connection.
        /// </summary>
        string DemoUser { get; }
        /// <summary>
        /// Don't handle runtime popup dialogs. Only handle the login popups. Effective only when 
        /// <see cref="P:iTrading.Core.Kernel.IBOptions.EnablePopupHandling" /> is TRUE.
        /// Default = FALSE.
        /// </summary>
        bool DontHandleRuntimePopups { get; set; }
        /// <summary>
        /// Enable/disable TWS popup handling. Default = TRUE.
        /// </summary>
        bool EnablePopupHandling { get; set; }
        /// <summary>
        /// Name of TWS host to connect to. Default = "localhost".
        /// </summary>
        string Host { get; set; }
        /// <summary>
        /// If set to true, allows triggering of orders outside of regular trading hours.
        /// </summary>
        bool IgnoreRth { get; set; }
        /// <summary>
        /// Get/set loglevel.
        /// </summary>
        IBLogLevel LogLevel { get; set; }
        /// <summary>
        /// Max. number of market data streams. Default: 40.
        /// Please note: TWS enforced an upper limit, which is set to 40 by default. 
        /// <see cref="P:iTrading.Core.Kernel.IBOptions.MaxMarketDataStreams" /> has to be smaller/equal than this limit.
        /// </summary>
        int MaxMarketDataStreams { get; set; }
        /// <summary>
        /// Max. number of trys to login. After a timeout occured on login TradeMagic tries.
        /// Logins failed due to e.g. invalid password will not be retried.
        /// Default = 3;
        /// </summary>
        int MaxTrysToLogin { get; set; }
        /// <summary>
        /// Max. number of seconds to wait until TWS should be up and running.
        /// Default = 60.
        /// </summary>
        int MaxWaitSecondsToLogin { get; set; }
        /// <summary>
        /// The popup handler checks every <see cref="P:iTrading.Core.Kernel.IBOptions.PopupHandlerMilliSeconds" /> milliseconds if there is a popup to handle.
        /// Do not set to values larger than 30000 (= 30 seconds).
        /// Default = 1000.
        /// </summary>
        int PopupHandlerMilliSeconds { get; set; }
        /// <summary>
        /// Number of TWS IP port to connect to. Default = <see cref="P:iTrading.Core.Kernel.IBOptions.DefaultPort" />.
        /// </summary>
        int Port { get; set; }
        /// <summary>
        /// Number of ids reserved for requests and orders.
        /// Default= 1000000.
        /// </summary>
        int RequestIds { get; set; }
        /// <summary>
        /// Max. waiting time int milliseonds for sending data to TWS.
        /// Default= 1000.
        /// </summary>
        int SendTimeOutMilliSeconds { get; set; }
        /// <summary>
        /// Use SSL to establish TWS connection.
        /// </summary>
        bool UseSsl { get; set; }
        /// <summary>
        /// Use existing TWS settings. If set to FALSE, TradeMagic starts TWS with a blank workspace and appropriate
        /// connection settings. This ensures that connecting to TWS will not fail due to incorrect settings.
        /// Default = FALSE;
        /// </summary>
        bool UseUserSettings { get; set; }
        /// <summary>
        /// Min. time in milliseconds between 2 subsequent requests.
        /// TWS only accepts max 50 requests per second (=20 milliseonds per request)
        /// Default = 40.
        /// </summary>
        int WaitMilliSecondsRequest { get; set; }
        /// <summary>
        /// Time to wait before sending <see cref="E:iTrading.Core.Kernel.Connection.ConnectionStatus" /> event. The value should be 
        /// big enough to receive all open orders, order status and executions from TWS. 
        /// After receiving orders, order status and executions events TradeMagic waits 
        /// <see cref="P:iTrading.Core.Kernel.IBOptions.WaitOnConnectMilliSeconds" /> milliseconds before throwing the 
        /// <see cref="E:iTrading.Core.Kernel.Connection.ConnectionStatus" /> event.
        /// Default = 1000.
        /// </summary>
        int WaitOnConnectMilliSeconds { get; set; }
        /// <summary>
        /// Identifies the broker or data provider. Default = <see cref="F:iTrading.Core.Kernel.ProviderTypeId.InteractiveBrokers" />
        /// </summary>
        ProviderType Provider { get; set; }
        /// <summary>
        /// TradeMagic maintained order history this many days back. Executions and orders themselves will not be
        /// deleted.
        /// Default = 30. Maximum = 1000.
        /// </summary>
        int HistoryMaintained { get; set; }
        /// <summary>
        /// Identifies the license.
        /// </summary>
        LicenseType License { get; }
        /// <summary>
        /// Identifies the mode.
        /// </summary>
        ModeType Mode { get; set; }
        /// <summary>
        /// Password to connect to broker or data provider. Default = "".
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// Get/set max gap (in minutes) between subsequent write calls to store the recorded data. Value must be greater than 0.
        /// </summary>
        int RecorderWriteMinutes { get; set; }
        /// <summary>
        /// TRUE: Connect to TradeMagic server. FALSE: Establish connection in current application scope.
        /// </summary>
        bool RunAtServer { get; set; }
        /// <summary>
        /// Save <see cref="T:iTrading.Core.Kernel.OptionsBase" /> object to "Config.xml" file.
        /// </summary>
        void Save();
        /// <summary>
        /// This property only is effective if the broker does not support native OCA orders.
        /// TRUE: Partial fills reduce the order quantity of the paired OCA orders, when OCA handling is simulated.
        /// FALSE: OCA simulation works on complete fills only.
        /// Default: TRUE.
        /// </summary>
        bool SimOcaPartialFills { get; set; }
        /// <summary>
        /// Reflects difference between local time and brokers server time in hours.
        /// Default = 0.
        /// </summary>
        int TimerDelayHours { get; set; }
        /// <summary>
        /// Default port for establishing a connection using TradeMagic server.
        /// Value = 8765.
        /// </summary>
        int TMDefaultPort { get; }
        /// <summary>
        /// Name of host to connect to. Do not enter an IP address string, but the hostname itself. Default = "localhost".
        /// </summary>
        string TMHost { get; set; }
        /// <summary>
        /// Number of port to connect to. Default = <see cref="P:iTrading.Core.Kernel.OptionsBase.TMDefaultPort" />
        /// </summary>
        int TMPort { get; set; }
        /// <summary>
        /// User id to connect to broker or data provider. Default = "".
        /// </summary>
        string User { get; set; }
        /// <summary>
        /// Vendor code to identify TradeMagic third party vendor. Default = "".
        /// </summary>
        string VendorCode { get; }
    }
}

