namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of SymbolLookupPolicies.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("FC21650B-E845-42be-AE46-489810142981")]
    public class LookupPolicyDictionary : DictionaryBase, IComLookupPolicyDictionary
    {
        internal void Add(LookupPolicy lookupPolicy)
        {
            base.Dictionary.Add(lookupPolicy.Id, lookupPolicy);
        }

        /// <summary>
        /// Checks if the LookupPolicy exists in this container.
        /// </summary>
        /// <param name="lookupPolicy"></param>
        /// <returns></returns>
        public bool Contains(LookupPolicy lookupPolicy)
        {
            return base.Dictionary.Contains(lookupPolicy.Id);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(LookupPolicy[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an LookupPolicy object by its name.
        /// </summary>
        /// <param name="name"></param>
        public LookupPolicy Find(string name)
        {
            foreach (LookupPolicy policy in base.Dictionary.Values)
            {
                if (policy.Name == name)
                {
                    return policy;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an LookupPolicy object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LookupPolicy this[LookupPolicyId id]
        {
            get
            {
                return (LookupPolicy) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.LookupPolicy" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.LookupPolicy" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.LookupPolicyDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        public iTrading.Core.Kernel.ValuesCollection ValuesCollection
        {
            get
            {
                return new iTrading.Core.Kernel.ValuesCollection(base.Dictionary.Values);
            }
        }
    }
}

