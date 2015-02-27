using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.SymbolTypeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class SymbolTypeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.SymbolType symbolType;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public SymbolTypeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.symbolType = new iTrading.Core.Kernel.SymbolType((SymbolTypeId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="symbolType"></param>
        /// <param name="mapId"></param>
        public SymbolTypeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, SymbolTypeId symbolType, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.symbolType = new iTrading.Core.Kernel.SymbolType(symbolType, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.SymbolTypeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.SymbolTypeEventArgs.Process: " + this.SymbolType.Name);
            }
            
                base.Request.Connection.SymbolTypes.OnSymbolTypeChange( base.Request.Connection, this);
            
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.symbolType.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.SymbolTypeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.SymbolTypeEventArgs;
            }
        }

        /// <summary>
        /// SymbolType.
        /// </summary>
        public iTrading.Core.Kernel.SymbolType SymbolType
        {
            get
            {
                return this.symbolType;
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

