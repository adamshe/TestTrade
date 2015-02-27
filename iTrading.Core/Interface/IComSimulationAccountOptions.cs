namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.SimulationAccountOptions" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("E712EA5C-E93F-4ad1-A47F-778DC4A493A8")]
    public interface IComSimulationAccountOptions
    {
        /// <summary>
        /// One way delay on internet communication to the exchange in milliseconds. Must be greater than 0.
        /// Default = 150.
        /// </summary>
        int DelayCommunication { get; set; }
        /// <summary>
        /// Delay on exchange processing an order -, order change - or order cancel request. Must be greater than 0.
        /// Default = 500.
        /// </summary>
        int DelayExchange { get; set; }
        /// <summary>
        /// Initial cash value of account. Must be greater than 0.
        /// Default = 100.000.
        /// </summary>
        double InitialCashValue { get; set; }
        /// <summary>
        /// Margin on this account.
        /// Default = 0.5.
        /// </summary>
        double Margin { get; set; }
        /// <summary>
        /// Maintenance margin on this account.
        /// Default = 0.3.
        /// </summary>
        double MaintenanceMargin { get; set; }
        /// <summary>
        /// Version number.
        /// </summary>
        int Version { get; }
        /// <summary>
        /// A running market data stream is mandatory for the simulator. Although the simulator turns on any non-running market data stream on order placement for the required
        /// symbol, data might not be available right away. On setting this option, the simulator can be advised to wait until finaly the first market data item was seen. 
        /// Default = 0. Note: Setting this option will delay order submission until the timer expired or the first market data was seen.
        /// </summary>
        int WaitForMarketDataSeconds { get; set; }
    }
}

