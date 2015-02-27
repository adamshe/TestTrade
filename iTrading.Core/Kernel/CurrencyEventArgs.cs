using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.CurrencyEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class CurrencyEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.Currency currency;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public CurrencyEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.currency = new iTrading.Core.Kernel.Currency((CurrencyId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public CurrencyEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, CurrencyId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.currency = new iTrading.Core.Kernel.Currency(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.CurrencyEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.CurrencyEventArgs.Process: " + this.Currency.Name);
            }
           
                base.Request.Connection.OnCurrencyChange(base.Request.Connection, this);
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.currency.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.CurrencyEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.CurrencyEventArgs;
            }
        }

        /// <summary>
        /// Currency.
        /// </summary>
        public iTrading.Core.Kernel.Currency Currency
        {
            get
            {
                return this.currency;
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

