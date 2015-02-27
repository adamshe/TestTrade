using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// This object will be passed as argument to the <see cref="T:iTrading.Core.Kernel.TimerEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class TimerEventArgs : ITradingBaseEventArgs, ITradingSerializable, IComTimerEventArgs
    {
        private iTrading.Core.Kernel.Connection connection;
        internal bool sync;
        private DateTime time;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public TimerEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.connection = (iTrading.Core.Kernel.Connection) bytes.ReadSerializable();
            this.time = bytes.ReadDateTime();
            this.sync = bytes.ReadBoolean();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="time"></param>
        /// <param name="nativeError"></param>
        /// <param name="sync"></param>
        public TimerEventArgs(iTrading.Core.Kernel.Connection currentConnection, ErrorCode errorCode, string nativeError, DateTime time, bool sync) : base(currentConnection, errorCode, nativeError)
        {
            this.connection = currentConnection;
            this.time = time;
            this.sync = sync;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.TimerEventArgs.Process");
           
                this.Connection.OnTimerChange(this.Connection, this);
           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.WriteSerializable(this.connection);
            bytes.Write(this.time);
            bytes.Write(this.sync);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.TimerEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.TimerEventArgs;
            }
        }

        /// <summary>
        /// The affected connection.
        /// </summary>
        public iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Current time.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public override int Version
        {
            get
            {
                return 2;
            }
        }
    }
}

