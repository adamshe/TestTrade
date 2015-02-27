using System;
using System.Runtime.InteropServices;
using iTrading.Core.Data;
using iTrading.Core.Kernel;
using iTrading.Core.Data;

namespace iTrading.Core.Data
{
    /// <summary>For internal use only.
    /// </summary>
    [ComVisible(false)]
    public class Bytes
    {
        private char[] charBuf;
        private iTrading.Core.Kernel.Connection connection;
        private byte[] inBuf;
        private int inIdx;
        private byte[] outBuf;
        private int outIdx;
        private const int ByteSize = 1024;
        /// <summary>
        /// </summary>
        public Bytes()
        {
            this.charBuf = new char[ByteSize];
            this.connection = null;
            this.inIdx = 0;
            this.outBuf = new byte[ByteSize];
            this.outIdx = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="connection"></param>
        public Bytes(iTrading.Core.Kernel.Connection connection)
        {
            this.charBuf = new char[ByteSize];
            this.connection = null;
            this.inIdx = 0;
            this.outBuf = new byte[ByteSize];
            this.outIdx = 0;
            this.connection = connection;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.Account" />.
        /// </summary>
        /// <returns></returns>
        public Account ReadAccount()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadAccount: connection not set");
            }
            string str = this.ReadString();
            foreach (Account account in this.connection.Accounts)
            {
                if (account.Name == str)
                {
                    return account;
                }
            }
            throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadAccount: unknown account '" + str + "'");
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.AccountItemType" />.
        /// </summary>
        /// <returns></returns>
        public AccountItemType ReadAccountItemType()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadAccountItemType: connection not set");
            }
            int num = this.ReadInt32();
            AccountItemType type = this.connection.AccountItemTypes[(AccountItemTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadAccountItemType: unknown account item type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.ActionType" />.
        /// </summary>
        /// <returns></returns>
        public ActionType ReadActionType()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadActionType: connection not set");
            }
            int num = this.ReadInt32();
            ActionType type = this.connection.ActionTypes[(ActionTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadActionType: unknown action type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read bool value.
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            if ((this.inBuf == null) || ((this.inIdx + 1) > this.inBuf.Length))
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadBoolean: unexpected end of byte stream");
            }
            bool flag = BitConverter.ToBoolean(this.inBuf, this.inIdx);
            this.inIdx++;
            return flag;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.ConnectionStatus" />.
        /// </summary>
        /// <returns></returns>
        public ConnectionStatus ReadConnectionStatus()
        {
            int num = this.ReadInt32();
            ConnectionStatus status = ConnectionStatus.All[(ConnectionStatusId) num];
            if (status == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadConnectionStatus: unknown connection status '" + num + "'");
            }
            return status;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.Currency" />.
        /// </summary>
        /// <returns></returns>
        public iTrading.Core.Kernel.Currency ReadCurrency()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadCurrency: connection not set");
            }
            int num = this.ReadInt32();
            iTrading.Core.Kernel.Currency currency = this.connection.Currencies[(CurrencyId) num];
            if (currency == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadCurrency: unknown currency '" + num + "'");
            }
            return currency;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.CurrencyId" />.
        /// </summary>
        /// <returns></returns>
        public CurrencyId ReadCurrencyId()
        {
            return (CurrencyId) this.ReadInt32();
        }

        /// <summary>
        /// Read DateTime.
        /// </summary>
        /// <returns></returns>
        public DateTime ReadDateTime()
        {
            if ((this.inBuf == null) || ((this.inIdx + 8) > this.inBuf.Length))
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadDateTime: unexpected end of byte stream");
            }
            DateTime time = new DateTime(BitConverter.ToInt64(this.inBuf, this.inIdx));
            this.inIdx += 8;
            return time;
        }

        /// <summary>
        /// Read 8 byte floating point value.
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            if ((this.inBuf == null) || ((this.inIdx + 8) > this.inBuf.Length))
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadDouble: unexpected end of byte stream");
            }
            double num = BitConverter.ToDouble(this.inBuf, this.inIdx);
            this.inIdx += 8;
            return num;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.Exchange" />.
        /// </summary>
        /// <returns></returns>
        public Exchange ReadExchange()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadExchange: connection not set");
            }
            int num = this.ReadInt32();
            Exchange exchange = this.connection.Exchanges[(ExchangeId) num];
            if (exchange == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadExchange: unknown exchange '" + num + "'");
            }
            return exchange;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.FeatureType" />.
        /// </summary>
        /// <returns></returns>
        public FeatureType ReadFeatureType()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadFeatureType: connection not set");
            }
            int num = this.ReadInt32();
            FeatureType type = this.connection.FeatureTypes[(FeatureTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadFeatureType: unknown feature type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read 4 byte integer.
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            if ((this.inBuf == null) || ((this.inIdx + 4) > this.inBuf.Length))
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadInt32: unexpected end of byte stream");
            }
            int num = BitConverter.ToInt32(this.inBuf, this.inIdx);
            this.inIdx += 4;
            return num;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.LicenseType" />.
        /// </summary>
        /// <returns></returns>
        public LicenseType ReadLicenseType()
        {
            int num = this.ReadInt32();
            LicenseType type = LicenseType.All[(LicenseTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadLicenseType: unknown license type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.MarketDataType" />.
        /// </summary>
        /// <returns></returns>
        public MarketDataType ReadMarketData()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadMarketData: connection not set");
            }
            int num = this.ReadInt32();
            MarketDataType type = this.connection.MarketDataTypes[(MarketDataTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadMarketData: unknown market data type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.MarketPosition" />.
        /// </summary>
        /// <returns></returns>
        public MarketPosition ReadMarketPosition()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadMarketPosition: connection not set");
            }
            int num = this.ReadInt32();
            MarketPosition position = this.connection.MarketPositions[(MarketPositionId) num];
            if (position == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadMarketPosition: unknown market position '" + num + "'");
            }
            return position;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.ModeType" />.
        /// </summary>
        /// <returns></returns>
        public ModeType ReadModeType()
        {
            int num = this.ReadInt32();
            ModeType type = ModeType.All[(ModeTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadModeType: unknown mode type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.NewsItemType" />.
        /// </summary>
        /// <returns></returns>
        public NewsItemType ReadNewsItemType()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadNewsItemType: connection not set");
            }
            int num = this.ReadInt32();
            NewsItemType type = this.connection.NewsItemTypes[(NewsItemTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadNewsItemType: unknown news type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.Order" />.
        /// </summary>
        /// <returns></returns>
        public Order ReadOrder()
        {
            int id = this.ReadInt32();
            Order order = this.ToOrder(id);
            if (order == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadOrder: unknown order '" + id + "'");
            }
            return order;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.OrderState" />.
        /// </summary>
        /// <returns></returns>
        public OrderState ReadOrderState()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadOrderState: connection not set");
            }
            int num = this.ReadInt32();
            OrderState state = this.connection.OrderStates[(OrderStateId) num];
            if (state == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadOrderState: unknown order state '" + num + "'");
            }
            return state;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.OrderType" />.
        /// </summary>
        /// <returns></returns>
        public OrderType ReadOrderType()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadOrderType: connection not set");
            }
            int num = this.ReadInt32();
            OrderType type = this.connection.OrderTypes[(OrderTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadOrderType: unknown order type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.ProviderType" />.
        /// </summary>
        /// <returns></returns>
        public ProviderType ReadProvider()
        {
            int num = this.ReadInt32();
            ProviderType type = ProviderType.All[(ProviderTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadProvider: unknown provider type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        /// <returns></returns>
        public Request ReadRequest()
        {
            int id = this.ReadInt32();
            return this.ToRequest(id);
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.Right" />.
        /// </summary>
        /// <returns></returns>
        public Right ReadRight()
        {
            int num = this.ReadInt32();
            Right right = Right.All[(RightId) num];
            if (right == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadRight: unknown right '" + num + "'");
            }
            return right;
        }

        /// <summary>
        /// Deserialize a <see cref="T:iTrading.Core.Kernel.ITradingSerializable" /> object.
        /// </summary>
        /// <returns></returns>
        public ITradingSerializable ReadSerializable()
        {
            int num = this.ReadInt32();
            if (num < 0)
            {
                return null;
            }
            int version = this.ReadInt32();
            switch (((ClassId) num))
            {
                case ClassId.AccountEventArgs:
                    return new AccountEventArgs(this, version);

                case ClassId.AccountItemTypeEventArgs:
                    return new AccountItemTypeEventArgs(this, version);

                case ClassId.AccountUpdateEventArgs:
                    return new AccountUpdateEventArgs(this, version);

                case ClassId.ActionTypeEventArgs:
                    return new ActionTypeEventArgs(this, version);

                case ClassId.Connection:
                    return new iTrading.Core.Kernel.Connection(this, version);

                case ClassId.ConnectionStatusEventArgs:
                    return new ConnectionStatusEventArgs(this, version);

                case ClassId.CurrencyEventArgs:
                    return new CurrencyEventArgs(this, version);

                case ClassId.ErrorEventArgs:
                    return new ITradingErrorEventArgs(this, version);

                case ClassId.ExchangeEventArgs:
                    return new ExchangeEventArgs(this, version);

                case ClassId.Execution:
                    return new Execution(this, version);

                case ClassId.ExecutionUpdateEventArgs:
                    return new ExecutionUpdateEventArgs(this, version);

                case ClassId.FeatureTypeEventArgs:
                    return new FeatureTypeEventArgs(this, version);

                case ClassId.IBOptions:
                    return new IBOptions(this, version);

                case ClassId.MarketData:
                    return new MarketData(this, version);

                case ClassId.MarketDataEventArgs:
                    return new MarketDataEventArgs(this, version);

                case ClassId.MarketDataTypeEventArgs:
                    return new MarketDataTypeEventArgs(this, version);

                case ClassId.MarketDepth:
                    return new MarketDepth(this, version);

                case ClassId.MarketDepthEventArgs:
                    return new MarketDepthEventArgs(this, version);

                case ClassId.MarketPositionEventArgs:
                    return new MarketPositionEventArgs(this, version);

                case ClassId.MbtOptions:
                    return new MbtOptions(this, version);

                case ClassId.NewsEventArgs:
                    return new NewsEventArgs(this, version);

                case ClassId.NewsItemTypeEventArgs:
                    return new NewsItemTypeEventArgs(this, version);

                case ClassId.OptionsBase:
                    return new OptionsBase(this, version);

                case ClassId.Order:
                    return new Order(this, version);

                case ClassId.OrderStateEventArgs:
                    return new OrderStateEventArgs(this, version);

                case ClassId.OrderStatusEventArgs:
                    return new OrderStatusEventArgs(this, version);

                case ClassId.OrderTypeEventArgs:
                    return new OrderTypeEventArgs(this, version);

                case ClassId.PatsOptions:
                    return new PatsOptions(this, version);

                case ClassId.PositionUpdateEventArgs:
                    return new PositionUpdateEventArgs(this, version);

                case ClassId.Symbol:
                    return new Symbol(this, version);

                case ClassId.SymbolEventArgs:
                    return new SymbolEventArgs(this, version);

                case ClassId.SymbolTypeEventArgs:
                    return new SymbolTypeEventArgs(this, version);

                case ClassId.TimeInForceEventArgs:
                    return new TimeInForceEventArgs(this, version);

                case ClassId.TrackOptions:
                    return new TrackOptions(this, version);

                case ClassId.SimulationAccountOptions:
                    return new SimulationAccountOptions(this, version);

                case ClassId.SimulationSymbolOptions:
                    return new SimulationSymbolOptions(this, version);

                case ClassId.DataFileHeader:
                    return new DataFileHeader(this, version);

                case ClassId.TimerEventArgs:
                    return new TimerEventArgs(this, version);

                case ClassId.DtnOptions:
                    return new DtnOptions(this, version);

                case ClassId.ESignalOptions:
                    return new ESignalOptions(this, version);

                case ClassId.BarUpdateEventArgs:
                    return new BarUpdateEventArgs(this, version);

                case ClassId.YahooOptions:
                    return new YahooOptions(this, version);

                case ClassId.CTOptions:
                    return new CTOptions(this, version);
            }
            throw new TMException(ErrorCode.Panic, "unknown type id '" + num + "' to deserialize");
        }

        /// <summary>
        /// Read string.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            int length = 0;
            while (true)
            {
                char ch = BitConverter.ToChar(this.inBuf, this.inIdx + (2 * length));
                if (ch == '\0')
                {
                    this.inIdx += 2 * (length + 1);
                    return new string(this.charBuf, 0, length);
                }
                if (length >= this.charBuf.Length)
                {
                    char[] array = new char[2 * this.charBuf.Length];
                    this.charBuf.CopyTo(array, 0);
                    this.charBuf = array;
                }
                this.charBuf[length] = ch;
                length++;
            }
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.Symbol" />.
        /// </summary>
        /// <returns></returns>
        public Symbol ReadSymbol()
        {
            int id = this.ReadInt32();
            Symbol symbol = this.ToSymbol(id);
            if (symbol == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadSymbol: unknown symbol '" + id + "'");
            }
            return symbol;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.SymbolType" />.
        /// </summary>
        /// <returns></returns>
        public SymbolType ReadSymbolType()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadSymbolType: connection not set");
            }
            int num = this.ReadInt32();
            SymbolType type = this.connection.SymbolTypes[(SymbolTypeId) num];
            if (type == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadSymbolType: unknown symbol type '" + num + "'");
            }
            return type;
        }

        /// <summary>
        /// Read <see cref="T:iTrading.Core.Kernel.TimeInForce" />.
        /// </summary>
        /// <returns></returns>
        public TimeInForce ReadTimeInForce()
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadTimeInForce: connection not set");
            }
            int num = this.ReadInt32();
            TimeInForce force = this.connection.TimeInForces[(TimeInForceId) num];
            if (force == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.ReadTimeInForce: unknown time in force type '" + num + "'");
            }
            return force;
        }

        /// <summary>
        /// Reset out buffer.
        /// </summary>
        public void Reset()
        {
            this.outIdx = 0;
        }

        /// <summary>
        /// Reset in buffer.
        /// </summary>
        public void Reset(byte[] inBuf)
        {
            this.inBuf = inBuf;
            this.inIdx = 0;
        }

        /// <summary>
        /// Convert <see cref="T:iTrading.Core.Kernel.Order" /> to id.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual int ToId(Order order)
        {
            return 0;
        }

        /// <summary>
        /// Convert <see cref="T:iTrading.Core.Kernel.Request" /> to id.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual int ToId(Request request)
        {
            return 0;
        }

        /// <summary>
        /// Convert <see cref="T:iTrading.Core.Kernel.Request" /> to id.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        protected virtual int ToId(Symbol symbol)
        {
            return 0;
        }

        /// <summary>
        /// Convert id to <see cref="T:iTrading.Core.Kernel.Order" />.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Order ToOrder(int id)
        {
            return null;
        }

        /// <summary>
        /// Convert id to <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Request ToRequest(int id)
        {
            return null;
        }

        /// <summary>
        /// Convert id to <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Symbol ToSymbol(int id)
        {
            return null;
        }

        /// <summary>
        /// Write boolean.
        /// </summary>
        /// <param name="myValue"></param>
        public void Write(bool myValue)
        {
            this.Write(BitConverter.GetBytes(myValue));
        }

        /// <summary>
        /// Write DateTime.
        /// </summary>
        /// <param name="myValue"></param>
        public void Write(DateTime myValue)
        {
            this.Write(BitConverter.GetBytes(myValue.Ticks));
        }

        /// <summary>
        /// Write 8 byte floating point value.
        /// </summary>
        /// <param name="myValue"></param>
        public void Write(double myValue)
        {
            this.Write(BitConverter.GetBytes(myValue));
        }

        private void Write(byte[] array)
        {
            while ((this.outIdx + array.Length) >= this.outBuf.Length)
            {
                byte[] buffer = new byte[2 * this.outBuf.Length];
                this.outBuf.CopyTo(buffer, 0);
                this.outBuf = buffer;
            }
            array.CopyTo(this.outBuf, this.outIdx);
            this.outIdx += array.Length;
        }

        /// <summary>
        /// Write 4 byte integer.
        /// </summary>
        /// <param name="myValue"></param>
        public void Write(int myValue)
        {
            this.Write(BitConverter.GetBytes(myValue));
        }

        /// <summary>
        /// Write string.
        /// </summary>
        /// <param name="myValue"></param>
        public void Write(string myValue)
        {
            char[] chArray = myValue.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                this.Write(BitConverter.GetBytes(chArray[i]));
            }
            this.Write(BitConverter.GetBytes('\0'));
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.Account" />.
        /// </summary>
        /// <param name="account"></param>
        public void Write(Account account)
        {
            this.Write(account.Name);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.AccountItemType" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(AccountItemType type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.ActionType" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(ActionType type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.ConnectionStatus" />.
        /// </summary>
        /// <param name="connectionStatus"></param>
        public void Write(ConnectionStatus connectionStatus)
        {
            this.Write((int) connectionStatus.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.Currency" />.
        /// </summary>
        /// <param name="currency"></param>
        public void Write(iTrading.Core.Kernel.Currency currency)
        {
            this.Write((int) currency.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.CurrencyId" />.
        /// </summary>
        /// <param name="currencyId"></param>
        public void Write(CurrencyId currencyId)
        {
            this.Write((int) currencyId);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.Exchange" />.
        /// </summary>
        /// <param name="myValue"></param>
        public void Write(Exchange myValue)
        {
            this.Write((int) myValue.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.FeatureType" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(FeatureType type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.LicenseType" />.
        /// </summary>
        /// <param name="license"></param>
        public void Write(LicenseType license)
        {
            this.Write((int) license.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.MarketDataType" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(MarketDataType type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.MarketPosition" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(MarketPosition type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.ModeType" />.
        /// </summary>
        /// <param name="mode"></param>
        public void Write(ModeType mode)
        {
            this.Write((int) mode.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.NewsItemType" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(NewsItemType type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.OrderState" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(OrderState type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.OrderType" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(OrderType type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.ProviderType" />.
        /// </summary>
        /// <param name="provider"></param>
        public void Write(ProviderType provider)
        {
            this.Write((int) provider.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.Right" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(Right type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.SymbolType" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(SymbolType type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.TimeInForce" />.
        /// </summary>
        /// <param name="type"></param>
        public void Write(TimeInForce type)
        {
            this.Write((int) type.Id);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.Order" />.
        /// </summary>
        /// <param name="order"></param>
        public void WriteOrder(Order order)
        {
            int myValue = this.ToId(order);
            if (myValue < 0)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.Write: unknown order '" + order.Id + "'");
            }
            this.Write(myValue);
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        /// <param name="request"></param>
        public void WriteRequest(Request request)
        {
            int myValue = this.ToId(request);
            this.Write(myValue);
        }

        /// <summary>
        /// Serializes a <see cref="T:iTrading.Core.Kernel.ITradingSerializable" /> object. A type header is included. 
        /// The object may be deserialized by calling <see cref="M:TradeMagic.Data.Bytes.ReadSerializable" />.
        /// </summary>
        /// <param name="serializable"></param>
        public void WriteSerializable(ITradingSerializable serializable)
        {
            if (serializable == null)
            {
                this.Write(-1);
            }
            else
            {
                this.Write((int) serializable.ClassId);
                this.Write(serializable.Version);
                serializable.Serialize(this, serializable.Version);
            }
        }

        /// <summary>
        /// Write <see cref="T:iTrading.Core.Kernel.Symbol" />.
        /// </summary>
        /// <param name="symbol"></param>
        public void WriteSymbol(Symbol symbol)
        {
            int myValue = this.ToId(symbol);
            if (myValue < 0)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Bytes.Write: unknown symbol '" + symbol.Id + "'");
            }
            this.Write(myValue);
        }

        /// <summary>
        /// The actual connection.
        /// </summary>
        public iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }

        /// <summary>
        /// Size of header.
        /// </summary>
        public static int HeaderSize
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// Get the number of deserialized bytes.
        /// </summary>
        public int InLength
        {
            get
            {
                return this.inIdx;
            }
        }

        /// <summary>
        /// Output buffer array. Will have more bytes than necessary. Call <see cref="P:TradeMagic.Data.Bytes.OutLength" /> for the
        /// number of serialized bytes in the array.
        /// </summary>
        public byte[] Out
        {
            get
            {
                return this.outBuf;
            }
        }

        /// <summary>
        /// Get the number of serialized bytes.
        /// </summary>
        public int OutLength
        {
            get
            {
                return this.outIdx;
            }
        }
    }
}