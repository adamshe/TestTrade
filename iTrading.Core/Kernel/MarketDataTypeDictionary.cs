namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of MarketDataTypes.
    /// </summary>
    [Guid("53F6E0CE-A54D-453f-B5E2-96C6019DE919"), ClassInterface(ClassInterfaceType.None)]
    public class MarketDataTypeDictionary : DictionaryBase, IComMarketDataTypeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new MarketDataType supported after opening the connection.
        /// </summary>
        public event MarketDataTypeEventHandler MarketDataType;

        public void OnMarketDataTypeChange (object pSender, MarketDataTypeEventArgs pEvent)
        {
            if (MarketDataType != null)
                MarketDataType(pSender, pEvent);
        }
        internal void Add(iTrading.Core.Kernel.MarketDataType marketDataType)
        {
            base.Dictionary.Add(marketDataType.Id, marketDataType);
        }

        /// <summary>
        /// Checks if the MarketDataType exists in this container.
        /// </summary>
        /// <param name="marketDataType"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.MarketDataType marketDataType)
        {
            return base.Dictionary.Contains(marketDataType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.MarketDataType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an MarketDataType object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.MarketDataType Find(string name)
        {
            foreach (iTrading.Core.Kernel.MarketDataType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an MarketDataType object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.MarketDataType FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.MarketDataType type in base.Dictionary.Values)
                {
                    if (type.MapId == mapId)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an MarketDataType object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.MarketDataType this[MarketDataTypeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.MarketDataType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.MarketDataTypeDictionary.MarketDataType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.MarketDataType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.MarketDataTypeDictionary" />
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

