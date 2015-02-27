namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Identifies the connection status.
    /// </summary>
    public enum ConnectionStatusId
    {
        Disconnected,
        Connecting,
        ConnectionLost,
        Connected
    }
}

