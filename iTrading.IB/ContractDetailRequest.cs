namespace iTrading.IB
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class ContractDetailRequest : iTrading.IB.Request
    {
        private Contract contract;
        private Symbol symbol;

        internal ContractDetailRequest(Adapter adapter, Symbol symbol) : base(adapter.connection)
        {
            this.contract = adapter.Convert(symbol);
            this.symbol = symbol;
        }

        internal static void Process(Adapter adapter, int version)
        {
            try
            {
                double tickSize = 0.0;
                double num2 = 1.0;
                Contract contract = new Contract("");
                string str = "";
                contract.Symbol = adapter.ibSocket.ReadString();
                contract.SecType = adapter.ibSocket.ReadString();
                contract.Expiry = adapter.ibSocket.ReadExpiry();
                contract.Strike = adapter.ibSocket.ReadDouble();
                contract.Right = adapter.ibSocket.ReadRight();
                contract.Exchange = adapter.ibSocket.ReadString();
                contract.Currency = adapter.ibSocket.ReadString();
                contract.LocalSymbol = adapter.ibSocket.ReadString();
                adapter.ibSocket.ReadString();
                adapter.ibSocket.ReadString();
                adapter.ibSocket.ReadInteger();
                tickSize = adapter.ibSocket.ReadDouble();
                num2 = adapter.ibSocket.ReadDouble();
                adapter.ibSocket.ReadString();
                str = adapter.ibSocket.ReadString();
                ExchangeDictionary exchanges = new ExchangeDictionary();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    Exchange exchange = adapter.connection.Exchanges.FindByMapId(str2);
                    if ((exchange != null) && !exchanges.Contains(exchange))
                    {
                        exchanges.Add(exchange);
                    }
                }
                iTrading.Core.Kernel.Currency currency = adapter.connection.Currencies.FindByMapId(contract.Currency);
                if (currency == null)
                {
                    currency = adapter.connection.Currencies[CurrencyId.Unknown];
                }
                double pointValue = (num2 == 0.0) ? 1.0 : num2;
                if ((contract.Symbol == "Z") && (contract.SecType == "FUT"))
                {
                    pointValue = 10.0;
                }
                Symbol symbol = adapter.Convert(contract, currency, tickSize, pointValue, exchanges);
                if (Globals.TraceSwitch.SymbolLookup)
                {
                    Trace.WriteLine("(" + adapter.connection.IdPlus + ") IB.ContractDetailRequest.Process: " + symbol.FullName);
                }
            }
            catch (Exception exception)
            {
                adapter.connection.ProcessEventArgs(new SymbolEventArgs(adapter.connection, ErrorCode.NoSuchSymbol, "No such symbol", null));
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.NoError, "", "IB.ContractDetailRequest.Process exception caught: " + exception.Message));
            }
        }

        internal override void Send(Adapter adapter)
        {
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + adapter.connection.IdPlus + ") IB.ContractDetailRequest.Send: " + this.symbol.FullName);
            }
            adapter.ibSocket.Send(9);
            adapter.ibSocket.Send(1);
            adapter.ibSocket.Send(this.contract.Symbol);
            adapter.ibSocket.Send(this.contract.SecType);
            adapter.ibSocket.Send((this.contract.Expiry == Globals.MaxDate) ? "" : this.contract.Expiry.ToString("yyyyMM"));
            adapter.ibSocket.Send(this.contract.Strike);
            adapter.ibSocket.Send((string) Names.Rights[this.contract.Right]);
            adapter.ibSocket.Send(this.contract.Exchange);
            adapter.ibSocket.Send(this.contract.Currency);
            adapter.ibSocket.Send(this.contract.LocalSymbol);
        }
    }
}

