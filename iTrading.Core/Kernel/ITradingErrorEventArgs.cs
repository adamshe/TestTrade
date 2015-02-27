using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Errors will usually be indicated by throwing the appopriate event and
    /// setting the <see cref="P:iTrading.Core.Kernel.ITradingBaseEventArgs.Error" /> and <see cref="P:iTrading.Core.Kernel.ITradingBaseEventArgs.NativeError" /> 
    /// parameters of the <see cref="T:iTrading.Core.Kernel.ITradingBaseEventArgs" /> parameter. 
    /// However in case of an error which can't be associated with an event,
    /// the event <see cref="E:iTrading.Core.Kernel.Connection.Error" /> will be thrown.
    /// </summary>
    [ComVisible(false)]
    public class ITradingErrorEventArgs : ITradingBaseEventArgs, IComErrorEventArgs, ITradingSerializable
    {
        private readonly string message;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public ITradingErrorEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.message = bytes.ReadString();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="message"></param>
        public ITradingErrorEventArgs(Connection connection, ErrorCode errorCode, string nativeError, string message) : base(connection, errorCode, nativeError)
        {
            this.message = message;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.WriteLine(this.Message);
            
           base.Request.Connection.OnErrorChange(base.Request, this);
           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.message);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.ITradingErrorEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.ErrorEventArgs;
            }
        }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message
        {
            get
            {
                return (this.message + ((base.NativeError.Length == 0) ? "" : (": " + base.NativeError)) + ((base.Error >= ErrorCode.NoError) ? "" : (" (" + base.Error.ToString("g") + ")")));
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

