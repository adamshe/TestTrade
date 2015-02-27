using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.ActionTypeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class ActionTypeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.ActionType actionType;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public ActionTypeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.actionType = new iTrading.Core.Kernel.ActionType((ActionTypeId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public ActionTypeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, ActionTypeId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.actionType = new iTrading.Core.Kernel.ActionType(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.ActionTypeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.ActionTypeEventArgs.Process: " + this.ActionType.Name);
            }
           
                base.Request.Connection.ActionTypes.OnActionTypeChange( base.Request.Connection, this);
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.actionType.Id);
        }

        /// <summary>
        /// ActionType.
        /// </summary>
        public iTrading.Core.Kernel.ActionType ActionType
        {
            get
            {
                return this.actionType;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.ActionTypeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.ActionTypeEventArgs;
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

