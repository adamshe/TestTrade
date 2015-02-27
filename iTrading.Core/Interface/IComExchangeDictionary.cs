namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ExchangeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("2984A0DC-0A98-455a-9BA2-2947B0AFA098")]
    public interface IComExchangeDictionary
    {
        /// <summary>
        /// Checks if the Exchange exists in this container.
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        bool Contains(Exchange exchange);
        /// <summary>
        /// Retrieves an exchange object by its name.
        /// </summary>
        /// <param name="name"></param>
        Exchange Find(string name);
        /// <summary>
        /// Retrieves an exchange object by its id.
        /// </summary>
        /// <param name="id"></param>
        Exchange this[ExchangeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.Exchange" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

