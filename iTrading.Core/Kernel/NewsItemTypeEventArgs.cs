using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.NewsItemTypeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class NewsItemTypeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.NewsItemType newsItemType;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public NewsItemTypeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.newsItemType = new iTrading.Core.Kernel.NewsItemType((NewsItemTypeId) bytes.ReadInt32(), "");
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="mapId"></param>
        public NewsItemTypeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, NewsItemTypeId id, string mapId) : base(currentConnection, errorCode, nativeError)
        {
            this.newsItemType = new iTrading.Core.Kernel.NewsItemType(id, mapId);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.NewsItemTypeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.NewsItemTypeEventArgs.Process: " + this.NewsItemType.Name);
            }
          
                base.Request.Connection.NewsItemTypes.OnNewsItemTypeChange( base.Request.Connection, this);
           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.newsItemType.Id);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.NewsItemTypeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.NewsItemTypeEventArgs;
            }
        }

        /// <summary>
        /// NewsItemType.
        /// </summary>
        public iTrading.Core.Kernel.NewsItemType NewsItemType
        {
            get
            {
                return this.newsItemType;
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

