namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ProviderType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("DD283F32-6A2E-40f7-8758-25C64177C853")]
    public interface IComProviderType
    {
        /// <summary>
        /// The TradeMagic id of the ProviderType.
        /// </summary>
        ProviderTypeId Id { get; }
        /// <summary>
        /// The name of the ProviderType.
        /// </summary>
        string Name { get; }
    }
}

