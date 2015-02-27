namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OptionsBase" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("94EF4DF5-89DB-4445-B44E-4260F3318D6D")]
    public interface IComOptionsBase
    {
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
        /// Reflects difference between local time and providers server time in hours.
        /// Default = 0.
        /// </summary>
        int TimerDelayHours { get; set; }
        /// <summary>
        /// Internal for timer events in milliseconds. Default = 1000.
        /// Please note: Becomes effective on connecting. Changes while connected won't have any effect.
        /// </summary>
        int TimerIntervalMilliSeconds { get; set; }
        /// <summary>
        /// Default port for establishing a connection using TradeMagic server.
        /// Value = 8765.
        /// </summary>
        int TMDefaultPort { get; }
        /// <summary>
        /// Name of host to connect to. Default = "localhost".
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

