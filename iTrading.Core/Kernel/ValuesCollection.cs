namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Generic container holding the elements of a <see cref="T:System.Collections.DictionaryBase" /> .
    /// Used to generate a Collection that can be enumerated in COM clients.
    /// Interop does not allow enumeration (for..each) of Dictionaries.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("B3F91815-079F-457e-8AD4-5EA564A1B318")]
    public class ValuesCollection : CollectionBase, IComValuesCollection
    {
        internal ValuesCollection(ICollection values)
        {
            lock (values)
            {
                foreach (object obj2 in values)
                {
                    if (base.List.Count == 0)
                    {
                        base.List.Add(obj2);
                    }
                    else
                    {
                        base.List.Insert(base.List.Count - 1, obj2);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the object exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(object value)
        {
            return base.List.Contains(value);
        }

        /// <summary>
        /// Get the n-th Object of the container.
        /// </summary>
        public object this[int index]
        {
            get
            {
                return base.List[index];
            }
        }
    }
}

