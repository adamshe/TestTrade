namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ConnectionStatus" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("52011807-B5AB-48b1-A39D-6315A0DFFA8A")]
    public interface IComConnectionStatus
    {
        /// <summary>
        /// The TradeMagic id of the ConnectionStatus.
        /// </summary>
        ConnectionStatusId Id { get; }
        /// <summary>
        /// The name of the ConnectionStatus.
        /// </summary>
        string Name { get; }
    }
}

