namespace iTrading.CT
{
    using System;
   using iTrading.Core.Kernel;

    internal class Exec
    {
        private Adapter adapter;

        internal Exec(Adapter adapter)
        {
            this.adapter = adapter;
        }

        internal void OnConnect(int demo)
        {
            try
            {
                if (this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connecting)
                {
                    this.adapter.Init();
                    this.adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.adapter.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
                }
            }
            catch (Exception exception)
            {
                this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Exec.OnConnect: " + exception.Message));
            }
        }

        internal void OnConnectFail(ConnectFail reason)
        {
        }

        internal void OnDisconnect(bool error)
        {
        }

        internal void OnServerConnect(string user)
        {
        }

        internal void OnServerDisconnect(string user)
        {
        }

        internal void OnUserLogoff(string user)
        {
        }

        internal void OnUserLogon(string user, bool demo)
        {
        }

        internal void OnUserLogonFail(string user, ConnectFail reason)
        {
        }
    }
}

