namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of Currencys.
    /// </summary>
    [Guid("0825029E-96F2-435b-A23A-1D5ED86F1F12"), ClassInterface(ClassInterfaceType.None)]
    public class CurrencyDictionary : DictionaryBase, IComCurrencyDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new Currency supported after opening the connection.
        /// </summary>
        public event CurrencyEventHandler Currency;

        public void OnCurrencyChange (object pSender, CurrencyEventArgs pEvent)
        {
            if (Currency != null )
                Currency(pSender, pEvent);
        }
        internal void Add(iTrading.Core.Kernel.Currency currency)
        {
            base.Dictionary.Add(currency.Id, currency);
        }

        /// <summary>
        /// Checks if the Currency exists in this container.
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.Currency currency)
        {
            return base.Dictionary.Contains(currency.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.Currency[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an Currency object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.Currency Find(string name)
        {
            foreach (iTrading.Core.Kernel.Currency currency in base.Dictionary.Values)
            {
                if (currency.Name == name)
                {
                    return currency;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an Currency object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        public iTrading.Core.Kernel.Currency FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.Currency currency in base.Dictionary.Values)
                {
                    if (currency.MapId == mapId)
                    {
                        return currency;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an Currency object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.Currency this[CurrencyId id]
        {
            get
            {
                return (iTrading.Core.Kernel.Currency) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.CurrencyDictionary.Currency" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.Currency" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.CurrencyDictionary" />
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

