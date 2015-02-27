namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of OrderTypes.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("E1984ACE-E89F-437f-B1D0-2364C901F561")]
    public class OrderTypeDictionary : DictionaryBase, IComOrderTypeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new OrderType supported after opening the connection.
        /// </summary>
        public event iTrading.Core.Kernel.OrderTypeEventHandler OrderType;


        public void OnOrderTypeChange(Request pRequest, OrderTypeEventArgs pEvent)
        {
            if (OrderType != null)
            OrderType(pRequest, pEvent);
        }

        internal void Add(iTrading.Core.Kernel.OrderType orderType)
        {
            base.Dictionary.Add(orderType.Id, orderType);
        }

        /// <summary>
        /// Checks if the OrderType exists in this container.
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.OrderType orderType)
        {
            return base.Dictionary.Contains(orderType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.OrderType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an OrderType object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.OrderType Find(string name)
        {
            foreach (iTrading.Core.Kernel.OrderType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an OrderType object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.OrderType FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.OrderType type in base.Dictionary.Values)
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
        /// Retrieves an OrderType object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.OrderType this[OrderTypeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.OrderType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.OrderTypeDictionary.OrderType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.OrderType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.OrderTypeDictionary" />
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

