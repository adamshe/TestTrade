namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.LookupPolicyDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("94FE0ABE-BC3F-40aa-9650-0D676D2F33C7")]
    public interface IComLookupPolicyDictionary
    {
        /// <summary>
        /// Checks if the LookupPolicy exists in this container.
        /// </summary>
        /// <param name="lookupPolicy"></param>
        /// <returns></returns>
        bool Contains(LookupPolicy lookupPolicy);
        /// <summary>
        /// Retrieves an ModeType object by its name.
        /// </summary>
        /// <param name="name"></param>
        LookupPolicy Find(string name);
        /// <summary>
        /// Retrieves an LookupPolicy object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LookupPolicy this[LookupPolicyId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.LookupPolicy" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

