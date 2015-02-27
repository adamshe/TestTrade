namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of OrderStates.
    /// </summary>
    [Guid("E8D23024-27D5-40d5-9882-A461E75C2EE1"), ClassInterface(ClassInterfaceType.None)]
    public class OrderStateDictionary : DictionaryBase, IComOrderStateDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new OrderState supported after opening the connection.
        /// </summary>
        public event iTrading.Core.Kernel.OrderStateEventHandler OrderState;

        public void OnOrderStateChange (object pSender, OrderStateEventArgs pEvent)
        {
            if (OrderState != null)
                OrderState(pSender, pEvent);
        }
        internal void Add(iTrading.Core.Kernel.OrderState orderState)
        {
            base.Dictionary.Add(orderState.Id, orderState);
        }

        /// <summary>
        /// Checks if the OrderState exists in this container.
        /// </summary>
        /// <param name="orderState"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.OrderState orderState)
        {
            return base.Dictionary.Contains(orderState.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.OrderState[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an OrderState object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.OrderState Find(string name)
        {
            foreach (iTrading.Core.Kernel.OrderState state in base.Dictionary.Values)
            {
                if (state.Name == name)
                {
                    return state;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an OrderState object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.OrderState FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.OrderState state in base.Dictionary.Values)
                {
                    if (state.MapId == mapId)
                    {
                        return state;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an OrderState object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.OrderState this[OrderStateId id]
        {
            get
            {
                return (iTrading.Core.Kernel.OrderState) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.OrderStateDictionary.OrderState" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.OrderState" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.OrderStateDictionary" />
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

