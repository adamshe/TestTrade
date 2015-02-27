using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// For internal use only.
    /// </summary>
    [ComVisible(false)]
    public class SymbolEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.Symbol symbol;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public SymbolEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.symbol = (iTrading.Core.Kernel.Symbol) bytes.ReadSerializable();
        }

        /// <summary></summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="symbol"></param>
        public SymbolEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, iTrading.Core.Kernel.Symbol symbol) : base(currentConnection, errorCode, nativeError)
        {
            this.symbol = symbol;
        }

        /// <summary></summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.SymbolEventArgs.Process");
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.SymbolEventArgs.Process " + ((base.Error == ErrorCode.NoError) ? this.Symbol.FullName : "null"));
            }
            base.Request.Connection.OnSymbolChange(base.Request, this);
          
        }

        internal void Pulse()
        {
            if ((base.Request.Connection.FeatureTypes[FeatureTypeId.SynchronousSymbolLookup] == null) && (base.Request.Connection.symbol2Lookup != null))
            {
                lock (base.Request.Connection.Symbols)
                {
                    Monitor.Pulse(base.Request.Connection.Symbols);
                }
            }
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.WriteSerializable(this.symbol);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.SymbolEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.SymbolEventArgs;
            }
        }

        /// <summary>
        /// The new symbol.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }
    }
}

