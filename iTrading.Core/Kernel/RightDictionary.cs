namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of open right types.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("F5504030-0910-46e0-8AD7-25DBF9D15E28")]
    public class RightDictionary : DictionaryBase, IComRightDictionary
    {
        internal void Add(Right right)
        {
            base.Dictionary.Add(right.Id, right);
        }

        /// <summary>
        /// Checks if the option right exists in this container.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool Contains(Right right)
        {
            return base.Dictionary.Contains(right.Id);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(ConnectionStatus[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an <see cref="T:iTrading.Core.Kernel.Right" /> object by its name.
        /// </summary>
        /// <param name="name"></param>
        public Right Find(string name)
        {
            foreach (Right right in base.Dictionary.Values)
            {
                if (right.Name == name)
                {
                    return right;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an <see cref="T:iTrading.Core.Kernel.Right" /> object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Right this[RightId id]
        {
            get
            {
                return (Right) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The ICollection of available <see cref="T:iTrading.Core.Kernel.Right" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.Right" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.RightDictionary" />
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

