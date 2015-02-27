using System.Collections;
using iTrading.Core.Data;
using iTrading.Core.Kernel;
using Order=iTrading.Core.Kernel.Order;
using Request=iTrading.Core.Kernel.Request;
using Symbol=iTrading.Core.Kernel.Symbol;

namespace iTrading.Client
{
    internal class ClientBytes : Bytes
    {
        private Hashtable clientId2Request;
        private Hashtable serverId2Request;

        internal ClientBytes(Connection connection) : base(connection)
        {
            this.clientId2Request = new Hashtable();
            this.serverId2Request = new Hashtable();
        }

        internal void RegisterClientRequest(int clientId, Request clientRequest)
        {
            lock (this.clientId2Request)
            {
                this.clientId2Request[clientId] = clientRequest;
            }
        }

        internal void RegisterServerRequest(int serverId, Request clientRequest)
        {
            lock (this.serverId2Request)
            {
                this.serverId2Request[serverId] = clientRequest;
                clientRequest.MapId = serverId;
            }
        }

        protected override int ToId(Order order)
        {
            return order.MapId;
        }

        protected override int ToId(Request request)
        {
            return request.Id;
        }

        protected override int ToId(Symbol symbol)
        {
            return symbol.MapId;
        }

        protected override Order ToOrder(int id)
        {
            return (Order) this.serverId2Request[id];
        }

        protected override Request ToRequest(int id)
        {
            if (id < 0)
            {
                return null;
            }
            return (Request) this.clientId2Request[id];
        }

        protected override Symbol ToSymbol(int id)
        {
            return (Symbol) this.serverId2Request[id];
        }
    }
}