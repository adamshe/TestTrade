namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDepth" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("F5226A77-097B-4179-B5A9-ECAD38A317CB")]
    public interface IComMarketDepth
    {
        /// <summary>
        /// Collection of ask side rows. Sorted by price.
        /// </summary>
        MarketDepthRowCollection Ask { get; }
        /// <summary>
        /// Collection of bid side rows. Sorted by Price.
        /// </summary>
        MarketDepthRowCollection Bid { get; }
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
        /// Dump recorded data to a file.
        /// </summary>
        /// <param name="fromDate">Start date.</param>
        /// <param name="toDate">End date.</param>
        /// <param name="path">File path. An existing file will be overriden.</param>
        /// <returns></returns>
        void Dump(DateTime fromDate, DateTime toDate, string path);
        /// <summary>
        /// Identifies the <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Load data file to the recorder repository.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        void Load(string path);
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

