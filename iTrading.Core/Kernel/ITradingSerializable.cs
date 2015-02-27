using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface ITradingSerializable
    {
        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        void Serialize(Bytes bytes, int version);

        /// <summary>
        /// Return <see cref="P:iTrading.Core.Kernel.ITradingSerializable.ClassId" /> or current object.
        /// </summary>
        iTrading.Core.Kernel.ClassId ClassId { get; }

        /// <summary>
        /// Version number.
        /// </summary>
        int Version { get; }
    }
}

