namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.PatsOptions" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("58D8A780-8171-4157-9B14-BA9A0DF1E794")]
    public interface IComPatsOptions
    {
        /// <summary>
        /// Application ID
        /// </summary>
        string ApplicationId { get; set; }
        /// <summary>
        /// Get/set pats diagnostic level
        /// </summary>
        int Enable { get; set; }
        /// <summary>
        /// Name of host to connect to.
        /// </summary>
        string HostAddress { get; set; }
        /// <summary>
        /// Get/set the interval for host handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.HostHandShakeTimeout" /> != 0.
        /// </summary>
        int HostHandShakeInterval { get; set; }
        /// <summary>
        /// Get/set the interval for host handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.HostHandShakeInterval" /> != 0.
        /// </summary>
        int HostHandShakeTimeout { get; set; }
        /// <summary>
        /// Number of port to connect to.
        /// </summary>
        int HostPort { get; set; }
        /// <summary>
        /// License key.
        /// </summary>
        string LicenseKey { get; set; }
        /// <summary>
        /// TRUE = free any Patsystems ressources and force Patsystems API to save it's data locally. 
        /// TradeMagic can not reconnect to Patsystems servers. TradeMagic must be terminated and restarted.
        /// FALSE = TradeMagic may reconnect to Patsystems servers. Patsystems API will not save it's data.
        /// (Default: FALSE)
        /// </summary>
        bool LogoffOnConnectionClose { get; set; }
        /// <summary>
        /// The Patsystems API occationally "stops" sending data by (a) no longer setting the appropriate price change flag and (b) not updating the Pats internal
        /// price data structure. A <see cref="T:iTrading.Core.Kernel.MarketDataEventArgs" /> event, holding an error <see cref="F:iTrading.Core.Kernel.ErrorCode.UnexpectedDataStop" />, will be sent after
        /// n seconds, if only these "void" data events have been seen. n can be set/get by this parameter. If this parameter is 0, no such event is triggered.
        /// Default = 0.
        /// Note: TradeMagic does not unsubscribe you from the affected data stream, you need to unsubscribe by yourself.
        /// </summary>
        int MaxChangeMaskZeroSecs { get; set; }
        /// <summary>
        /// New password. Set a valid new password only if you want to change your password. 
        /// The new password will be valid after successful login. Default = "" (no password change).
        /// </summary>
        string NewPassword { get; set; }
        /// <summary>
        /// Name of price host to connect to.
        /// </summary>
        string PriceAddress { get; set; }
        /// <summary>
        /// Get/set the interval for price server handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.PriceHandShakeTimeout" /> != 0.
        /// </summary>
        int PriceHandShakeInterval { get; set; }
        /// <summary>
        /// Get/set the interval for price server handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.PriceHandShakeInterval" /> != 0.
        /// </summary>
        int PriceHandShakeTimeout { get; set; }
        /// <summary>
        /// Number of price host port to connect to.
        /// </summary>
        int PricePort { get; set; }
        /// <summary>
        /// Enable/disable SuperTAS support. Default: disabled.
        /// </summary>
        bool SuperTas { get; set; }
        /// <summary>
        /// Identifies the broker or data provider. Default = <see cref="F:iTrading.Core.Kernel.ProviderTypeId.Patsystems" />
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

