using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.MarketDataTypeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class MarketDataTypeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.MarketDataType marketDataType;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public MarketDataTypeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.marketDataType = new iTrading.Core.Kernel.MarketDataType((MarketDataTypeId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public MarketDataTypeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, MarketDataTypeId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.marketDataType = new iTrading.Core.Kernel.MarketDataType(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.MarketDataTypeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.MarketDataTypeEventArgs.Process: " + this.MarketDataType.Name);
            }
            
                base.Request.Connection.MarketDataTypes.OnMarketDataTypeChange( base.Request.Connection, this);
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.marketDataType.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.MarketDataTypeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.MarketDataTypeEventArgs;
            }
        }

        /// <summary>
        /// MarketDataType.
        /// </summary>
        public iTrading.Core.Kernel.MarketDataType MarketDataType
        {
            get
            {
                return this.marketDataType;
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

