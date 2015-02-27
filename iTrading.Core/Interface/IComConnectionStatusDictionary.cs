namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ConnectionStatusDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("32A0B89B-3CA1-4eed-B84F-09F97BFE6669")]
    public interface IComConnectionStatusDictionary
    {
        /// <summary>
        /// Checks if the ConnectionStatus exists in this container.
        /// </summary>
        /// <param name="connectionStatus"></param>
        /// <returns></returns>
        bool Contains(ConnectionStatus connectionStatus);
        /// <summary>
        /// Retrieves an ConnectionStatus object by its name.
        /// </summary>
        /// <param name="name"></param>
        ConnectionStatus Find(string name);
        /// <summary>
        /// Retrieves an ConnectionStatus object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ConnectionStatus this[ConnectionStatusId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.ConnectionStatus" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

