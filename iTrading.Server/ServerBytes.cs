using iTrading.Core.Data;
using iTrading.Core.Kernel;
using System.Collections;
using System.Diagnostics;

namespace TradeMagic.Server
{

    internal class ServerBytes : Bytes
    {
        private Hashtable request2ClientId = new Hashtable();
        internal Hashtable serverId2Request = new Hashtable();

        internal ServerBytes()
        {
        }

        internal void RegisterClientRequest(int clientId, Request serverRequest)
        {
            lock (this.request2ClientId)
            {
                this.request2ClientId[serverRequest] = clientId;
            }
        }

        internal void RegisterServerRequest(int serverId, Request serverRequest)
        {
            lock (this.serverId2Request)
            {
                this.serverId2Request[serverId] = serverRequest;
            }
        }

        protected override int ToId(Order order)
        {
            return order.Id;
        }

        protected override int ToId(Request serverRequest)
        {
            if (this.request2ClientId.Contains(serverRequest))
            {
                return (int) this.request2ClientId[serverRequest];
            }
            return -1;
        }

        protected override int ToId(Symbol symbol)
        {
            return symbol.Id;
        }

        protected override Order ToOrder(int id)
        {
            return (Order) this.serverId2Request[id];
        }

        protected override Request ToRequest(int id)
        {
            Trace.Assert(false, "Server.ServerBytes.ToRequest: code should not be reached");
            return null;
        }

        protected override Symbol ToSymbol(int id)
        {
            return (Symbol) this.serverId2Request[id];
        }
    }
}

