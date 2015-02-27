using iTrading.Core.Kernel;

namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using iTrading.Core.Kernel;

    internal class BrokerRequestTransactionData : iTrading.Track.Request
    {
        private Account account;

        internal BrokerRequestTransactionData(Adapter adapter, Account account) : base(adapter)
        {
            this.account = account;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestTransactionData.Process: account='" + this.account.Name + "'");
            }
            char ch = (char) reader.ReadByte();
            reader.Skip(3);
            reader.ReadInteger();
            int countFields = reader.ReadInteger();
            reader.ReadInteger();
            StringCollection fields = reader.ReadBrokerTableFields(countFields);
            switch (ch)
            {
                case 'B':
                    ProcessNow(base.Adapter, fields);
                    return;

                case 'E':
                    base.Adapter.Init2();
                    break;
            }
        }

        internal static void ProcessNow(Adapter adapter, StringCollection fields)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + adapter.connection.IdPlus + ") Track.BrokerRequestTransactionData.ProcessNow: " + iTrading.Track.Request.ToString(fields));
            }
            Account account = adapter.connection.Accounts.FindByName(fields[12]);
            if (account == null)
            {
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestTransactionData.ProcessNow: unknown account id '" + fields[15] + "'"));
            }
            else
            {
                Symbol symbol = adapter.ToSymbol(fields[2]);
                if (symbol == null)
                {
                    adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestTransactionData.ProcessNow: unknown symbol '" + fields[2] + "'"));
                }
                else
                {
                    MarketPosition marketPosition = adapter.connection.MarketPositions[MarketPositionId.Long];
                    if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[3], "Sell", CompareOptions.IgnoreCase) == 0)
                    {
                        marketPosition = adapter.connection.MarketPositions[MarketPositionId.Short];
                    }
                    double avgPrice = 0.0;
                    try
                    {
                        avgPrice = Convert.ToDouble(fields[5], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestTransactionData.ProcessNow: illegal execution price '" + fields[5] + "'"));
                        return;
                    }
                    int quantity = 0;
                    try
                    {
                        quantity = Convert.ToInt32(fields[4], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestTransactionData.ProcessNow: illegal quantity '" + fields[4] + "'"));
                        return;
                    }
                    Execution execution = account.Executions.FindByExecId(fields[15]);
                    Operation insert = Operation.Insert;
                    if (execution != null)
                    {
                        insert = Operation.Update;
                    }
                    adapter.connection.ProcessEventArgs(new ExecutionUpdateEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", insert, fields[15], account, symbol, adapter.connection.Now, marketPosition, fields[10], quantity, avgPrice));
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            string startDate = base.Adapter.connection.Now.ToString("MM/dd/yy");
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestTransactionData.Send: account='" + this.account.Name + "' + endDate='" + startDate + "'");
            }
            return (iTrading.Track.ErrorCode) Api.BrokerRequestTransactionData((short) base.Rqn, this.account.Name, "", startDate, startDate);
        }
    }
}

