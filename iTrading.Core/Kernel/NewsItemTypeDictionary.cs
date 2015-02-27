namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of NewsItemTypes.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("6056ED68-EBF1-4644-A212-9F33F99BB7F2")]
    public class NewsItemTypeDictionary : DictionaryBase, IComNewsItemTypeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new NewsItemType supported after opening the connection.
        /// </summary>
        public event NewsItemTypeEventHandler NewsItemType;

        public void OnNewsItemTypeChange (object pSender, NewsItemTypeEventArgs pEvent)
        {
            if (NewsItemType != null)
                NewsItemType(pSender, pEvent);
        }
        internal void Add(iTrading.Core.Kernel.NewsItemType newsItemType)
        {
            base.Dictionary.Add(newsItemType.Id, newsItemType);
        }

        /// <summary>
        /// Checks if the NewsItemType exists in this container.
        /// </summary>
        /// <param name="newsItemType"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.NewsItemType newsItemType)
        {
            return base.Dictionary.Contains(newsItemType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.NewsItemType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an NewsItemType object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.NewsItemType Find(string name)
        {
            foreach (iTrading.Core.Kernel.NewsItemType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an NewsItemType object, by it's broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.NewsItemType FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.NewsItemType type in base.Dictionary.Values)
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
        /// Retrieves an NewsItemType object by it's id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.NewsItemType this[NewsItemTypeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.NewsItemType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.NewsItemTypeDictionary.NewsItemType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.NewsItemType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.NewsItemTypeDictionary" />
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

