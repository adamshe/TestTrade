using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.AccountItemTypeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class AccountItemTypeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.AccountItemType accountItemType;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public AccountItemTypeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.accountItemType = new iTrading.Core.Kernel.AccountItemType((AccountItemTypeId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public AccountItemTypeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, AccountItemTypeId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.accountItemType = new iTrading.Core.Kernel.AccountItemType(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.AccountItemTypeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.AccountItemTypeEventArgs.Process: " + this.AccountItemType.Name);
            }
          
                base.Request.Connection.AccountItemTypes.OnAccountItemTypeChange(base.Request.Connection, this);
        }



        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.accountItemType.Id);
        }

        /// <summary>
        /// AccountItemType.
        /// </summary>
        public iTrading.Core.Kernel.AccountItemType AccountItemType
        {
            get
            {
                return this.accountItemType;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.AccountItemTypeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.AccountItemTypeEventArgs;
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

