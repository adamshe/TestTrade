namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.LookupPolicy" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("F0E664C0-9D26-41d0-8114-62CB28565A54")]
    public interface IComLookupPolicy
    {
        /// <summary>
        /// The TradeMagic id of the LookupPolicy.
        /// </summary>
        LookupPolicyId Id { get; }
        /// <summary>
        /// The name of the LookupPolicy.
        /// </summary>
        string Name { get; }
    }
}

