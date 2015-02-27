namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.TimeInForceDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("8D9D7300-DA0E-4d97-AF0C-F72EEC9D8B03")]
    public interface IComTimeInForceDictionary
    {
        /// <summary>
        /// Checks if the TimeInForce exists in this container.
        /// </summary>
        /// <param name="timeInForce"></param>
        /// <returns></returns>
        bool Contains(TimeInForce timeInForce);
        /// <summary>
        /// Retrieves an TimeInForce object by it's name.
        /// </summary>
        /// <param name="name"></param>
        TimeInForce Find(string name);
        /// <summary>
        /// Retrieves an TimeInForce object by its id.
        /// </summary>
        /// <param name="id"></param>
        TimeInForce this[TimeInForceId id] { get; }
        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.TimeInForce" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.TimeInForceDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

