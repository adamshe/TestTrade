using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.OrderTypeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class OrderTypeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.OrderType orderType;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public OrderTypeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.orderType = new iTrading.Core.Kernel.OrderType((OrderTypeId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public OrderTypeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, OrderTypeId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.orderType = new iTrading.Core.Kernel.OrderType(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.OrderTypeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.OrderTypeEventArgs.Process: " + this.OrderType.Name);
            }

            base.Request.Connection.OrderTypes.OnOrderTypeChange(base.Request.Connection, this);
           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.orderType.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.OrderTypeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.OrderTypeEventArgs;
            }
        }

        /// <summary>
        /// OrderType.
        /// </summary>
        public iTrading.Core.Kernel.OrderType OrderType
        {
            get
            {
                return this.orderType;
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

