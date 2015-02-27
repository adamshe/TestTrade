using iTrading.Core.Kernel;

namespace iTrading.IB
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Timers;
    using iTrading.Core.Kernel;

    internal class ManagedAcctsRequest : iTrading.IB.Request
    {
        internal ManagedAcctsRequest(Adapter adapter) : base(adapter.connection)
        {
        }

        internal static void Process(Adapter adapter, int version)
        {
            string str = adapter.ibSocket.ReadString();
            adapter.connection.SynchronizeInvoke.AsyncInvoke(new ProcessDelegate(ManagedAcctsRequest.ProcessNow), new object[] { adapter, str });
        }

        private static void ProcessNow(Adapter adapter, string accounts)
        {
            lock (adapter.connection.Accounts)
            {
                foreach (string str in accounts.Split(new char[] { ',' }))
                {
                    Account account = null;
                    foreach (Account account2 in adapter.connection.Accounts)
                    {
                        if ((str.Length > 0) && (account2.Name == str))
                        {
                            account = account2;
                        }
                    }
                    if ((str.Length > 0) && (account == null))
                    {
                        adapter.connection.ProcessEventArgs(new AccountEventArgs(adapter.connection, ErrorCode.NoError, "", str, null));
                    }
                }
                if (adapter.connection.ConnectionStatusId != ConnectionStatusId.Connected)
                {
                    if (adapter.faCustom.Length == 0)
                    {
                        new FaRequest(adapter, Fa.AccountAliases).Send(adapter);
                        new FaRequest(adapter, Fa.Groups).Send(adapter);
                        new FaRequest(adapter, Fa.Profile).Send(adapter);
                    }
                    adapter.accountUpdateRequests.Clear();
                    foreach (Account account3 in adapter.connection.Accounts)
                    {
                        AccountUpdatesRequest request = new AccountUpdatesRequest(adapter, account3.Name, true);
                        adapter.accountUpdateRequests.Add(request);
                        request.Send(adapter);
                    }
                    if (adapter.connection.Accounts.Count > 0)
                    {
                        adapter.accountUpdateTimer = new Timer(1000.0);
                        adapter.accountUpdateTimer.Elapsed += new ElapsedEventHandler(adapter.AccountUpdateTimer_Elapsed);
                        ((AccountUpdatesRequest) adapter.accountUpdateRequests[0]).Send(adapter);
                        adapter.accountUpdateTimer.Start();
                    }
                    adapter.StartConnectTimer();
                }
            }
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(0x11);
            adapter.ibSocket.Send(1);
        }

        private delegate void ProcessDelegate(Adapter adapter, string accounts);
    }
}

