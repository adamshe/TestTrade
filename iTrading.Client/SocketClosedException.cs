using System;
using System.Runtime.Serialization;

namespace iTrading.Client
{
    [Serializable]
    public class SocketClosedException : Exception
    {
        public SocketClosedException()
        {
        }

        public SocketClosedException(string message) : base(message)
        {
        }

        protected SocketClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SocketClosedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}