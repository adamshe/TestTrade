using iTrading.Core.Kernel;

namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class BrokerRequestAccountIds : iTrading.Track.Request
    {
        internal BrokerRequestAccountIds(Adapter adapter) : base(adapter)
        {
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestAccountIds.Process1");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadShort();
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Unable to retrieve account ids"));
            }
            else
            {
                int num = reader.ReadShort();
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.BrokerRequestAccountIds.Process2: num=", num }));
                }
                for (int i = 0; i < num; i++)
                {
                    string accountName = reader.ReadString(0x10).Trim();
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestAccountIds.Process3: account='" + accountName + "'");
                    }
                    if (base.Adapter.connection.Accounts.FindByName(accountName) == null)
                    {
                        base.Adapter.connection.ProcessEventArgs(new AccountEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", accountName, null));
                    }
                    Account account = base.Adapter.connection.Accounts.FindByName(accountName);
                    Trace.Assert(account != null, "Track.BrokerRequestAccountIds.Process: account='" + accountName + "'");
                    new BrokerRequestAcctSummary(base.Adapter, account).Send();
                    lock (base.Adapter.startupRequests)
                    {
                        base.Adapter.startupRequests.Add(new BrokerRequestAcctPositions(base.Adapter, account));
                        base.Adapter.startupRequests.Add(new BrokerRequestOrder(base.Adapter, account));
                        base.Adapter.startupRequests.Add(new BrokerRequestTransactionData(base.Adapter, account));
                    }
                }
            }
            base.Adapter.Init2();
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestAccountIds.Send");
            }
            return (iTrading.Track.ErrorCode) Api.BrokerRequestAcctIds(base.Rqn);
        }
    }
}

