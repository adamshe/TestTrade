namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Request" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("0578F207-6644-453d-8364-D9BAC3121CB2")]
    public interface IComRequest
    {
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
    }
}

