namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.StreamingRequest" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("F10D8834-F5AF-4c80-9B62-CCB81FA80AC3")]
    public interface IComStreamingRequest
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

