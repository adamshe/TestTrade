namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of TraceLevels.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("B372A6FC-3FBF-47dc-9C5B-9EF3726BDC27")]
    public class TraceLevelDictionary : DictionaryBase, IComTraceLevelDictionary
    {
        internal void Add(TraceLevel traceLevel)
        {
            base.Dictionary.Add(traceLevel.Id, traceLevel);
        }

        /// <summary>
        /// Checks if the tracelevel exists in this container.
        /// </summary>
        /// <param name="traceLevel"></param>
        /// <returns></returns>
        public bool Contains(TraceLevel traceLevel)
        {
            return base.Dictionary.Contains(traceLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(TraceLevel[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an TraceLevel object by its name.
        /// </summary>
        /// <param name="name"></param>
        public TraceLevel Find(string name)
        {
            foreach (TraceLevel level in base.Dictionary.Values)
            {
                if (level.Name == name)
                {
                    return level;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an TraceLevel object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TraceLevel this[TraceLevelIds id]
        {
            get
            {
                return (TraceLevel) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.TraceLevel" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.TraceLevel" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.TraceLevelDictionary" />
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

