namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents a connection status.
    /// </summary>
    [Guid("83B2135B-D68D-44cf-8C80-BFEDDCF4F546"), ClassInterface(ClassInterfaceType.None)]
    public class ConnectionStatus : IComConnectionStatus
    {
        private static ConnectionStatusDictionary all = null;
        private ConnectionStatusId id;

        internal ConnectionStatus(ConnectionStatusId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available connection status values.
        /// </summary>
        public static ConnectionStatusDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new ConnectionStatusDictionary();
                        all.Add(new ConnectionStatus(ConnectionStatusId.Connected));
                        all.Add(new ConnectionStatus(ConnectionStatusId.Connecting));
                        all.Add(new ConnectionStatus(ConnectionStatusId.ConnectionLost));
                        all.Add(new ConnectionStatus(ConnectionStatusId.Disconnected));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the connection status.
        /// </summary>
        public ConnectionStatusId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the ConnectionStatus.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case ConnectionStatusId.Disconnected:
                        return "Disconnected";

                    case ConnectionStatusId.Connecting:
                        return "Connecting";

                    case ConnectionStatusId.ConnectionLost:
                        return "Connection Lost";

                    case ConnectionStatusId.Connected:
                        return "Connected";
                }
                return "Disconnected";
            }
        }
    }
}

