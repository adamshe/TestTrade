namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of ModeTypes.
    /// </summary>
    [Guid("3F99EDEA-BD51-4065-9E0F-DB9CF9EB11F8"), ClassInterface(ClassInterfaceType.None)]
    public class ModeTypeDictionary : DictionaryBase, IComModeTypeDictionary
    {
        internal void Add(ModeType modeType)
        {
            base.Dictionary.Add(modeType.Id, modeType);
        }

        /// <summary>
        /// Checks if the ModeType exists in this container.
        /// </summary>
        /// <param name="modeType"></param>
        /// <returns></returns>
        public bool Contains(ModeType modeType)
        {
            return base.Dictionary.Contains(modeType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(ModeType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an ModeType object by its name.
        /// </summary>
        /// <param name="name"></param>
        public ModeType Find(string name)
        {
            foreach (ModeType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an ModeType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ModeType this[ModeTypeId id]
        {
            get
            {
                return (ModeType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.ModeType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.ModeType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.ModeTypeDictionary" />
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

