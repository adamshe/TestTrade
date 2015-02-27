using iTrading.Core.Kernel;

namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class BrokerRequestAcctSummary : iTrading.Track.Request
    {
        private Account account;

        internal BrokerRequestAcctSummary(Adapter adapter, Account account) : base(adapter)
        {
            this.account = account;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestAcctSummary.Process");
            }
            reader.ReadByte();
            string mapId = reader.ReadString(30).Trim();
            string str2 = reader.ReadString(20).Trim().Replace(",", "");
            reader.ReadString(0x10).Trim();
            if (mapId.Length != 0)
            {
                AccountItemType itemType = base.Adapter.connection.AccountItemTypes.FindByMapId(mapId);
                if (itemType != null)
                {
                    Currency currency = base.Adapter.connection.Currencies[CurrencyId.UsDollar];
                    Trace.Assert(currency != null, "Track.BrokerRequestAcctSummary.Process");
                    double newValue = 0.0;
                    try
                    {
                        newValue = Convert.ToDouble(str2, base.Adapter.cultureInfo);
                    }
                    catch
                    {
                        return;
                    }
                    if (this.account.GetItem(itemType, currency).Value != newValue)
                    {
                        base.Adapter.connection.ProcessEventArgs(new AccountUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", this.account, itemType, currency, newValue, base.Adapter.connection.Now));
                    }
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestAcctSummary.Send: account='" + this.account.Name + "'");
            }
            return (iTrading.Track.ErrorCode) Api.BrokerRequestAcctSummary(base.Rqn, this.account.Name);
        }
    }
}

