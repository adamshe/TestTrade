namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// Represents an update operation of a bar. An instance of this class will be passed as argument 
    /// to a <see cref="T:TradeMagic.Data.BarUpdateEventArgs" />.
    /// </summary>
    [Guid("88528F5D-D5A1-4500-B84F-5C450128532E")]
    public interface IComBarUpdateEventArgs
    {
        /// <summary>
        /// Returns the TradeMagic error code of the Args. 
        /// If the asyncronous call has been successful, the value will be <seealso cref="F:iTrading.Core.Kernel.ErrorCode.NoError" />.
        /// </summary>
        ErrorCode Error { get; }
        /// <summary>
        /// Index of last bar inserted/updated. <seealso cref="P:TradeMagic.Data.BarUpdateEventArgs.Operation" />.
        /// </summary>
        int First { get; }
        /// <summary>
        /// Index of last bar inserted/updated. <seealso cref="P:TradeMagic.Data.BarUpdateEventArgs.Operation" />.
        /// </summary>
        int Last { get; }
        /// <summary>
        /// The <see cref="P:iTrading.Core.Interface.IComBarUpdateEventArgs.Quotes" /> object the <see cref="T:TradeMagic.Data.Bar" /> belongs to.
        /// </summary>
        Quotes Quotes { get; }
        /// <summary>
        /// The update operation. 
        /// <see cref="F:iTrading.Core.Kernel.Operation.Insert" />: <see cref="P:TradeMagic.Data.BarUpdateEventArgs.Last" /> holds the index of the first bar inserted,
        /// <see cref="P:TradeMagic.Data.BarUpdateEventArgs.Last" /> holds the index of the last bar inserted.
        /// <see cref="F:iTrading.Core.Kernel.Operation.Update" />: <see cref="P:TradeMagic.Data.BarUpdateEventArgs.Last" /> holds the index of the bar updated.
        /// </summary>
        iTrading.Core.Kernel.Operation Operation { get; }
        /// <summary>
        /// Request causing the Args.
        /// </summary>
        iTrading.Core.Kernel.Request Request { get; }
    }
}

