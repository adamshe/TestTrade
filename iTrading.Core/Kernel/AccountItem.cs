namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents the value of an account attribute.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("4F123189-C61D-4829-B310-DAEC6B851B4A")]
    public class AccountItem : IComAccountItem
    {
        internal iTrading.Core.Kernel.Currency currency;
        internal double val;

        internal AccountItem(double val, iTrading.Core.Kernel.Currency currency)
        {
            this.currency = currency;
            this.val = val;
        }

        /// <summary>
        /// </summary>
        /// <param name="myObject"></param>
        /// <returns></returns>
        public override bool Equals(object myObject)
        {
            return (((myObject != null) && (myObject is AccountItem)) && (this == ((AccountItem) myObject)));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>Hash code value</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Checks if two <see cref="T:iTrading.Core.Kernel.AccountItem" /> instances are identical.
        /// </summary>
        /// <param name="accountItem1"></param>
        /// <param name="accountItem2"></param>
        /// <returns></returns>
        public static bool operator ==(AccountItem accountItem1, AccountItem accountItem2)
        {
            if ((accountItem1 != null) || (accountItem2 != null))
            {
                if ((accountItem1 == null) || (accountItem2 == null))
                {
                    return false;
                }
                if (accountItem1.Value != accountItem2.Value)
                {
                    return false;
                }
                if (accountItem1.Currency != accountItem2.Currency)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if two <see cref="T:iTrading.Core.Kernel.AccountItem" /> instances are different.
        /// </summary>
        /// <param name="accountItem1"></param>
        /// <param name="accountItem2"></param>
        /// <returns></returns>
        public static bool operator !=(AccountItem accountItem1, AccountItem accountItem2)
        {
            return !(accountItem1 == accountItem2);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (this.val.ToString("N") + " " + this.currency.Sign);
        }

        /// <summary>
        /// Currency.
        /// </summary>
        public iTrading.Core.Kernel.Currency Currency
        {
            get
            {
                return this.currency;
            }
        }

        /// <summary>
        /// Value.
        /// </summary>
        public double Value
        {
            get
            {
                return this.val;
            }
        }
    }
}

