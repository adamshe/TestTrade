using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Abstract base class for all TradeMagic event args.
    /// </summary>
    [ComVisible(false)]
    public abstract class ITradingBaseEventArgs : EventArgs, IComTMEventArgs, ITradingSerializable
    {
        private ErrorCode errorCode;
        private string nativeError;
        private iTrading.Core.Kernel.Request request;

        /// <summary>
        /// For internal use only.
        /// </summary>
        protected ITradingBaseEventArgs(iTrading.Core.Kernel.Request request)
        {
            this.errorCode = ErrorCode.NoError;
            this.nativeError = "";
            this.request = request;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        protected ITradingBaseEventArgs(Bytes bytes, int version)
        {
            this.errorCode = (ErrorCode) bytes.ReadInt32();
            this.nativeError = bytes.ReadString();
            this.request = bytes.ReadRequest();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        protected ITradingBaseEventArgs(iTrading.Core.Kernel.Request request, ErrorCode errorCode, string nativeError)
        {
            this.errorCode = errorCode;
            this.nativeError = nativeError;
            this.request = request;
        }

        /// <summary>For internal use only.</summary>
        protected internal abstract void Process();
        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public virtual void Serialize(Bytes bytes, int version)
        {
            bytes.Write((int) this.errorCode);
            bytes.Write(this.nativeError);
            bytes.WriteRequest(this.request);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.ITradingBaseEventArgs.ClassId" /> of current object.
        /// </summary>
        public virtual iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.TMEventArgs;
            }
        }

        /// <summary>
        /// Returns the error code of the Args. 
        /// If the asyncronous call has been successful, the value will be <seealso cref="F:iTrading.Core.Kernel.ErrorCode.NoError" />.
        /// </summary>
        public ErrorCode Error
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// Native error code of underlying broker or data provider.
        /// </summary>
        public string NativeError
        {
            get
            {
                return this.nativeError;
            }
        }

        /// <summary>
        /// Request causing this event.
        /// </summary>
        public iTrading.Core.Kernel.Request Request
        {
            get
            {
                return this.request;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public virtual int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

