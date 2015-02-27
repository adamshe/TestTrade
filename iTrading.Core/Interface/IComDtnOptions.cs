namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.DtnOptions" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("F3EAFDCD-A454-4842-9748-89EEDCED78DA")]
    public interface IComDtnOptions
    {
        /// <summary>
        /// Name of DTN datafeed host to connect to. Do not enter an IP address string, but the hostname itself. 
        /// Default = "localhost".
        /// </summary>
        string Host { get; set; }
        /// <summary>
        /// Retrieve news n days back.
        /// Default = 1.
        /// </summary>
        int NewsDaysBack { get; set; }
        /// <summary>
        /// Check for new every n seconds.
        /// Default = 60.
        /// </summary>
        int NewsIntervalSeconds { get; set; }
        /// <summary>
        /// Retrieve max # of news back.
        /// Default= 10.
        /// </summary>
        int NewsMaxBack { get; set; }
        /// <summary>
        /// The port for streaming data. Default: 5009
        /// </summary>
        int PortFeed { get; set; }
        /// <summary>
        /// The port for level 2 data. Default: 9200
        /// </summary>
        int PortLevel2 { get; set; }
        /// <summary>
        /// The port for data lookup. Default: 9100
        /// </summary>
        int PortLookup { get; set; }
        /// <summary>
        /// Identifies the broker or data provider. Default = <see cref="F:iTrading.Core.Kernel.ProviderTypeId.Dtn" />
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
        /// Reflects difference between local time and providers server time in hours.
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

