using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.ExchangeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class ExchangeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.Exchange exchange;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public ExchangeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.exchange = new iTrading.Core.Kernel.Exchange((ExchangeId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="exchangeId"></param>
        /// <param name="mapId"></param>
        public ExchangeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, ExchangeId exchangeId, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.exchange = new iTrading.Core.Kernel.Exchange(exchangeId, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.ExchangeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.ExchangeEventArgs.Process: " + this.Exchange.Name);
            }
           
                base.Request.Connection.Exchanges.OnExchangeChange( base.Request.Connection, this);
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.exchange.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.ExchangeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.ExchangeEventArgs;
            }
        }

        /// <summary>
        /// Exchange.
        /// </summary>
        public iTrading.Core.Kernel.Exchange Exchange
        {
            get
            {
                return this.exchange;
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

