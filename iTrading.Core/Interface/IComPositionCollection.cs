namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.PositionCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("66CBC6F3-D32E-4b86-957D-A6254019229A")]
    public interface IComPositionCollection
    {
        /// <summary>
        /// Checks if the execution exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(Position value);
        /// <summary>
        /// Get the n-th position of the container.
        /// </summary>
        Position this[int index] { get; }
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.Position" /> instances.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get positions matching a given <see cref="T:iTrading.Core.Kernel.Symbol" />. 
        /// Please note: <see cref="P:iTrading.Core.Kernel.Symbol.Exchange" /> is not used to identify the <see cref="T:iTrading.Core.Kernel.Symbol" />.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Position FindBySymbol(Symbol symbol);
    }
}

