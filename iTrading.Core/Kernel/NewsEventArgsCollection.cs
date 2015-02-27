namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds all NewsEventArgs of a connection.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("650DEE77-05A8-443f-8439-DE3C1632451C")]
    public class NewsEventArgsCollection : CollectionBase, IComNewsEventArgsCollection
    {
        private Connection connection;
        internal NewsEventHandler newsEventHandler;

        /// <summary>
        /// This event will be thrown once for every new NewsEventArgs in the actual connection.
        /// </summary>
        public event NewsEventHandler News
        {
            add
            {
                foreach (NewsEventArgs args in this)
                {
                    value(this, args);
                }
                this.newsEventHandler = (NewsEventHandler) Delegate.Combine(this.newsEventHandler, value);
            }
            remove
            {
                this.newsEventHandler = (NewsEventHandler) Delegate.Remove(this.newsEventHandler, value);
            }
        }

        internal NewsEventArgsCollection(Connection connection)
        {
            this.connection = connection;
        }

        internal void Add(NewsEventArgs newsEventArgs)
        {
            base.List.Add(newsEventArgs);
        }

        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(NewsEventArgs value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(NewsEventArgs[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(NewsEventArgs value)
        {
            return ((IList) this).IndexOf(value);
        }

        /// <summary>
        /// Get the n-th NewsEventArgs of the container.
        /// </summary>
        public NewsEventArgs this[int index]
        {
            get
            {
                return (NewsEventArgs) base.List[index];
            }
        }
    }
}

