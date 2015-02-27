using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an update operation of an account. 
    /// An instance of this will be passed as argument to the <see cref="E:iTrading.Core.Kernel.Account.AccountUpdate" />.
    /// Either <see cref="P:iTrading.Core.Kernel.AccountUpdateEventArgs.Value" /> or 
    /// <see cref="P:iTrading.Core.Kernel.AccountUpdateEventArgs.Time" /> will hold valid values.
    /// </summary>
    [ComVisible(false)]
    public class AccountUpdateEventArgs : ITradingBaseEventArgs, IComAccountUpdateEventArgs, ITradingSerializable
    {
        private Account account;
        private iTrading.Core.Kernel.Currency currency;
        private AccountItemType itemType;
        private DateTime time;
        private double val;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public AccountUpdateEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.account = bytes.ReadAccount();
            this.currency = bytes.ReadCurrency();
            this.itemType = bytes.ReadAccountItemType();
            this.val = bytes.ReadDouble();
            this.time = bytes.ReadDateTime();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="account"></param>
        /// <param name="itemType"></param>
        /// <param name="currency"></param>
        /// <param name="newValue"></param>
        /// <param name="time"></param>
        public AccountUpdateEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, Account account, AccountItemType itemType, iTrading.Core.Kernel.Currency currency, double newValue, DateTime time) : base(currentConnection, errorCode, nativeError)
        {
            this.account = account;
            this.itemType = itemType;
            this.currency = currency;
            this.val = newValue;
            this.time = time;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.AccountUpdateEventArgs.Process");
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.AccountUpdateEventArgs.Process: account='" + this.Account.Name + "' item='" + this.itemType.Name + "' currency='" + this.Currency.Name + "'");
            }
            this.Account.GetItem(this.itemType, this.currency).val = this.Value;
            this.Account.lastUpdate = this.Time;
          
                this.Account.OnAccountUpdate(base.Request.Connection.Accounts, this);
           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.account);
            bytes.Write(this.currency);
            bytes.Write(this.itemType);
            bytes.Write(this.val);
            bytes.Write(this.time);
        }

        /// <summary>
        /// Account being updated. <seealso cref="E:iTrading.Core.Kernel.Account.AccountUpdate" />
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.AccountUpdateEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.AccountUpdateEventArgs;
            }
        }

        /// <summary>
        /// Currency of the new value.
        /// </summary>
        public iTrading.Core.Kernel.Currency Currency
        {
            get
            {
                return this.currency;
            }
        }

        /// <summary>
        /// Identifies the attribute being updated.
        /// </summary>
        public AccountItemType ItemType
        {
            get
            {
                return this.itemType;
            }
        }

        /// <summary>
        /// The new time value.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// The new value of the attribute.
        /// </summary>
        public double Value
        {
            get
            {
                return this.val;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public override int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

