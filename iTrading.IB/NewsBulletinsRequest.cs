namespace iTrading.IB
{
    using System;
    using System.Text.RegularExpressions;
    using iTrading.Core.Kernel;

    internal class NewsBulletinsRequest : iTrading.IB.StreamingRequest
    {
        private Adapter adapter;
        private readonly bool allNews;

        internal NewsBulletinsRequest(Adapter adapter) : base(adapter.connection)
        {
            this.allNews = true;
            this.adapter = adapter;
        }

        protected override void DoCancel()
        {
            this.adapter.ibSocket.Send(13);
            this.adapter.ibSocket.Send(1);
        }

        internal static void Process(Adapter adapter)
        {
            string id = adapter.ibSocket.ReadString();
            adapter.ibSocket.ReadString();
            string headLine = adapter.ibSocket.ReadString();
            string str3 = adapter.ibSocket.ReadString();
            if ((str3.Length == 0) || (str3 == "ALL"))
            {
                str3 = "SMART";
            }
            foreach (string str4 in str3.Split(new char[] { ',' }))
            {
                if (adapter.connection.Exchanges.FindByMapId(str4) == null)
                {
                    Exchange exchange = adapter.connection.Exchanges[ExchangeId.Default];
                    adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.NewsBulletinsRequest.Process: can't convert iTrading.IB exchange '" + str4 + "'"));
                }
                NewsItemType itemType = adapter.connection.NewsItemTypes[NewsItemTypeId.Default];
                headLine = Regex.Replace(headLine.Replace('\n', ' ').Replace('\r', ' '), "===*", "");
                adapter.connection.ProcessEventArgs(new NewsEventArgs(adapter.connection, ErrorCode.NoError, "", id, itemType, adapter.connection.Now, headLine, headLine));
            }
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(12);
            adapter.ibSocket.Send(1);
            adapter.ibSocket.Send(this.allNews ? 1 : 0);
        }
    }
}

