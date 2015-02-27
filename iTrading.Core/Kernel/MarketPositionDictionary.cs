namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of MarketPositions.
    /// </summary>
    [Guid("7EB4B038-E005-4aef-8AB5-8D38B8AE4EE9"), ClassInterface(ClassInterfaceType.None)]
    public class MarketPositionDictionary : DictionaryBase, IComMarketPositionDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new MarketPosition supported after opening the connection.
        /// </summary>
        public event MarketPositionEventHandler MarketPosition;

        public void OnMarketPositionChange (object pSender, MarketPositionEventArgs pEvent)
        {
            if (MarketPosition != null)
                MarketPosition(pSender, pEvent);
        }

        internal void Add(iTrading.Core.Kernel.MarketPosition marketPosition)
        {
            base.Dictionary.Add(marketPosition.Id, marketPosition);
        }

        /// <summary>
        /// Checks if the MarketPosition exists in this container.
        /// </summary>
        /// <param name="marketPosition"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.MarketPosition marketPosition)
        {
            return base.Dictionary.Contains(marketPosition.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.MarketPosition[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an MarketPosition object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.MarketPosition Find(string name)
        {
            foreach (iTrading.Core.Kernel.MarketPosition position in base.Dictionary.Values)
            {
                if (position.Name == name)
                {
                    return position;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an MarketPosition object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.MarketPosition FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.MarketPosition position in base.Dictionary.Values)
                {
                    if (position.MapId == mapId)
                    {
                        return position;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an MarketPosition object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.MarketPosition this[MarketPositionId id]
        {
            get
            {
                return (iTrading.Core.Kernel.MarketPosition) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.MarketPositionDictionary.MarketPosition" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.MarketPosition" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.MarketPositionDictionary" />
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

