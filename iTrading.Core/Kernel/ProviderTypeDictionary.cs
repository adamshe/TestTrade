namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of ProviderTypes.
    /// </summary>
    [Guid("92EDEB80-A4F4-4e4b-8CDD-0B9CB9743FC8"), ClassInterface(ClassInterfaceType.None)]
    public class ProviderTypeDictionary : DictionaryBase, IComProviderTypeDictionary
    {
        internal void Add(ProviderType providerType)
        {
            base.Dictionary.Add(providerType.Id, providerType);
        }

        /// <summary>
        /// Checks if the ProviderType exists in this container.
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public bool Contains(ProviderType providerType)
        {
            return base.Dictionary.Contains(providerType.Id);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(ProviderType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an ProviderType object by its name.
        /// </summary>
        /// <param name="name"></param>
        public ProviderType Find(string name)
        {
            foreach (ProviderType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an ProviderType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProviderType this[ProviderTypeId id]
        {
            get
            {
                return (ProviderType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The ICollection of available <see cref="T:iTrading.Core.Kernel.ProviderType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.ProviderType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.ProviderTypeDictionary" />
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

