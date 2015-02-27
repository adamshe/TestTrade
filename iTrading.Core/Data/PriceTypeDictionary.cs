namespace iTrading.Core.Data
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of PriceTypes.
    /// </summary>
    [Guid("D969032E-32C0-4cfa-AD22-582474440848"), ClassInterface(ClassInterfaceType.None)]
    public class PriceTypeDictionary : DictionaryBase, IComPriceTypeDictionary
    {
        internal void Add(PriceType priceType)
        {
            base.Dictionary.Add(priceType.Id, priceType);
        }

        /// <summary>
        /// Checks if the PriceType exists in this container.
        /// </summary>
        /// <param name="priceType"></param>
        /// <returns></returns>
        public bool Contains(PriceType priceType)
        {
            return base.Dictionary.Contains(priceType.Id);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(PriceType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an PriceType object by its name.
        /// </summary>
        /// <param name="name"></param>
        public PriceType Find(string name)
        {
            foreach (PriceType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an PriceType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PriceType this[PriceTypeId id]
        {
            get
            {
                return (PriceType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The ICollection of available <see cref="T:TradeMagic.Data.PriceType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:TradeMagic.Data.PriceType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:TradeMagic.Data.PriceTypeDictionary" />
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

