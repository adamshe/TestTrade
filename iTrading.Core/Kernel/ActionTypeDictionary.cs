namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of ActionTypes.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("D0059EB7-2A6C-4a02-9735-007B29414596")]
    public class ActionTypeDictionary : DictionaryBase, IComActionTypeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new ActionType supported after opening the connection.
        /// </summary>
        public event iTrading.Core.Kernel.ActionTypeEventHandler ActionType;

        public void OnActionTypeChange (object pSender, ActionTypeEventArgs pEvent)
        {
            if (ActionType != null)
                ActionType(pSender, pEvent);

        }
        internal void Add(iTrading.Core.Kernel.ActionType actionType)
        {
            base.Dictionary.Add(actionType.Id, actionType);
        }

        /// <summary>
        /// Checks if the ActionType exists in this container.
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.ActionType actionType)
        {
            return base.Dictionary.Contains(actionType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.ActionType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an ActionType object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.ActionType Find(string name)
        {
            foreach (iTrading.Core.Kernel.ActionType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an ActionType object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        public iTrading.Core.Kernel.ActionType FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.ActionType type in base.Dictionary.Values)
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
        /// Retrieves an ActionType object by it's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.ActionType this[ActionTypeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.ActionType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.ActionTypeDictionary.ActionType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.ActionType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.ActionTypeDictionary" />
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

