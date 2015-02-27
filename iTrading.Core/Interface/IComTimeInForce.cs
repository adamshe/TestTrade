namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.TimeInForce" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("758D5ACF-4DA5-44d0-BDE5-74266E7EA655")]
    public interface IComTimeInForce
    {
        /// <summary>
        /// The TradeMagic id.
        /// </summary>
        TimeInForceId Id { get; }
        /// <summary>
        /// The broker dependent id.
        /// </summary>
        string MapId { get; }
        /// <summary>
        /// </summary>
        string Name { get; }
    }
}

