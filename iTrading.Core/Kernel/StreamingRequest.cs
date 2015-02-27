using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Base class for all streaming requests. <see cref="T:iTrading.Core.Kernel.StreamingRequest" /> should not be constructed or called directly.
    /// The class is mainly for internal use. Please call the appropriate methods of e.g. <see cref="T:iTrading.Core.Kernel.Connection" />,
    /// <see cref="T:iTrading.Core.Kernel.Account" /> or <see cref="T:iTrading.Core.Kernel.Order" />.
    /// </summary>
    public abstract class StreamingRequest : Request, ITradingSerializable
    {
        internal bool isRunning;

        /// <summary>For internal use only.</summary>
        protected StreamingRequest(Connection currentConnection) : base(currentConnection)
        {
            this.isRunning = false;
            lock (base.Connection.streamingRequests)
            {
                base.Connection.streamingRequests.Add(this);
            }
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        protected StreamingRequest(Bytes bytes, int version) : base(bytes, version)
        {
            this.isRunning = false;
            this.isRunning = bytes.ReadBoolean();
        }

        /// <summary>
        /// Cancels the actual request.
        /// </summary>
        public abstract void Cancel();
        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.isRunning);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.StreamingRequest.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.StreamingRequest;
            }
        }

        /// <summary>
        /// Returns the status of this request. TRUE if associated events will be thrown, else FALSE.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }
    }
}

