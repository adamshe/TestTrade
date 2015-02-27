using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.OrderStateEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class OrderStateEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.OrderState orderState;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public OrderStateEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.orderState = new iTrading.Core.Kernel.OrderState((OrderStateId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public OrderStateEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, OrderStateId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.orderState = new iTrading.Core.Kernel.OrderState(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.OrderStateEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.OrderStateEventArgs.Process: " + this.OrderState.Name);
            }
           
            base.Request.Connection.OrderStates.OnOrderStateChange( base.Request.Connection, this);            
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.orderState.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.OrderStateEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.OrderStateEventArgs;
            }
        }

        /// <summary>
        /// OrderState.
        /// </summary>
        public iTrading.Core.Kernel.OrderState OrderState
        {
            get
            {
                return this.orderState;
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

