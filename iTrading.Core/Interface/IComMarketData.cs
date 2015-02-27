namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketData" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("90BDD031-A5BC-44eb-85C3-13D0BA0B7997")]
    public interface IComMarketData
    {
        /// <summary>
        /// Get last ask change event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no ask change event has ben seen yet.
        /// </summary>
        MarketDataEventArgs Ask { get; }
        /// <summary>
        /// Get last bid change event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no bid change event has ben seen yet.
        /// </summary>
        MarketDataEventArgs Bid { get; }
        /// <summary>
        /// Stop recording of market data.
        /// </summary>
        void CancelRecorder();
        /// <summary>
        /// <see cref="T:iTrading.Core.Kernel.Connection" /> where market data is requested from.
        /// </summary>
        iTrading.Core.Kernel.Connection Connection { get; }
        /// <summary>
        /// <see cref="P:iTrading.Core.Kernel.Request.CustomLink" />. This property may be used to attach any object to the request.
        /// </summary>
        object CustomLink { get; set; }
        /// <summary>
        /// Identifies the <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Get last daily-high event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-high event has ben seen yet.
        /// </summary>
        MarketDataEventArgs DailyHigh { get; }
        /// <summary>
        /// Get last daily-low event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-low event has ben seen yet.
        /// </summary>
        MarketDataEventArgs DailyLow { get; }
        /// <summary>
        /// Get last daily-volume event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-volume event has ben seen yet.
        /// </summary>
        MarketDataEventArgs DailyVolume { get; }
        /// <summary>
        /// Dump recorded data to a file.
        /// </summary>
        /// <param name="fromDate">Start date.</param>
        /// <param name="toDate">End date.</param>
        /// <param name="path">File path. An existing file will be overriden.</param>
        /// <returns></returns>
        void Dump(DateTime fromDate, DateTime toDate, string path);
        /// <summary>
        /// Get last price change event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no price change event has ben seen yet.
        /// </summary>
        MarketDataEventArgs Last { get; }
        /// <summary>
        /// Get last previous day close event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no previous day close event has ben seen yet.
        /// </summary>
        MarketDataEventArgs LastClose { get; }
        /// <summary>
        /// Load data file to the recorder repository.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        void Load(string path);
        /// <summary>
        /// Get daily opening event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-opening event has ben seen yet.
        /// </summary>
        MarketDataEventArgs Opening { get; }
        /// <summary>
        /// Start market data recorder.
        /// </summary>
        void StartRecorder();
        /// <summary>
        /// Symbol where market data is requested for.
        /// </summary>
        iTrading.Core.Kernel.Symbol Symbol { get; }
    }
}

