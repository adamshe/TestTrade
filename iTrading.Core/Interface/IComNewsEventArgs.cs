namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.NewsEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("E7A062CA-7988-484f-BEAA-15A016427ACD")]
    public interface IComNewsEventArgs
    {
        /// <summary>
        /// The new head line.
        /// </summary>
        string HeadLine { get; }
        /// <summary>
        /// Identifies the news item.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Identifies the news item type.
        /// </summary>
        NewsItemType ItemType { get; }
        /// <summary>
        /// Text of news item.
        /// </summary>
        string Text { get; }
        /// <summary>
        /// Time of news item.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// Returns the TradeMagic error code of the Args. 
        /// If the asyncronous call has been successful, the value will be <seealso cref="F:iTrading.Core.Kernel.ErrorCode.NoError" />.
        /// </summary>
        ErrorCode Error { get; }
        /// <summary>
        /// Native error code of underlying broker or data provider.
        /// </summary>
        string NativeError { get; }
        /// <summary>
        /// Request causing the Args.
        /// </summary>
        iTrading.Core.Kernel.Request Request { get; }
    }
}

