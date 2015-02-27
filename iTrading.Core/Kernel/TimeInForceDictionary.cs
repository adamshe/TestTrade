namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of TimeInForce values.
    /// </summary>
    [Guid("82039EA0-66C4-4f33-8FE6-0C431193D660"), ClassInterface(ClassInterfaceType.None)]
    public class TimeInForceDictionary : DictionaryBase, IComTimeInForceDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new TimeInForce supported after opening the connection.
        /// </summary>
        public event TimeInForceEventHandler TimeInForce;

        public void OnTimeInForceChange (object pSender, TimeInForceEventArgs pEvent)
        {
            if (TimeInForce!=null)
            {
                TimeInForce(pSender, pEvent);
            }
        }
        internal void Add(iTrading.Core.Kernel.TimeInForce timeInForce)
        {
            base.Dictionary.Add(timeInForce.Id, timeInForce);
        }

        /// <summary>
        /// Checks if the TimeInForce exists in this container.
        /// </summary>
        /// <param name="timeInForce"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.TimeInForce timeInForce)
        {
            return base.Dictionary.Contains(timeInForce.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.TimeInForce[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an TimeInForce object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.TimeInForce Find(string name)
        {
            foreach (iTrading.Core.Kernel.TimeInForce force in base.Dictionary.Values)
            {
                if (force.Name == name)
                {
                    return force;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an TimeInForce object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.TimeInForce FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.TimeInForce force in base.Dictionary.Values)
                {
                    if (force.MapId == mapId)
                    {
                        return force;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an TimeInForce object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.TimeInForce this[TimeInForceId id]
        {
            get
            {
                return (iTrading.Core.Kernel.TimeInForce) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.TimeInForceDictionary.TimeInForce" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.TimeInForce" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.TimeInForceDictionary" />
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

