using iTrading.Core.Kernel;

namespace iTrading.IB
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using iTrading.Core.Kernel;

    internal class AccountUpdatesRequest : iTrading.IB.Request
    {
        internal string account;
        internal int elapsed;
        private static DateTime lastUpdate = Globals.MaxDate;
        internal bool subscribe;

        internal AccountUpdatesRequest(Adapter adapter, string account, bool subscribe) : base(adapter.connection)
        {
            this.elapsed = 0;
            this.account = account;
            this.subscribe = subscribe;
        }

        internal static void ProcessPortfolioUpdate(Adapter adapter, int version)
        {
            PortfolioUpdate update = new PortfolioUpdate();
            update.contract.Symbol = adapter.ibSocket.ReadString();
            update.contract.SecType = adapter.ibSocket.ReadString();
            update.contract.Expiry = adapter.ibSocket.ReadExpiry();
            update.contract.Strike = adapter.ibSocket.ReadDouble();
            update.contract.Right = adapter.ibSocket.ReadRight();
            update.contract.Currency = adapter.ibSocket.ReadString();
            if (version >= 2)
            {
                update.contract.LocalSymbol = adapter.ibSocket.ReadString();
            }
            update.size = adapter.ibSocket.ReadInteger();
            update.marketPrice = adapter.ibSocket.ReadDouble();
            update.marketValue = adapter.ibSocket.ReadDouble();
            if (version >= 3)
            {
                update.averageCost = adapter.ibSocket.ReadDouble();
                update.unrealizedPNL = adapter.ibSocket.ReadDouble();
                update.realizedPNL = adapter.ibSocket.ReadDouble();
            }
            if (version >= 4)
            {
                update.account = adapter.ibSocket.ReadString();
            }
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", adapter.connection.IdPlus, ") IB.AccountUpdatesRequest.ProcessPortfolioUpdate: symbol='", update.contract.Symbol, "' size=", update.size, " averageCost=", update.averageCost, " marketPrice=", update.marketPrice }));
            }
            adapter.connection.SynchronizeInvoke.AsyncInvoke(new ProcessPortfolioUpdateDelegate(AccountUpdatesRequest.ProcessPortfolioUpdateNow), new object[] { adapter, update });
        }

        private static void ProcessPortfolioUpdateNow(Adapter adapter, PortfolioUpdate portfolioUpdate)
        {
            Account account = adapter.GetAccount(portfolioUpdate.account);
            if ((account != null) && (((portfolioUpdate.contract.SecType == "FUT") || (portfolioUpdate.contract.SecType == "STK")) || ((portfolioUpdate.contract.SecType == "OPT") || (portfolioUpdate.contract.SecType == "IND"))))
            {
                iTrading.Core.Kernel.Operation insert = (portfolioUpdate.size == 0) ? iTrading.Core.Kernel.Operation.Delete : iTrading.Core.Kernel.Operation.Update;
                Position position = null;
                Symbol symbol = adapter.Convert(portfolioUpdate.contract, null, 0.01, 1.0, null);
                if (symbol == null)
                {
                    adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.InvalidNativeSymbol, "", string.Concat(new object[] { "IB.AccountUpdatesRequest.ProcessPortfolioUpdate: Contract '", portfolioUpdate.contract.Symbol, "/", portfolioUpdate.contract.SecType, "/", portfolioUpdate.contract.Expiry, "/", portfolioUpdate.contract.Exchange })));
                }
                else
                {
                    position = account.Positions.FindBySymbol(symbol);
                    if (position == null)
                    {
                        if (insert == iTrading.Core .Kernel .Operation.Delete)
                        {
                            return;
                        }
                        if (portfolioUpdate.size == 0)
                        {
                            return;
                        }
                        insert = iTrading.Core .Kernel .Operation.Insert;
                    }
                    iTrading.Core .Kernel .Currency currency = adapter.connection.Currencies.FindByMapId(portfolioUpdate.contract.Currency);
                    if (currency == null)
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.AccountUpdatesRequest.Process: Unknown IB currency '" + portfolioUpdate.contract.Currency + "'"));
                        currency = adapter.connection.Currencies[CurrencyId.Unknown];
                    }
                    MarketPosition marketPosition = adapter.connection.MarketPositions[(portfolioUpdate.size < 0) ? MarketPositionId.Short : MarketPositionId.Long];
                    int num = Math.Abs(portfolioUpdate.size);
                    if ((((insert != iTrading.Core .Kernel .Operation.Update) || (marketPosition.Id != position.MarketPosition.Id)) || ((num != position.Quantity) || (currency.Id != position.Currency.Id))) || (portfolioUpdate.averageCost != position.AvgPrice))
                    {
                        double averageCost = portfolioUpdate.averageCost;
                        if ((portfolioUpdate.contract.Symbol == "Z") && (portfolioUpdate.contract.SecType == "FUT"))
                        {
                            averageCost *= 100.0;
                        }
                        adapter.connection.ProcessEventArgs(new PositionUpdateEventArgs(adapter.connection, ErrorCode.NoError, "", insert, account, symbol, marketPosition, (insert == iTrading.Core .Kernel .Operation.Delete) ? 0 : num, currency, averageCost));
                    }
                }
            }
        }

        internal static void ProcessUpdate(Adapter adapter, int version)
        {
            AccountUpdate update = new AccountUpdate();
            update.stringItem = adapter.ibSocket.ReadString();
            update.stringValue = adapter.ibSocket.ReadString();
            update.stringCurrency = adapter.ibSocket.ReadString();
            if (version >= 2)
            {
                update.account = adapter.ibSocket.ReadString();
            }
            else if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", adapter.connection.IdPlus, ") IB.AccountUpdatesRequest.ProcessUpdate: version=", version }));
            }
            adapter.connection.SynchronizeInvoke.AsyncInvoke(new ProcessUpdateDelegate(AccountUpdatesRequest.ProcessUpdateNow), new object[] { adapter, update });
        }

        private static void ProcessUpdateNow(Adapter adapter, AccountUpdate accountUpdate)
        {
            accountUpdate.itemType = adapter.connection.AccountItemTypes.FindByMapId(accountUpdate.stringItem);
            if (accountUpdate.stringCurrency == "BASE")
            {
                accountUpdate.currency = adapter.connection.Currencies[((IBOptions) adapter.connection.Options).BaseCurrency];
            }
            else
            {
                accountUpdate.currency = adapter.connection.Currencies.FindByMapId(accountUpdate.stringCurrency);
            }
            if (accountUpdate.currency == null)
            {
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.AccountUpdatesRequest.Process: Unknown IB currency '" + accountUpdate.stringCurrency + "'"));
                accountUpdate.currency = adapter.connection.Currencies[CurrencyId.Unknown];
            }
            if (accountUpdate.itemType != null)
            {
                Account account = adapter.GetAccount(accountUpdate.account);
                if ((account != null) && (accountUpdate.stringValue.Length != 0))
                {
                    try
                    {
                        accountUpdate.doubleValue = Convert.ToDouble(accountUpdate.stringValue, adapter.ibSocket.numberFormatInfo);
                    }
                    catch (Exception)
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.AccountUpdatesRequest.Process: Unable to convert '" + accountUpdate.stringValue + "' to double value"));
                    }
                    if (lastUpdate == Globals.MaxDate)
                    {
                        lastUpdate = adapter.connection.Now;
                    }
                    AccountItem item = account.GetItem(accountUpdate.itemType, accountUpdate.currency);
                    if ((item.Currency.Id != accountUpdate.currency.Id) || (item.Value != accountUpdate.doubleValue))
                    {
                        adapter.connection.ProcessEventArgs(new AccountUpdateEventArgs(adapter.connection, ErrorCode.NoError, "", account, accountUpdate.itemType, accountUpdate.currency, accountUpdate.doubleValue, lastUpdate));
                    }
                }
            }
        }

        internal static void ProcessUpdateTime(Adapter adapter, int version)
        {
            string str = adapter.ibSocket.ReadString();
            if (str.Length != 0)
            {
                string[] strArray = str.Split(new char[] { ':' });
                lastUpdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), 0);
            }
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(6);
            adapter.ibSocket.Send(2);
            adapter.ibSocket.Send(this.subscribe ? 1 : 0);
            if (adapter.ibSocket.ServerVersion >= 9)
            {
                adapter.ibSocket.Send(this.account);
            }
        }

        private delegate void ProcessPortfolioUpdateDelegate(Adapter adapter, PortfolioUpdate portfolioUpdate);

        private delegate void ProcessUpdateDelegate(Adapter adapter, AccountUpdate accountUpdate);
    }
}

