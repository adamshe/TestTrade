namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public class StreamingRequestDictionary : DictionaryBase
    {
        internal void Add(StreamingRequest request)
        {
            base.Dictionary.Add(request.Id, request);
        }

        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Request value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Request[] array, int index)
        {
            this.CopyTo(array, index);
        }

        internal void Remove(int id)
        {
            base.Dictionary.Remove(id);
        }

        /// <summary></summary>
        public StreamingRequest this[int id]
        {
            get
            {
                return (StreamingRequest) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of all <see cref="T:iTrading.Core.Kernel.StreamingRequest" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }
    }
}

