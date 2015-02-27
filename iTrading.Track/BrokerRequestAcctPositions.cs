using iTrading.Core.Kernel;

namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class BrokerRequestAcctPositions : iTrading.Track.Request
    {
        private Account account;

        internal BrokerRequestAcctPositions(Adapter adapter, Account account) : base(adapter)
        {
            this.account = account;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestAcctPositions.Process");
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
                    ProcessNow(base.Adapter, 'A', fields);
                    return;

                case 'E':
                    base.Adapter.Init2();
                    break;
            }
        }

        internal static void ProcessNow(Adapter adapter, char type, StringCollection fields)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + adapter.connection.IdPlus + ") Track.BrokerRequestAcctPositions.ProcessNow");
            }
            Account account = adapter.connection.Accounts.FindByName(fields[12]);
            if (account == null)
            {
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestAcctPositions.Process.OpenPositions: unknown account id '" + fields[15] + "'"));
            }
            else
            {
                Symbol symbol = adapter.ToSymbol(fields[0]);
                if (symbol == null)
                {
                    adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestAcctPositions.ProcessNow: unknown symbol '" + fields[0] + "'"));
                }
                else
                {
                    double avgPrice = 0.0;
                    try
                    {
                        avgPrice = Convert.ToDouble(fields[4], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestAcctPositions.ProcessNow: illegal execution price '" + fields[4] + "'"));
                        return;
                    }
                    int quantity = 0;
                    try
                    {
                        quantity = Convert.ToInt32(fields[1], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestAcctPositions.ProcessNow: illegal quantity '" + fields[1] + "'"));
                        return;
                    }
                    MarketPosition marketPosition = adapter.connection.MarketPositions[MarketPositionId.Long];
                    if (quantity < 0)
                    {
                        quantity = -quantity;
                        marketPosition = adapter.connection.MarketPositions[MarketPositionId.Short];
                    }
                    Operation update = Operation.Update;
                    Position position2 = account.Positions.FindBySymbol(symbol);
                    if (type == 'R')
                    {
                        update = Operation.Delete;
                    }
                    else if (position2 == null)
                    {
                        update = Operation.Insert;
                    }
                    adapter.connection.ProcessEventArgs(new PositionUpdateEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", update, account, symbol, marketPosition, quantity, adapter.connection.Currencies[CurrencyId.UsDollar], avgPrice));
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestAcctPositions.Send: account='" + this.account.Name + "'");
            }
            return (iTrading.Track.ErrorCode) Api.BrokerRequestAcctPositions((short) base.Rqn, this.account.Name, '\x0001');
        }
    }
}

