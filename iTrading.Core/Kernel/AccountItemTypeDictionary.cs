namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of AccountItemTypes.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("EC421384-BD69-4bfb-8296-E301527A347C")]
    public class AccountItemTypeDictionary : DictionaryBase, IComAccountItemTypeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new AccountItemType supported after opening the connection.
        /// </summary>
        public event AccountItemTypeEventHandler AccountItemType;

        public void OnAccountItemTypeChange(object pSender, AccountItemTypeEventArgs pEvent)
        {
            if (AccountItemType != null)
                AccountItemType(pSender, pEvent);
        }
        internal void Add(iTrading.Core.Kernel.AccountItemType accountItemType)
        {
            base.Dictionary.Add(accountItemType.Id, accountItemType);
        }

        /// <summary>
        /// Checks if the AccountItemType exists in this container.
        /// </summary>
        /// <param name="accountItemType"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.AccountItemType accountItemType)
        {
            return base.Dictionary.Contains(accountItemType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.AccountItemType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an AccountItemType object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.AccountItemType Find(string name)
        {
            foreach (iTrading.Core.Kernel.AccountItemType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an AccountItemType object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        public iTrading.Core.Kernel.AccountItemType FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.AccountItemType type in base.Dictionary.Values)
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
        /// Retrieves an AccountItemType object by it's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.AccountItemType this[AccountItemTypeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.AccountItemType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.AccountItemTypeDictionary.AccountItemType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.AccountItemType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.AccountItemTypeDictionary" />
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

