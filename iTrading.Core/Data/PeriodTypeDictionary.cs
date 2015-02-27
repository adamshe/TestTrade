namespace iTrading.Core.Data
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of quotes size types.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("55B87CC8-387F-4197-BDB6-7AB31EBB8472")]
    public class PeriodTypeDictionary : DictionaryBase, IComPeriodTypeDictionary
    {
        internal void Add(PeriodType periodType)
        {
            base.Dictionary.Add(periodType.Id, periodType);
        }

        /// <summary>
        /// Checks if the PeriodType exists in this container.
        /// </summary>
        /// <param name="periodType"></param>
        /// <returns></returns>
        public bool Contains(PeriodType periodType)
        {
            return base.Dictionary.Contains(periodType.Id);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(PeriodType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an <see cref="T:TradeMagic.Data.PeriodType" /> object by its name.
        /// </summary>
        /// <param name="name"></param>
        public PeriodType Find(string name)
        {
            foreach (PeriodType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an <see cref="T:TradeMagic.Data.PeriodType" /> object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PeriodType this[PeriodTypeId id]
        {
            get
            {
                return (PeriodType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="T:TradeMagic.Data.PeriodType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:TradeMagic.Data.PeriodType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:TradeMagic.Data.PeriodTypeDictionary" />
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

