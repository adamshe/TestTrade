using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.AccountCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("EF1565AC-0409-4da0-8E83-733C47D782FC")]
    public interface IComAccountCollection
    {
        /// <summary>
        /// Get the n-th account of the container.
        /// </summary>
        Account this[int index] { get; }
        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(Account value);
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.Account" /> instances.
        /// </summary>
        int Count { get; }
    }
}

