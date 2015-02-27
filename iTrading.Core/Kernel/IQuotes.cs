namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Data;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface IQuotes
    {
        /// <summary>
        /// Request quotes.
        /// </summary>
        /// <param name="quotes"></param>
        void Request(Quotes quotes);
    }
}

