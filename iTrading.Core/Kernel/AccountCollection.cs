using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds all accounts of a connection.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("0468D2E7-9782-4bcb-AEAB-8E54CA60B4A7")]
    public class AccountCollection : CollectionBase, IComAccountCollection
    {
        /// <summary>
        /// This event will be thrown once for every new account in the actual connection
        /// after opening the connection and before reveiving <see cref="E:iTrading.Core.Kernel.Account.AccountUpdate" /> events.
        /// </summary>
        public event AccountEventHandler Account;

        public void OnAccountChange(object pSender, AccountEventArgs pEvent)
        {
            if (Account != null)
                Account(pSender, pEvent);
        }

        internal AccountCollection()
        {
        }

        internal void Add(Account account)
        {
            base.List.Add(account);
        }

        /// <summary>
        /// Checks if the account exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Account value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Account[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves the account with <see cref="P:iTrading.Core.Kernel.Account.Name" /> = "accountName".
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public Account FindByName(string accountName)
        {
            lock (this)
            {
                foreach (Account account in base.List)
                {
                    if (account.Name == accountName)
                    {
                        return account;
                    }
                }
            }
            return null;
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(Account value)
        {
            return ((IList) this).IndexOf(value);
        }

        /// <summary>
        /// Get the n-th account of the container.
        /// </summary>
        public Account this[int index]
        {
            get
            {
                return (Account) base.List[index];
            }
        }
    }
}

