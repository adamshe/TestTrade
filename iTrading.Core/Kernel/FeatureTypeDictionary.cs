namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of FeatureTypes.
    /// </summary>
    [Guid("A3D4EA37-4BC6-4fae-B0EA-93AC4C281045"), ClassInterface(ClassInterfaceType.None)]
    public class FeatureTypeDictionary : DictionaryBase, IComFeatureTypeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new FeatureType supported after opening the connection.
        /// </summary>
        public event FeatureTypeEventHandler FeatureType;

        public void OnFeatureTypeChange(object pSender, FeatureTypeEventArgs pEvent)
        {
            if (FeatureType != null)
                FeatureType(pSender, pEvent);
        }
        internal void Add(iTrading.Core.Kernel.FeatureType featureType)
        {
            base.Dictionary.Add(featureType.Id, featureType);
        }

        /// <summary>
        /// Checks if the FeatureType exists in this container.
        /// </summary>
        /// <param name="featureType"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.FeatureType featureType)
        {
            return base.Dictionary.Contains(featureType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.FeatureType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an FeatureType object by it's name.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.FeatureType Find(string name)
        {
            foreach (iTrading.Core.Kernel.FeatureType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an FeatureType object by it's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.FeatureType this[FeatureTypeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.FeatureType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.FeatureTypeDictionary.FeatureType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.FeatureType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.FeatureTypeDictionary" />
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

