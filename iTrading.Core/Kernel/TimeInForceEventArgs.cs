using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.TimeInForceEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class TimeInForceEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.TimeInForce timeInForce;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public TimeInForceEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.timeInForce = new iTrading.Core.Kernel.TimeInForce((TimeInForceId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public TimeInForceEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, TimeInForceId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.timeInForce = new iTrading.Core.Kernel.TimeInForce(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.TimeInForceEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.TimeInForceEventArgs.Process: " + this.TimeInForce.Name);
            }
            
                base.Request.Connection.TimeInForces.OnTimeInForceChange( base.Request.Connection, this);
           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.TimeInForce.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.TimeInForceEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.TimeInForceEventArgs;
            }
        }

        /// <summary>
        /// TimeInForce.
        /// </summary>
        public iTrading.Core.Kernel.TimeInForce TimeInForce
        {
            get
            {
                return this.timeInForce;
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

