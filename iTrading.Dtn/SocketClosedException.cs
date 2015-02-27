namespace iTrading.Dtn
{
    using System;

    internal class SocketClosedException : Exception
    {
        internal SocketClosedException(string msg) : base(msg)
        {
        }
    }
}

