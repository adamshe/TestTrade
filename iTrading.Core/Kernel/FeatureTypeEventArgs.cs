using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.FeatureTypeEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class FeatureTypeEventArgs : ITradingBaseEventArgs, ITradingSerializable
    {
        private iTrading.Core.Kernel.FeatureType featureType;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public FeatureTypeEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.featureType = new iTrading.Core.Kernel.FeatureType((FeatureTypeId) bytes.ReadInt32(), bytes.ReadDouble());
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="customValue"></param>
        public FeatureTypeEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, FeatureTypeId id, double customValue) : base(currentConnection, errorCode, nativeError)
        {
            this.featureType = new iTrading.Core.Kernel.FeatureType(id, customValue);
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.FeatureTypeEventArgs.Process");
            if (Globals.TraceSwitch.Types)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.FeatureTypeEventArgs.Process: " + this.FeatureType.Name);
            }
            
             base.Request.Connection.FeatureTypes.OnFeatureTypeChange(base.Request.Connection, this);
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write((int) this.featureType.Id);
            bytes.Write(this.featureType.Value);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.FeatureTypeEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.FeatureTypeEventArgs;
            }
        }

        /// <summary>
        /// FeatureType.
        /// </summary>
        public iTrading.Core.Kernel.FeatureType FeatureType
        {
            get
            {
                return this.featureType;
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

