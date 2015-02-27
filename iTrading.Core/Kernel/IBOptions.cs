using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds Interactive Brokers specific connection options.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("3E28AA74-0E5B-48bb-A9D6-30021AE59361")]
    public class IBOptions : OptionsBase, IComIBOptions, ITradingSerializable
    {
        private int accountsUpdateSeconds;
        private CurrencyId baseCurrency;
        private bool checkForMarketData;
        private int clientId;
        private bool connect2RunningTws;
        private const int defaultPort = 0x1d48;
        private bool dontHandleRuntimePopups;
        private bool enablePopupHandling;
        private string host;
        private bool ignoreRth;
        private IBLogLevel logLevel;
        private int maxMarketDataStreams;
        private int maxTrysToLogin;
        private int maxWaitSecondsToLogin;
        private int popupHandlerMilliSeconds;
        private int port;
        private int requestIds;
        private int sendTimeOutMilliSeconds;
        private bool useNativeOcaOrders;
        private bool useSsl;
        private bool useUserSettings;
        private int waitMilliSecondsRequest;
        private int waitOnConnectMilliSeconds;

        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.IBOptions" /> with default settings to connect
        /// to a demo account.
        /// </summary>
        public IBOptions()
        {
            this.accountsUpdateSeconds = 5;
            this.baseCurrency = CurrencyId.UsDollar;
            this.checkForMarketData = true;
            this.clientId = 0;
            this.connect2RunningTws = true;
            this.dontHandleRuntimePopups = false;
            this.enablePopupHandling = true;
            this.host = "localhost";
            this.ignoreRth = true;
            this.logLevel = IBLogLevel.Error;
            this.popupHandlerMilliSeconds = 0x3e8;
            this.port = 0x1d48;
            this.maxMarketDataStreams = 40;
            this.maxTrysToLogin = 3;
            this.maxWaitSecondsToLogin = 60;
            this.requestIds = 0xf4240;
            this.sendTimeOutMilliSeconds = 0x3e8;
            this.useNativeOcaOrders = true;
            this.useSsl = false;
            this.useUserSettings = false;
            this.waitMilliSecondsRequest = 40;
            this.waitOnConnectMilliSeconds = 0x3e8;
            base.Provider = ProviderType.All[ProviderTypeId.InteractiveBrokers];
            this.Host = this.DefaultHost;
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Password = this.DemoPassword;
            this.Port = this.DefaultPort;
            base.User = this.DemoUser;
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public IBOptions(OptionsBase options) : base(options)
        {
            this.accountsUpdateSeconds = 5;
            this.baseCurrency = CurrencyId.UsDollar;
            this.checkForMarketData = true;
            this.clientId = 0;
            this.connect2RunningTws = true;
            this.dontHandleRuntimePopups = false;
            this.enablePopupHandling = true;
            this.host = "localhost";
            this.ignoreRth = true;
            this.logLevel = IBLogLevel.Error;
            this.popupHandlerMilliSeconds = 0x3e8;
            this.port = 0x1d48;
            this.maxMarketDataStreams = 40;
            this.maxTrysToLogin = 3;
            this.maxWaitSecondsToLogin = 60;
            this.requestIds = 0xf4240;
            this.sendTimeOutMilliSeconds = 0x3e8;
            this.useNativeOcaOrders = true;
            this.useSsl = false;
            this.useUserSettings = false;
            this.waitMilliSecondsRequest = 40;
            this.waitOnConnectMilliSeconds = 0x3e8;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public IBOptions(Bytes bytes, int version) : base(bytes, version)
        {
            this.accountsUpdateSeconds = 5;
            this.baseCurrency = CurrencyId.UsDollar;
            this.checkForMarketData = true;
            this.clientId = 0;
            this.connect2RunningTws = true;
            this.dontHandleRuntimePopups = false;
            this.enablePopupHandling = true;
            this.host = "localhost";
            this.ignoreRth = true;
            this.logLevel = IBLogLevel.Error;
            this.popupHandlerMilliSeconds = 0x3e8;
            this.port = 0x1d48;
            this.maxMarketDataStreams = 40;
            this.maxTrysToLogin = 3;
            this.maxWaitSecondsToLogin = 60;
            this.requestIds = 0xf4240;
            this.sendTimeOutMilliSeconds = 0x3e8;
            this.useNativeOcaOrders = true;
            this.useSsl = false;
            this.useUserSettings = false;
            this.waitMilliSecondsRequest = 40;
            this.waitOnConnectMilliSeconds = 0x3e8;
            base.Provider = ProviderType.All[ProviderTypeId.InteractiveBrokers];
            this.accountsUpdateSeconds = bytes.ReadInt32();
            this.baseCurrency = bytes.ReadCurrencyId();
            this.checkForMarketData = bytes.ReadBoolean();
            this.clientId = bytes.ReadInt32();
            this.connect2RunningTws = bytes.ReadBoolean();
            if (version >= 6)
            {
                this.dontHandleRuntimePopups = bytes.ReadBoolean();
            }
            this.enablePopupHandling = bytes.ReadBoolean();
            this.host = bytes.ReadString();
            this.ignoreRth = bytes.ReadBoolean();
            this.logLevel = (IBLogLevel) bytes.ReadInt32();
            this.popupHandlerMilliSeconds = bytes.ReadInt32();
            this.port = bytes.ReadInt32();
            if (version >= 4)
            {
                this.maxMarketDataStreams = bytes.ReadInt32();
            }
            this.maxTrysToLogin = bytes.ReadInt32();
            this.maxWaitSecondsToLogin = bytes.ReadInt32();
            this.requestIds = bytes.ReadInt32();
            this.sendTimeOutMilliSeconds = bytes.ReadInt32();
            this.useNativeOcaOrders = bytes.ReadBoolean();
            this.useSsl = bytes.ReadBoolean();
            this.useUserSettings = bytes.ReadBoolean();
            this.waitMilliSecondsRequest = bytes.ReadInt32();
            this.waitOnConnectMilliSeconds = bytes.ReadInt32();
        }

        /// <summary>
        /// Checks if two instances of <see cref="T:iTrading.Core.Kernel.OptionsBase" /> will establish the same connection.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public override bool SameConnection(OptionsBase options)
        {
            if (!base.SameConnection(options))
            {
                return false;
            }
            if (!(options is IBOptions))
            {
                return false;
            }
            IBOptions options2 = (IBOptions) options;
            if (options2.clientId != this.clientId)
            {
                return false;
            }
            if (options2.host != this.host)
            {
                return false;
            }
            if (options2.port != this.port)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.accountsUpdateSeconds);
            bytes.Write(this.baseCurrency);
            bytes.Write(this.checkForMarketData);
            bytes.Write(this.clientId);
            bytes.Write(this.connect2RunningTws);
            if (version >= 6)
            {
                bytes.Write(this.dontHandleRuntimePopups);
            }
            bytes.Write(this.enablePopupHandling);
            bytes.Write(this.host);
            bytes.Write(this.ignoreRth);
            bytes.Write((int) this.logLevel);
            bytes.Write(this.popupHandlerMilliSeconds);
            bytes.Write(this.port);
            if (version >= 4)
            {
                bytes.Write(this.maxMarketDataStreams);
            }
            bytes.Write(this.maxTrysToLogin);
            bytes.Write(this.maxWaitSecondsToLogin);
            bytes.Write(this.requestIds);
            bytes.Write(this.sendTimeOutMilliSeconds);
            bytes.Write(this.useNativeOcaOrders);
            bytes.Write(this.useSsl);
            bytes.Write(this.useUserSettings);
            bytes.Write(this.waitMilliSecondsRequest);
            bytes.Write(this.waitOnConnectMilliSeconds);
        }

        /// <summary>
        /// Used for friends and family accounts only. Due to a defect in IB API, FA accounts have to be updated
        /// sequentially. This is done once on startup and after every recept on an execution.
        /// TradeMagic waits <see cref="P:iTrading.Core.Kernel.IBOptions.AccountsUpdateSeconds" /> to receive account update and position
        /// update events after initial request.
        /// </summary>
        public int AccountsUpdateSeconds
        {
            get
            {
                return this.accountsUpdateSeconds;
            }
            set
            {
                this.accountsUpdateSeconds = value;
            }
        }

        /// <summary>
        /// Base currency of all accounts of this actual connection.
        /// Default = <see cref="F:iTrading.Core.Kernel.CurrencyId.UsDollar" />.
        /// </summary>
        public CurrencyId BaseCurrency
        {
            get
            {
                return this.baseCurrency;
            }
            set
            {
                this.baseCurrency = value;
            }
        }

        /// <summary>
        /// Turn check for market data on order submission / order change on/off.
        /// Default: TRUE=on.
        /// Note: If turned off, some workarounds on TWS glitches do not work.
        /// </summary>
        public bool CheckForMarketData
        {
            get
            {
                return this.checkForMarketData;
            }
            set
            {
                this.checkForMarketData = value;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.IBOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.IBOptions;
            }
        }

        /// <summary>
        /// Used to establish an Interactive Brokers connection with a specific client id. 
        /// Default = 0.
        /// </summary>
        public int ClientId
        {
            get
            {
                return this.clientId;
            }
            set
            {
                this.clientId = value;
            }
        }

        /// <summary>
        /// Connects to an already running TWS. If no TWS is running a new instance of TWS will be started.
        /// </summary>
        public bool Connect2RunningTws
        {
            get
            {
                return this.connect2RunningTws;
            }
            set
            {
                this.connect2RunningTws = value;
            }
        }

        /// <summary>
        /// Default host for establishing an Interactive Brokers connection using TradeMagic lite.
        /// Value = "localhost".
        /// </summary>
        public string DefaultHost
        {
            get
            {
                return "localhost";
            }
        }

        /// <summary>
        /// Default port for establishing an Interactive Brokers connection using TradeMagic lite.
        /// Value = 7496.
        /// </summary>
        public int DefaultPort
        {
            get
            {
                return 0x1d48;
            }
        }

        /// <summary>
        /// Password for <see cref="F:iTrading.Core.Kernel.ProviderTypeId.InteractiveBrokers" /> demo connection.
        /// </summary>
        public string DemoPassword
        {
            get
            {
                return "demouser";
            }
        }

        /// <summary>
        /// User for <see cref="F:iTrading.Core.Kernel.ProviderTypeId.InteractiveBrokers" /> demo connection.
        /// </summary>
        public string DemoUser
        {
            get
            {
                return "edemo";
            }
        }

        /// <summary>
        /// Don't handle runtime popup dialogs. Only handle the login popups. Effective only when 
        /// <see cref="P:iTrading.Core.Kernel.IBOptions.EnablePopupHandling" /> is TRUE.
        /// Default = FALSE.
        /// </summary>
        public bool DontHandleRuntimePopups
        {
            get
            {
                return this.dontHandleRuntimePopups;
            }
            set
            {
                this.dontHandleRuntimePopups = value;
            }
        }

        /// <summary>
        /// Enable/disable TWS popup handling. Default = TRUE.
        /// </summary>
        public bool EnablePopupHandling
        {
            get
            {
                return this.enablePopupHandling;
            }
            set
            {
                this.enablePopupHandling = value;
            }
        }

        /// <summary>
        /// Name of TWS host to connect to. Default = "localhost".
        /// </summary>
        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                this.host = value;
            }
        }

        /// <summary>
        /// If set to true, allows triggering of orders outside of regular trading hours.
        /// </summary>
        public bool IgnoreRth
        {
            get
            {
                return this.ignoreRth;
            }
            set
            {
                this.ignoreRth = value;
            }
        }

        /// <summary>
        /// Get/set loglevel.
        /// </summary>
        public IBLogLevel LogLevel
        {
            get
            {
                return this.logLevel;
            }
            set
            {
                this.logLevel = value;
            }
        }

        /// <summary>
        /// Max. number of market data streams. Default: 40.
        /// Please note: TWS enforced an upper limit, which is set to 40 by default. 
        /// <see cref="P:iTrading.Core.Kernel.IBOptions.MaxMarketDataStreams" /> has to be smaller/equal than this limit.
        /// </summary>
        public int MaxMarketDataStreams
        {
            get
            {
                return this.maxMarketDataStreams;
            }
            set
            {
                this.maxMarketDataStreams = value;
            }
        }

        /// <summary>
        /// Max. number of trys to login. After a timeout occured on login TradeMagic tries.
        /// Logins failed due to e.g. invalid password will not be retried.
        /// Default = 3;
        /// </summary>
        public int MaxTrysToLogin
        {
            get
            {
                return this.maxTrysToLogin;
            }
            set
            {
                this.maxTrysToLogin = value;
            }
        }

        /// <summary>
        /// Max. number of seconds to wait until TWS should be up and running.
        /// Default = 60.
        /// </summary>
        public int MaxWaitSecondsToLogin
        {
            get
            {
                return this.maxWaitSecondsToLogin;
            }
            set
            {
                this.maxWaitSecondsToLogin = value;
            }
        }

        /// <summary>
        /// The popup handler checks every <see cref="P:iTrading.Core.Kernel.IBOptions.PopupHandlerMilliSeconds" /> milliseconds if there is a popup to handle.
        /// Do not set to values larger than 30000 (= 30 seconds).
        /// Default = 1000.
        /// </summary>
        public int PopupHandlerMilliSeconds
        {
            get
            {
                return this.popupHandlerMilliSeconds;
            }
            set
            {
                this.popupHandlerMilliSeconds = value;
            }
        }

        /// <summary>
        /// TWS IP port to connect to. Default = <see cref="P:iTrading.Core.Kernel.IBOptions.DefaultPort" />.
        /// </summary>
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }

        /// <summary>
        /// Number of ids reserved for requests and orders.
        /// Default= 1000000.
        /// </summary>
        public int RequestIds
        {
            get
            {
                return this.requestIds;
            }
            set
            {
                this.requestIds = value;
            }
        }

        /// <summary>
        /// Max. waiting time int milliseonds for sending data to TWS.
        /// Default= 1000.
        /// </summary>
        public int SendTimeOutMilliSeconds
        {
            get
            {
                return this.sendTimeOutMilliSeconds;
            }
            set
            {
                this.sendTimeOutMilliSeconds = value;
            }
        }

        /// <summary>
        /// Use IB's native OCA order support. Default = TRUE.
        /// </summary>
        public bool UseNativeOcaOrders
        {
            get
            {
                return this.useNativeOcaOrders;
            }
            set
            {
                this.useNativeOcaOrders = value;
            }
        }

        /// <summary>
        /// Use SSL to establish TWS connection. Default = FALSE.
        /// </summary>
        public bool UseSsl
        {
            get
            {
                return this.useSsl;
            }
            set
            {
                this.useSsl = value;
            }
        }

        /// <summary>
        /// Use existing TWS settings. If set to FALSE, TradeMagic starts TWS with a blank workspace and appropriate
        /// connection settings. This ensures that connecting to TWS will not fail due to incorrect settings.
        /// Default = FALSE;
        /// </summary>
        public bool UseUserSettings
        {
            get
            {
                return this.useUserSettings;
            }
            set
            {
                this.useUserSettings = value;
            }
        }

        /// <summary>
        /// Min. time in milliseconds between 2 subsequent requests.
        /// TWS only accepts max 50 requests per second (=20 milliseonds per request)
        /// Default = 40.
        /// </summary>
        public int WaitMilliSecondsRequest
        {
            get
            {
                return this.waitMilliSecondsRequest;
            }
            set
            {
                this.waitMilliSecondsRequest = value;
            }
        }

        /// <summary>
        /// Time to wait before sending <see cref="E:iTrading.Core.Kernel.Connection.ConnectionStatus" /> event. The value should be 
        /// big enough to receive all open orders, order status and executions from TWS. 
        /// After receiving orders, order status and executions events TradeMagic waits 
        /// <see cref="P:iTrading.Core.Kernel.IBOptions.WaitOnConnectMilliSeconds" /> milliseconds before throwing the 
        /// <see cref="E:iTrading.Core.Kernel.Connection.ConnectionStatus" /> event.
        /// Default = 1000.
        /// </summary>
        public int WaitOnConnectMilliSeconds
        {
            get
            {
                return this.waitOnConnectMilliSeconds;
            }
            set
            {
                this.waitOnConnectMilliSeconds = value;
            }
        }
    }
}

