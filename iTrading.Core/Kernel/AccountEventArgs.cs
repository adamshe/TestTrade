using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.AccountEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class AccountEventArgs : ITradingBaseEventArgs, IComAccountEventArgs, ITradingSerializable
    {
        private Account account;
        private iTrading.Core.Kernel.SimulationAccountOptions simulationAccountOptions;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public AccountEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.account = new iTrading.Core.Kernel.Account(bytes.Connection, bytes.ReadString(), (iTrading.Core.Kernel.SimulationAccountOptions) bytes.ReadSerializable());
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="accountName"></param>
        /// <param name="simulationAccountOptions"></param>
        public AccountEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, string accountName, iTrading.Core.Kernel.SimulationAccountOptions simulationAccountOptions) : base(currentConnection, errorCode, nativeError)
        {
            this.account = new iTrading.Core.Kernel.Account(base.Request.Connection, accountName, simulationAccountOptions);
            this.simulationAccountOptions = simulationAccountOptions;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.AccountEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.AccountEventArgs.Process: " + this.Account.Name);
            }
          
                base.Request.Connection.Accounts.OnAccountChange( base.Request.Connection, this);
           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.account.Name);
            bytes.WriteSerializable(this.simulationAccountOptions);
        }

        /// <summary>
        /// Account.
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.AccountEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.AccountEventArgs;
            }
        }

        /// <summary>
        /// </summary>
        public iTrading.Core.Kernel.SimulationAccountOptions SimulationAccountOptions
        {
            get
            {
                return this.simulationAccountOptions;
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

