namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of Exchanges.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("B78C1808-2891-4479-9082-222516E322EA")]
    public class ExchangeDictionary : DictionaryBase, IComExchangeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new exchange supported after opening the connection.
        /// </summary>
        public event ExchangeEventHandler Exchange;

        /// <summary>
        /// Add an exchange.
        /// </summary>
        /// <param name="exchange"></param>
        public void Add(iTrading.Core.Kernel.Exchange exchange)
        {
            base.Dictionary.Add(exchange.Id, exchange);
        }

        public void OnExchangeChange(Object pSender, ExchangeEventArgs pEvent)
        {
            if (Exchange != null)
                Exchange(pSender, pEvent);
        }
        /// <summary>
        /// Checks if the Exchange exists in this container.
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.Exchange exchange)
        {
            return base.Dictionary.Contains(exchange.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.Exchange[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an exchange object by its name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.Exchange Find(string name)
        {
            foreach (iTrading.Core.Kernel.Exchange exchange in base.Dictionary.Values)
            {
                if (exchange.Name == name)
                {
                    return exchange;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an exchange object, by its broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        public iTrading.Core.Kernel.Exchange FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.Exchange exchange in base.Dictionary.Values)
                {
                    if (exchange.MapId == mapId)
                    {
                        return exchange;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an exchange object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.Exchange this[ExchangeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.Exchange) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.ExchangeDictionary.Exchange" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.Exchange" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.ExchangeDictionary" />
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

