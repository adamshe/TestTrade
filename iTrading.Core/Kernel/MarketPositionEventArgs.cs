using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.MarketPositionEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class MarketPositionEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.MarketPosition marketPosition;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public MarketPositionEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.marketPosition = new iTrading.Core.Kernel.MarketPosition((MarketPositionId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public MarketPositionEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, MarketPositionId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.marketPosition = new iTrading.Core.Kernel.MarketPosition(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.MarketPositionEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.MarketPositionEventArgs.Process: " + this.MarketPosition.Name);
            }
           
                base.Request.Connection.MarketPositions.OnMarketPositionChange( base.Request.Connection, this);
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.marketPosition.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.MarketPositionEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.MarketPositionEventArgs;
            }
        }

        /// <summary>
        /// MarketPosition.
        /// </summary>
        public iTrading.Core.Kernel.MarketPosition MarketPosition
        {
            get
            {
                return this.marketPosition;
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

