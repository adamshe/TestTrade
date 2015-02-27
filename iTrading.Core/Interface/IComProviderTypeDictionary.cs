namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ProviderTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("ED0D7F80-CFCE-4f7e-AC00-0772FD0110E1")]
    public interface IComProviderTypeDictionary
    {
        /// <summary>
        /// Checks if the ProviderType exists in this container.
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        bool Contains(ProviderType providerType);
        /// <summary>
        /// Retrieves an ProviderType object by its name.
        /// </summary>
        /// <param name="name"></param>
        ProviderType Find(string name);
        /// <summary>
        /// Retrieves an ProviderType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProviderType this[ProviderTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.ProviderType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

