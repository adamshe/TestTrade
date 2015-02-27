namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// TradeMagic global exception class.
    /// </summary>
    [Serializable, ComVisible(false)]
    public class TMException : Exception, ISerializable
    {
        private ErrorCode errorCode;

        /// <summary></summary>
        public TMException()
        {
        }

        /// <summary></summary>
        /// <param name="message"></param>
        public TMException(string message) : base(message)
        {
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="errorCode"></param>
        public TMException(ErrorCode errorCode) : base("")
        {
            this.errorCode = errorCode;
        }

        /// <summary></summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected TMException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.errorCode = (ErrorCode) info.GetValue("errorCode", typeof(int));
        }

        /// <summary></summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TMException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public TMException(ErrorCode errorCode, string message) : base(message)
        {
            this.errorCode = errorCode;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("errorCode", (int) this.errorCode);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Indicates the cause of the exception.
        /// </summary>
        public ErrorCode Error
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// Returns the exption error message.
        /// </summary>
        public string Message
        {
            get
            {
                return (base.Message + " (" + this.Error.ToString("g") + ")");
            }
        }
    }
}

