namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface IAdapter
    {
        /// <summary>
        /// Clear any symbol lookup cache.
        /// </summary>
        void Clear();
        /// <summary>
        /// Establish connection.
        /// </summary>
        void Connect();
        /// <summary>
        /// Destroys connection.
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Lookup symbol at the broker's system.
        /// </summary>
        /// <param name="symbolTemplate"></param>
        void SymbolLookup(Symbol symbolTemplate);
    }
}

