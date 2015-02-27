using System.Windows.Forms;
using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;
    using iTrading.Core.Data;

    /// <summary>
    /// Represents a symbol. Some brokers (e.g. InteractiveBrokers) prefer the name "contract".
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("3BC248EE-A0FF-417f-886F-E2D4D6A1854E")]
    public class Symbol : Request, IComSymbol, IComparable, ITradingSerializable
    {
        private double commission;
        private string companyName;
        private iTrading.Core.Kernel.Currency currency;
        private string customText;
        private DividendDictionary dividends;
        private iTrading.Core.Kernel.Exchange exchange;
        private ExchangeDictionary exchanges;
        private DateTime expiry;
        private double margin;
        private iTrading.Core.Kernel.MarketData marketData;
        private iTrading.Core.Kernel.MarketDepth marketDepth;
        private string name;
        private double pointValue;
        private Hashtable providerNames;
        private iTrading.Core.Kernel.Right right;
        private int rolloverMonths;
        private double slippage;
        private SplitDictionary splits;
        private double strikePrice;
        private iTrading.Core.Kernel.SymbolType symbolType;
        private double tickSize;
        private string url;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public Symbol(Bytes bytes, int version) : base(bytes, version)
        {
            this.commission = 0.0;
            this.customText = "";
            this.dividends = new DividendDictionary();
            this.margin = 0.0;
            this.pointValue = 1.0;
            this.providerNames = new Hashtable();
            this.rolloverMonths = 0;
            this.slippage = 0.0;
            this.splits = new SplitDictionary();
            this.strikePrice = 0.0;
            this.tickSize = 0.0;
            this.url = "";
            if (version >= 2)
            {
                this.commission = bytes.ReadDouble();
            }
            this.companyName = bytes.ReadString();
            this.currency = bytes.ReadCurrency();
            this.customText = bytes.ReadString();
            this.exchange = bytes.ReadExchange();
            this.expiry = bytes.ReadDateTime();
            if (version >= 2)
            {
                this.margin = bytes.ReadDouble();
            }
            this.name = bytes.ReadString();
            this.pointValue = bytes.ReadDouble();
            this.right = bytes.ReadRight();
            this.rolloverMonths = bytes.ReadInt32();
            if (version >= 2)
            {
                this.slippage = bytes.ReadDouble();
            }
            this.strikePrice = bytes.ReadDouble();
            this.symbolType = bytes.ReadSymbolType();
            this.tickSize = bytes.ReadDouble();
            this.url = bytes.ReadString();
            this.providerNames = new Hashtable();
            int num = bytes.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                this.providerNames.Add(bytes.ReadProvider().Id, bytes.ReadString());
            }
            this.dividends = new DividendDictionary();
            num = bytes.ReadInt32();
            for (int j = 0; j < num; j++)
            {
                this.dividends.Add(bytes.ReadDateTime(), bytes.ReadDouble());
            }
            this.exchanges = new ExchangeDictionary();
            num = bytes.ReadInt32();
            for (int k = 0; k < num; k++)
            {
                this.exchanges.Add(bytes.ReadExchange());
            }
            this.splits = new SplitDictionary();
            num = bytes.ReadInt32();
            for (int m = 0; m < num; m++)
            {
                this.splits.Add(bytes.ReadDateTime(), bytes.ReadDouble());
            }
        }

        /// <summary>
        /// Create a symbol object. This symbol object is inactive, it may not be used to e.g. submit orders or
        /// subsribe to market data. Call <see cref="M:iTrading.Core.Kernel.Connection.GetSymbol(System.String,System.DateTime,iTrading.Core.Kernel.SymbolType,iTrading.Core.Kernel.Exchange,System.Double,iTrading.Core.Kernel.RightId,iTrading.Core.Kernel.LookupPolicyId)" /> to retrieve active symbol objects.
        /// </summary>
        /// <param name="currentConnection">The current connection. The connection it required for proper
        /// repository and cache sychronisation.</param>
        /// <param name="name">Broker independent base name of the symbol.</param>
        /// <param name="expiry">Expiry. Set <see cref="F:iTrading.Core.Kernel.Globals.MaxDate" /> for stocks. The date value is
        /// normalized and set to the 1st day of the expiry month.</param>
        /// <param name="currency">Currency for this symbol.</param>
        /// <param name="symbolType">Type of symbol.</param>
        /// <param name="exchange">Exchange</param>
        /// <param name="strikePrice">Strike price. Options only. Set to 0 for any other symbol type.</param>
        /// <param name="rightId">Option right. Set to <see cref="F:iTrading.Core.Kernel.RightId.Unknown" /> for any other symbol type.</param>
        /// <param name="tickSize">Ticksize. Make sure not to have 0 there.</param>
        /// <param name="pointValue">Value of a point. Make sure not have 0 there.</param>
        /// <param name="companyName">Descriptive company name of the symbol. Set to empty string if not privided.</param>
        /// <param name="exchanges">A dictionary for exchanges supporting the symbol. Set to NULL if this information is not provided.</param>
        /// <param name="splits">Stock splits. Set to NULL if this information is not provided.</param>
        /// <param name="dividends">Stock dividends. Set to NULL if this information is not provided.</param>
        public Symbol(Connection currentConnection, string name, DateTime expiry, iTrading.Core.Kernel.SymbolType symbolType, iTrading.Core.Kernel.Exchange exchange, double strikePrice, RightId rightId, iTrading.Core.Kernel.Currency currency, double tickSize, double pointValue, string companyName, ExchangeDictionary exchanges, SplitDictionary splits, DividendDictionary dividends) : base(currentConnection)
        {
            this.commission = 0.0;
            this.customText = "";
            this.dividends = new DividendDictionary();
            this.margin = 0.0;
            this.pointValue = 1.0;
            this.providerNames = new Hashtable();
            this.rolloverMonths = 0;
            this.slippage = 0.0;
            this.splits = new SplitDictionary();
            this.strikePrice = 0.0;
            this.tickSize = 0.0;
            this.url = "";
            this.expiry = ((symbolType.Id == SymbolTypeId.Future) || (symbolType.Id == SymbolTypeId.Option)) ? expiry : Globals.MaxDate;
            this.expiry = new DateTime(this.expiry.Year, this.expiry.Month, 1);
            this.companyName = companyName;
            this.currency = currency;
            this.exchange = exchange;
            this.exchanges = new ExchangeDictionary();
            this.marketData = null;
            this.marketDepth = null;
            this.name = name.ToUpper();
            this.pointValue = pointValue;
            this.right = iTrading.Core.Kernel.Right.All[(symbolType.Id != SymbolTypeId.Option) ? RightId.Unknown : rightId];
            this.strikePrice = (symbolType.Id != SymbolTypeId.Option) ? 0.0 : this.Round2TickSize(strikePrice);
            this.symbolType = symbolType;
            if (exchanges != null)
            {
                foreach (iTrading.Core.Kernel.Exchange exchange2 in exchanges.Values)
                {
                    this.exchanges.Add(exchange2);
                }
                if ((this.exchanges.Count == 0) || !this.exchanges.Contains(this.exchange))
                {
                    this.exchanges.Add(this.exchange);
                }
            }
            else
            {
                this.exchanges.Add(this.exchange);
            }
            if (dividends != null)
            {
                this.dividends = dividends;
            }
            if (splits != null)
            {
                this.splits = splits;
            }
            if (tickSize != 0.0)
            {
                this.tickSize = tickSize;
            }
            if (((symbolType.Id == SymbolTypeId.Future) || (symbolType.Id == SymbolTypeId.Option)) && ((expiry.Year == Globals.MaxDate.Year) && (expiry.Month == Globals.MaxDate.Month)))
            {
                if (DateTime.Now.Month < 3)
                {
                    this.expiry = new DateTime(DateTime.Now.Year, 3, 1);
                }
                else if (DateTime.Now.Month < 6)
                {
                    this.expiry = new DateTime(DateTime.Now.Year, 6, 1);
                }
                else if (DateTime.Now.Month < 9)
                {
                    this.expiry = new DateTime(DateTime.Now.Year, 9, 1);
                }
                else if (DateTime.Now.Month < 12)
                {
                    this.expiry = new DateTime(DateTime.Now.Year, 12, 1);
                }
                else
                {
                    this.expiry = new DateTime(DateTime.Now.Year + 1, 3, 1);
                }
            }
        }

        /// <summary>
        /// Compares to <see cref="T:iTrading.Core.Kernel.Symbol" /> objects.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if ((obj == null) || !(obj is Symbol))
            {
                return -1;
            }
            int num = this.Name.CompareTo(((Symbol) obj).Name);
            if (num == 0)
            {
                num = this.SymbolType.Id.CompareTo(((Symbol) obj).SymbolType.Id);
                if (num != 0)
                {
                    return num;
                }
                num = this.Exchange.Id.CompareTo(((Symbol) obj).Exchange.Id);
                if (num != 0)
                {
                    return num;
                }
                if ((this.SymbolType.Id == SymbolTypeId.Future) && (((num = this.Expiry.Year.CompareTo(((Symbol) obj).Expiry.Year)) != 0) || ((num = this.Expiry.Month.CompareTo(((Symbol) obj).Expiry.Month)) != 0)))
                {
                    return num;
                }
                if ((this.SymbolType.Id != SymbolTypeId.Option) || ((((num = this.Expiry.Year.CompareTo(((Symbol) obj).Expiry.Year)) == 0) && ((num = this.Expiry.Month.CompareTo(((Symbol) obj).Expiry.Month)) == 0)) && (((num = this.Right.Id.CompareTo(((Symbol) obj).Right.Id)) == 0) && ((num = this.StrikePrice.CompareTo(((Symbol) obj).StrikePrice)) == 0))))
                {
                    return 0;
                }
            }
            return num;
        }

        /// <summary>
        /// Checks if two symbols are identical.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return (((obj != null) && (obj is Symbol)) && (this == ((Symbol) obj)));
        }

        /// <summary>
        /// Formats price according the <see cref="P:iTrading.Core.Kernel.Symbol.TickSize" /> value.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public string FormatPrice(double price)
        {
            return this.FormatPrice(price, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Formats price according the <see cref="P:iTrading.Core.Kernel.Symbol.TickSize" /> value.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string FormatPrice(double price, IFormatProvider formatProvider)
        {
            string str = (price < 0.0) ? "-" : "";
            double d = Math.Abs(this.Round2TickSize(price));
            if (price == 0.0)
            {
                return price.ToString(formatProvider);
            }
            if (this.tickSize != 0.0)
            {
                if (this.tickSize == Globals.TickSize8)
                {
                    double num2 = Math.Floor(d);
                    double num3 = (d - num2) / Globals.TickSize8;
                    if (num2 == 0.0)
                    {
                        return (str + num3.ToString(formatProvider) + "/8");
                    }
                    return (str + num2.ToString(formatProvider) + ((num3 == 0.0) ? "" : (" " + num3.ToString(formatProvider) + "/8")));
                }
                if (this.tickSize == Globals.TickSize32)
                {
                    double num4 = Math.Floor(d);
                    double num5 = (d - num4) / Globals.TickSize32;
                    if (num4 == 0.0)
                    {
                        return (str + num5.ToString(formatProvider) + "/32");
                    }
                    return (str + num4.ToString(formatProvider) + ((num5 == 0.0) ? "" : (" " + ((num5 < 10.0) ? " " : "") + num5.ToString(formatProvider) + "/32")));
                }
                if (this.tickSize == Globals.TickSize64)
                {
                    double num6 = Math.Floor(d);
                    double num7 = (d - num6) / Globals.TickSize64;
                    if (num6 == 0.0)
                    {
                        return (str + num7.ToString(formatProvider) + "/64");
                    }
                    return (str + num6.ToString(formatProvider) + ((num7 == 0.0) ? "" : (" " + ((num7 < 10.0) ? " " : "") + num7.ToString(formatProvider) + "/64")));
                }
                if (this.tickSize == Globals.TickSize128)
                {
                    double num8 = Math.Floor(d);
                    double num9 = (d - num8) / Globals.TickSize128;
                    if (num8 == 0.0)
                    {
                        return (str + num9.ToString(formatProvider) + "/128");
                    }
                    return (str + num8.ToString(formatProvider) + ((num9 == 0.0) ? "" : (" " + ((num9 < 100.0) ? (" " + ((num9 < 10.0) ? " " : "")) : "") + num9.ToString(formatProvider) + "/128")));
                }
                if (this.tickSize == 0.005)
                {
                    return (str + d.ToString("0.000", formatProvider));
                }
                if (this.tickSize == 0.025)
                {
                    return (str + d.ToString("0.000", formatProvider));
                }
                if (this.tickSize == 0.001)
                {
                    return (str + d.ToString("0.000", formatProvider));
                }
                if (this.tickSize == 0.01)
                {
                    return (str + d.ToString("0.00", formatProvider));
                }
                if (this.tickSize == 0.25)
                {
                    return (str + d.ToString("0.00", formatProvider));
                }
                if (this.tickSize == 0.5)
                {
                    return (str + d.ToString("0.0", formatProvider));
                }
                if (this.tickSize < 1E-06)
                {
                    return (str + d.ToString("0.0000000", formatProvider));
                }
                if (this.tickSize < 1E-05)
                {
                    return (str + d.ToString("0.000000", formatProvider));
                }
                if (this.tickSize < 0.0001)
                {
                    return (str + d.ToString("0.00000", formatProvider));
                }
                if (this.tickSize < 0.001)
                {
                    return (str + d.ToString("0.0000", formatProvider));
                }
                if (this.tickSize < 0.01)
                {
                    return (str + d.ToString("0.000", formatProvider));
                }
                if (this.tickSize < 0.1)
                {
                    return (str + d.ToString("0.00", formatProvider));
                }
                if (this.tickSize < 1.0)
                {
                    return (str + d.ToString("0.0", formatProvider));
                }
            }
            return (str + d.ToString(formatProvider));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>Hash code value</returns>
        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        /// <summary>
        /// Get the provider dependant symbol name.
        /// </summary>
        /// <param name="broker"></param>
        /// <returns></returns>
        public string GetProviderName(ProviderTypeId broker)
        {
            string str = (string) this.providerNames[broker];
            if ((str != null) && (str.Length > 0))
            {
                return str;
            }
            return this.name;
        }

        /// <summary>
        /// Compares two symbols. Opposed to <see cref="M:iTrading.Core.Kernel.Symbol.CompareTo(System.Object)" /> the exchange is NOT significant. 
        /// This is required to e.g. build account positions.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool IsEqual(Symbol symbol)
        {
            if (this.name.CompareTo(symbol.name) != 0)
            {
                return false;
            }
            if (this.symbolType.Id.CompareTo(symbol.symbolType.Id) != 0)
            {
                return false;
            }
            if ((this.symbolType.Id == SymbolTypeId.Future) && ((this.expiry.Year.CompareTo(symbol.expiry.Year) != 0) || (this.expiry.Month.CompareTo(symbol.expiry.Month) != 0)))
            {
                return false;
            }
            return ((this.SymbolType.Id != SymbolTypeId.Option) || (((this.expiry.Year.CompareTo(symbol.expiry.Year) == 0) && (this.expiry.Month.CompareTo(symbol.Expiry.Month) == 0)) && ((this.right.Id.CompareTo(symbol.Right.Id) == 0) && (this.strikePrice.CompareTo(symbol.StrikePrice) == 0))));
        }

        /// <summary>
        /// Compares two symbols. Opposed to <see cref="M:iTrading.Core.Kernel.Symbol.CompareTo(System.Object)" /> exchange and expiry are NOT significant. 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool IsEqualFamily(Symbol symbol)
        {
            if (this.name.CompareTo(symbol.name) != 0)
            {
                return false;
            }
            if (this.symbolType.Id.CompareTo(symbol.symbolType.Id) != 0)
            {
                return false;
            }
            return ((this.SymbolType.Id != SymbolTypeId.Option) || ((this.right.Id.CompareTo(symbol.Right.Id) == 0) && (this.strikePrice.CompareTo(symbol.StrikePrice) == 0)));
        }

        /// <summary>
        /// Load quote file to update the repository.
        /// </summary>
        /// <param name="path">Path of file to read the quote data from.</param>
        /// <param name="separator">Character to seperate the input fields.</param>
        /// <param name="periodTypeId"></param>
        /// <returns></returns>
        public void LoadQuotes(string path, char separator, PeriodTypeId periodTypeId)
        {
            CultureInfo provider = new CultureInfo("en-US");
            char[] chArray = new char[] { separator };
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path);
            }
            catch (Exception)
            {
                throw new TMException(ErrorCode.Panic, "Unable to open file '" + path + "'");
            }
            string str = "";
            Quotes quotes = new Quotes(this, Globals.MinDate, Globals.MaxDate, new Period(periodTypeId, 1), false);
            for (int i = 1; (str = reader.ReadLine()) != null; i++)
            {
                string[] strArray = str.Split(chArray);
                if ((periodTypeId == PeriodTypeId.Day) && (strArray.Length != 6))
                {
                    throw new TMException(ErrorCode.Panic, "Unexpected number of fields in line '" + i + "', should be 6");
                }
                if ((periodTypeId != PeriodTypeId.Day) && (strArray.Length != 7))
                {
                    throw new TMException(ErrorCode.Panic, "Unexpected number of fields in line '" + i + "', should be 7");
                }
                try
                {
                    DateTime time = (periodTypeId == PeriodTypeId.Day) ? new DateTime(Convert.ToInt32(strArray[0].Substring(0, 4)), Convert.ToInt32(strArray[0].Substring(4, 2)), Convert.ToInt32(strArray[0].Substring(6, 2))) : new DateTime(Convert.ToInt32(strArray[0].Substring(0, 4)), Convert.ToInt32(strArray[0].Substring(4, 2)), Convert.ToInt32(strArray[0].Substring(6, 2)), Convert.ToInt32(strArray[1].Substring(0, 2)), Convert.ToInt32(strArray[1].Substring(2, 2)), Convert.ToInt32(strArray[1].Substring(4, 2)));
                    if (quotes.Bars.Count > 0)
                    {
                        if ((periodTypeId == PeriodTypeId.Minute) && (quotes.Bars[quotes.Bars.Count - 1].Time >= time))
                        {
                            throw new TMException(ErrorCode.Panic, "Time stamp line '" + i + "' is smaller/equal than timestamp of previous line");
                        }
                        if ((periodTypeId == PeriodTypeId.Day) && (quotes.Bars[quotes.Bars.Count - 1].Time.Date >= time.Date))
                        {
                            throw new TMException(ErrorCode.Panic, "Time stamp line '" + i + "' is smaller/equal than timestamp of previous line");
                        }
                        if ((periodTypeId == PeriodTypeId.Tick) && (quotes.Bars[quotes.Bars.Count - 1].Time > time))
                        {
                            throw new TMException(ErrorCode.Panic, "Time stamp line '" + i + "' is smaller than timestamp of previous line");
                        }
                    }
                    int num2 = (periodTypeId == PeriodTypeId.Day) ? 0 : 1;
                    double open = Convert.ToDouble(strArray[1 + num2], provider);
                    double high = Convert.ToDouble(strArray[2 + num2], provider);
                    double low = Convert.ToDouble(strArray[3 + num2], provider);
                    double close = Convert.ToDouble(strArray[4 + num2], provider);
                    int volume = Convert.ToInt32(strArray[5 + num2], provider);
                    if ((((((open <= 0.0) || (high <= 0.0)) || ((low <= 0.0) || (close <= 0.0))) || ((this.SymbolType.Id == SymbolTypeId.Index) ? (volume < 0) : (volume <= 0))) || ((open < low) || (high < low))) || (((close < low) || (open > high)) || ((low > high) || (close > high))))
                    {
                        throw new TMException(ErrorCode.Panic, "Illegal quote value in '" + i + "'");
                    }
                    quotes.Bars.Add(open, high, low, close, time, volume, false);
                }
                catch (Exception exception)
                {
                    throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Format error in line '", i, "': ", exception.Message }));
                }
            }
            if (quotes.Bars.Count > 0)
            {
                quotes.from = quotes.Bars[0].Time.Date;
                quotes.to = quotes.Bars[quotes.Bars.Count - 1].Time.Date;
            }
            Globals.DB.Update(quotes, true);
            reader.Close();
        }

        /// <summary>
        /// Get nearest expiry date.
        /// </summary>
        /// <param name="afterDate"></param>
        /// <returns></returns>
        public DateTime NearestExpiry(DateTime afterDate)
        {
            int rolloverMonths = this.rolloverMonths;
            if ((this.symbolType.Id != SymbolTypeId.Future) && (this.symbolType.Id != SymbolTypeId.Option))
            {
                return afterDate;
            }
            if (rolloverMonths == 0)
            {
                rolloverMonths = 3;
            }
            return afterDate.AddMonths((rolloverMonths - afterDate.Month) + (((int) (((double) afterDate.Month) / ((double) rolloverMonths))) * rolloverMonths));
        }

        /// <summary>
        /// Checks if two symbols are identical.
        /// </summary>
        /// <param name="symbol1"></param>
        /// <param name="symbol2"></param>
        /// <returns></returns>
        public static bool operator ==(Symbol symbol1, Symbol symbol2)
        {
            return (((symbol1 == null) && (symbol2 == null)) || (((symbol1 != null) && (symbol2 != null)) && (symbol1.CompareTo(symbol2) == 0)));
        }

        /// <summary></summary>
        /// <param name="symbol1"></param>
        /// <param name="symbol2"></param>
        /// <returns></returns>
        public static bool operator >(Symbol symbol1, Symbol symbol2)
        {
            return (symbol1.CompareTo(symbol2) > 0);
        }

        /// <summary>
        /// Checks if two symbols are different.
        /// </summary>
        /// <param name="symbol1"></param>
        /// <param name="symbol2"></param>
        /// <returns></returns>
        public static bool operator !=(Symbol symbol1, Symbol symbol2)
        {
            return !(symbol1 == symbol2);
        }

        /// <summary></summary>
        /// <param name="symbol1"></param>
        /// <param name="symbol2"></param>
        /// <returns></returns>
        public static bool operator <(Symbol symbol1, Symbol symbol2)
        {
            return (symbol1.CompareTo(symbol2) < 0);
        }

        /// <summary>
        /// Retrieves a quotes object from the provider/repository.
        /// Quotes only can be retrieved active symbols, that is symbols created by
        /// <see cref="M:iTrading.Core.Kernel.Connection.GetSymbol(System.String,System.DateTime,iTrading.Core.Kernel.SymbolType,iTrading.Core.Kernel.Exchange,System.Double,iTrading.Core.Kernel.RightId,iTrading.Core.Kernel.LookupPolicyId)" />.
        /// Please note: Quotes only can be retrieved date-wise. Intraday timestamp will be ignored.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="period"></param>
        /// <param name="lookupPolicyId">Only options <see cref="F:iTrading.Core.Kernel.LookupPolicyId.ProviderOnly" />,
        /// <see cref="F:iTrading.Core.Kernel.LookupPolicyId.RepositoryOnly" /> and <see cref="F:iTrading.Core.Kernel.LookupPolicyId.RepositoryAndProvider" />
        /// are supported.</param>
        /// <param name="splitAdjusted">Effective for stocks only.</param>
        /// <param name="customLink"></param>
        public void RequestQuotes(DateTime fromDate, DateTime toDate, Period period, bool splitAdjusted, LookupPolicyId lookupPolicyId, object customLink)
        {
            if (base.connection == null)
            {
                throw new TMException(ErrorCode.Panic, "Quotes only can be retrieved for active symbols");
            }
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.connection.IdPlus, ") Cbi.Symbol.RequestQuotes1: symbol='", this.FullName, "' from=", fromDate.Date, " to=", toDate.Date, " period=", period.ToString(), " lookup=", lookupPolicyId }));
            }
            if (fromDate.Date > toDate.Date)
            {
                throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Cbi.Symbol.RequestQuotes: from-date (", fromDate.Date, ") has to be smaller than to-date (", toDate.Date, ")" }));
            }
            if (toDate.Date > base.connection.Now.Date)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Symbol.RequestQuotes: historical data only can be retrieved up to the current day");
            }
            if (this.symbolType.Id != SymbolTypeId.Stock)
            {
                splitAdjusted = false;
            }
            Quotes quotes = new Quotes(this, fromDate, toDate, period, splitAdjusted);
            Quotes quotes2 = new Quotes(this, fromDate, toDate, new Period(period.Id, 1), false);
            int num = 2;
            if (lookupPolicyId != LookupPolicyId.ProviderOnly)
            {
                num = Globals.DB.Select(quotes2);
            }
            if (((num == 0) && (base.connection.Now.Date == toDate.Date)) && (lookupPolicyId != LookupPolicyId.RepositoryOnly))
            {
                num = 1;
            }
            foreach (Bar bar in quotes2.Bars)
            {
                double num2 = splitAdjusted ? this.Splits.GetSplitFactor(bar.Time.Date) : 1.0;
                quotes.Bars.Add(bar.Open / num2, bar.High / num2, bar.Low / num2, bar.Close / num2, bar.Time, bar.Volume, false);
            }
            quotes.CustomLink = customLink;
            Quotes adapterQuotes = null;
            if ((((((num == 0) || (lookupPolicyId == LookupPolicyId.RepositoryOnly)) || (base.Connection.quotes == null)) || ((period.Id == PeriodTypeId.Day) && (base.connection.FeatureTypes[FeatureTypeId.QuotesDaily] == null))) || ((period.Id == PeriodTypeId.Minute) && (base.connection.FeatureTypes[FeatureTypeId.Quotes1Minute] == null))) || ((period.Id == PeriodTypeId.Tick) && (base.connection.FeatureTypes[FeatureTypeId.QuotesTick] == null)))
            {
                base.connection.ProcessEventArgs(new BarUpdateEventArgs(base.connection, ErrorCode.NoError, "", Operation.Insert, quotes, 0, quotes.Bars.Count - 1));
            }
            else
            {
                if (num == 1)
                {
                    adapterQuotes = new Quotes(this, quotes.Bars[quotes.Bars.Count - 1].Time.Date, toDate, new Period(period.Id, 1), false);
                }
                else
                {
                    adapterQuotes = new Quotes(this, fromDate, toDate, new Period(period.Id, 1), false);
                }
                lock (base.connection.quotesRequests)
                {
                    base.connection.quotesRequests.Add(new QuotesRequest(adapterQuotes, quotes));
                    if (base.connection.quotesRequests.Count == 1)
                    {
                        if (Globals.TraceSwitch.Quotes)
                        {
                            Trace.WriteLine(string.Concat(new object[] { "(", base.connection.IdPlus, ") Cbi.Symbol.RequestQuotes2: symbol='", this.FullName, "' from=", fromDate.Date, " to=", toDate.Date, " period=", period.ToString(), " lookup=", lookupPolicyId }));
                        }
                        base.connection.SynchronizeInvoke.AsyncInvoke(new MethodInvoker(base.connection.GetQuotesNow), null);
                    }
                }
            }
        }

        /// <summary>
        /// Round price to <see cref="P:iTrading.Core.Kernel.Symbol.TickSize" />.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public double Round2TickSize(double price)
        {
            return Round2TickSize(price, this.TickSize);
        }

        /// <summary>
        /// Round price to ticksize value.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="tickSize"></param>
        /// <returns></returns>
        public static double Round2TickSize(double price, double tickSize)
        {
            if (tickSize == 0.0)
            {
                return price;
            }
            int num = (price < 0.0) ? -1 : 1;
            return (num * Math.Round((double) (Math.Round((double) (Math.Abs(price) / tickSize)) * tickSize), 7));
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            lock (this)
            {
                base.Serialize(bytes, version);
                if (version >= 2)
                {
                    bytes.Write(this.commission);
                }
                bytes.Write(this.companyName);
                bytes.Write(this.currency);
                bytes.Write(this.customText);
                bytes.Write(this.exchange);
                bytes.Write(this.expiry);
                if (version >= 2)
                {
                    bytes.Write(this.margin);
                }
                bytes.Write(this.name);
                bytes.Write(this.pointValue);
                bytes.Write(this.right);
                bytes.Write(this.rolloverMonths);
                if (version >= 2)
                {
                    bytes.Write(this.slippage);
                }
                bytes.Write(this.strikePrice);
                bytes.Write(this.symbolType);
                bytes.Write(this.tickSize);
                bytes.Write(this.url);
                lock (this.providerNames)
                {
                    bytes.Write(this.providerNames.Count);
                    foreach (ProviderTypeId id in this.providerNames.Keys)
                    {
                        bytes.Write((int) id);
                        bytes.Write((string) this.providerNames[id]);
                    }
                }
                lock (this.dividends)
                {
                    bytes.Write(this.dividends.Count);
                    foreach (DateTime time in this.dividends.Keys)
                    {
                        bytes.Write(time);
                        bytes.Write(this.dividends[time]);
                    }
                }
                lock (this.exchanges)
                {
                    bytes.Write(this.exchanges.Count);
                    foreach (iTrading.Core.Kernel.Exchange exchange in this.exchanges.Values)
                    {
                        bytes.Write(exchange);
                    }
                }
                lock (this.splits)
                {
                    bytes.Write(this.splits.Count);
                    foreach (DateTime time2 in this.splits.Keys)
                    {
                        bytes.Write(time2);
                        bytes.Write(this.splits[time2]);
                    }
                }
            }
        }

        /// <summary>
        /// Set the broker dependant symbol name.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerName"></param>
        public void SetProviderName(ProviderTypeId provider, string providerName)
        {
            this.providerNames[provider] = providerName;
        }

        /// <summary>
        /// Symbol name including exchange.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (this.FullName + " " + this.Exchange.Name);
        }

        internal void Update(Symbol symbol)
        {
            lock (this)
            {
                this.commission = symbol.commission;
                this.currency = symbol.currency;
                this.companyName = symbol.companyName;
                this.margin = symbol.margin;
                this.pointValue = symbol.pointValue;
                this.slippage = symbol.slippage;
                this.tickSize = symbol.tickSize;
                lock (symbol.dividends)
                {
                    this.dividends.Clear();
                    foreach (DateTime time in symbol.dividends.Keys)
                    {
                        this.dividends.Add(time, symbol.dividends[time]);
                    }
                }
                lock (symbol.exchanges)
                {
                    this.exchanges.Clear();
                    foreach (iTrading.Core.Kernel.Exchange exchange in symbol.exchanges.Values)
                    {
                        this.exchanges.Add(exchange);
                    }
                }
                lock (symbol.splits)
                {
                    this.splits.Clear();
                    foreach (DateTime time2 in symbol.splits.Keys)
                    {
                        this.splits.Add(time2, symbol.splits[time2]);
                    }
                }
                foreach (ProviderType type in ProviderType.All.Values)
                {
                    this.SetProviderName(type.Id, symbol.GetProviderName(type.Id));
                }
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.Symbol.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.Symbol;
            }
        }

        /// <summary>
        /// Commission for this symbol. Used for simulation. Overrides the global setting.
        /// </summary>
        public double Commission
        {
            get
            {
                return this.commission;
            }
            set
            {
                this.commission = value;
            }
        }

        /// <summary>
        /// Get broker dependent symbol description.
        /// </summary>
        public string CompanyName
        {
            get
            {
                if ((this.companyName != null) && (this.companyName.Length != 0))
                {
                    return this.companyName;
                }
                return this.FullName;
            }
            set
            {
                this.companyName = value;
            }
        }

        /// <summary>
        /// The currency of the symbol.
        /// </summary>
        public iTrading.Core.Kernel.Currency Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
            }
        }

        /// <summary>
        /// Get/set a custom text.
        /// </summary>
        public string CustomText
        {
            get
            {
                if (this.customText != null)
                {
                    return this.customText;
                }
                return "";
            }
            set
            {
                this.customText = value;
            }
        }

        /// <summary>
        /// The dictionary of dividends. 
        /// Please note: This dictionary only is filled for stocks.
        /// Not all brokers support this feature. You may exprience, that <see cref="P:iTrading.Core.Kernel.Symbol.Dividends" />
        /// dictionary is empty.
        /// </summary>
        public DividendDictionary Dividends
        {
            get
            {
                return this.dividends;
            }
        }

        /// <summary>
        /// Identifies the exchange / order routing system of the current symbol.
        /// </summary>
        public iTrading.Core.Kernel.Exchange Exchange
        {
            get
            {
                return this.exchange;
            }
        }

        /// <summary>
        /// The dictionary of exchanges where this symbol is tradable. 
        /// Please note: Not all brokers support this feature. You may exprience, that <see cref="P:iTrading.Core.Kernel.Symbol.Exchanges" />
        /// dictionary only holds the <see cref="P:iTrading.Core.Kernel.Symbol.Exchange" /> exchange.
        /// </summary>
        public ExchangeDictionary Exchanges
        {
            get
            {
                return this.exchanges;
            }
        }

        /// <summary>
        /// Expiry date of the current symbol. Used only for futures.
        /// </summary>
        public DateTime Expiry
        {
            get
            {
                return this.expiry;
            }
        }

        /// <summary>
        /// Get the format string according the <see cref="P:iTrading.Core.Kernel.Symbol.TickSize" /> value.
        /// </summary>
        public string FormatString
        {
            get
            {
                if (this.tickSize == Globals.TickSize8)
                {
                    return "0.000";
                }
                if (this.tickSize == Globals.TickSize32)
                {
                    return "0.000000";
                }
                if (this.tickSize == Globals.TickSize64)
                {
                    return "0.0000000";
                }
                if (this.tickSize == Globals.TickSize128)
                {
                    return "0.00000000";
                }
                if (this.tickSize == 0.005)
                {
                    return "0.000";
                }
                if (this.tickSize == 0.025)
                {
                    return "0.000";
                }
                if (this.tickSize == 0.001)
                {
                    return "0.000";
                }
                if (this.tickSize == 0.01)
                {
                    return "0.00";
                }
                if (this.tickSize == 0.25)
                {
                    return "0.00";
                }
                if (this.tickSize == 0.5)
                {
                    return "0.0";
                }
                if (this.tickSize < 1E-06)
                {
                    return "0.0000000";
                }
                if (this.tickSize < 1E-05)
                {
                    return "0.000000";
                }
                if (this.tickSize < 0.0001)
                {
                    return "0.00000";
                }
                if (this.tickSize < 0.001)
                {
                    return "0.0000";
                }
                if (this.tickSize < 0.01)
                {
                    return "0.000";
                }
                if (this.tickSize < 0.1)
                {
                    return "0.00";
                }
                if (this.tickSize < 1.0)
                {
                    return "0.0";
                }
                return "0";
            }
        }

        /// <summary>
        /// Returns the full name.
        /// </summary>
        public string FullName
        {
            get
            {
                string str = "";
                if (this.symbolType.Id == SymbolTypeId.Index)
                {
                    str = "^";
                }
                else if (this.symbolType.Id == SymbolTypeId.Option)
                {
                    str = "+";
                }
                string str2 = "";
                if (this.symbolType.Id == SymbolTypeId.Option)
                {
                    str2 = " " + ((this.right.Id == RightId.Call) ? "C" : "P") + " " + this.strikePrice.ToString(Connection.cultureInfo);
                }
                string str3 = "";
                if ((this.symbolType.Id == SymbolTypeId.Future) || (this.symbolType.Id == SymbolTypeId.Option))
                {
                    str3 = " " + ((this.Expiry == Globals.ContinousContractExpiry) ? "##/##" : (this.Expiry.ToString("MM") + "-" + this.Expiry.ToString("yy")));
                }
                return (str + this.Name + str3 + str2);
            }
        }

        /// <summary>
        /// Margin requirement for this symbol. Used for simulations. Overrídes the global setting.
        /// Note: Applies to futures and options only.
        /// </summary>
        public double Margin
        {
            get
            {
                return this.margin;
            }
            set
            {
                this.margin = value;
            }
        }

        /// <summary>
        /// Returns an object to handle the <see cref="T:iTrading.Core.Kernel.MarketDataEventArgs" /> data stream for this object.
        /// </summary>
        /// <returns>Object for handling the data stream.</returns>
        public iTrading.Core.Kernel.MarketData MarketData
        {
            get
            {
                lock (this)
                {
                    if (this.marketData == null)
                    {
                        this.marketData = new iTrading.Core.Kernel.MarketData(base.Connection, this);
                    }
                    return this.marketData;
                }
            }
        }

        /// <summary>
        /// Returns an object to handle the <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> data stream for this object.
        /// </summary>
        /// <returns>Object for handling the data stream.</returns>
        public iTrading.Core.Kernel.MarketDepth MarketDepth
        {
            get
            {
                lock (this)
                {
                    if (this.marketDepth == null)
                    {
                        this.marketDepth = new iTrading.Core.Kernel.MarketDepth(base.Connection, this);
                    }
                    return this.marketDepth;
                }
            }
        }

        /// <summary>
        /// Name of identifier of the current symbol.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name != null)
                {
                    return this.name;
                }
                return "";
            }
        }

        /// <summary>
        /// To indicate that a provider does not support this symbol.
        /// </summary>
        public string NoProviderName
        {
            get
            {
                return "-";
            }
        }

        /// <summary>
        /// Value of one point (e.g. $50 for ES). <see cref="P:iTrading.Core.Kernel.Symbol.PointValue" /> = 1 for stocks.
        /// </summary>
        public double PointValue
        {
            get
            {
                return this.pointValue;
            }
        }

        /// <summary>
        /// Options only.
        /// </summary>
        public iTrading.Core.Kernel.Right Right
        {
            get
            {
                return this.right;
            }
        }

        /// <summary>
        /// Get/set the number of months for rollover, e.g. "ES" rolls over every 3 months.
        /// </summary>
        public int RolloverMonths
        {
            get
            {
                return this.rolloverMonths;
            }
            set
            {
                this.rolloverMonths = value;
            }
        }

        /// <summary>
        /// Slippage as ticks per side for this symbol. Used for simulation. Overrides the global setting.
        /// </summary>
        public double Slippage
        {
            get
            {
                return this.slippage;
            }
            set
            {
                this.slippage = value;
            }
        }

        /// <summary>
        /// The dictionary of stock splits. 
        /// Please note: This dictionary only is filled for stocks.
        /// Not all brokers support this feature. You may exprience, that <see cref="P:iTrading.Core.Kernel.Symbol.Splits" />
        /// dictionary is empty.
        /// </summary>
        public SplitDictionary Splits
        {
            get
            {
                return this.splits;
            }
        }

        /// <summary>
        /// Strike price. Options only.
        /// </summary>
        public double StrikePrice
        {
            get
            {
                return this.strikePrice;
            }
        }

        /// <summary>
        /// Identifies the type of a symbol. 
        /// </summary>
        public iTrading.Core.Kernel.SymbolType SymbolType
        {
            get
            {
                return this.symbolType;
            }
        }

        /// <summary>
        /// Min. size of a tick.
        /// </summary>
        public double TickSize
        {
            get
            {
                return this.tickSize;
            }
        }

        /// <summary>
        /// Get/set url to symbol specifications.
        /// </summary>
        public string Url
        {
            get
            {
                if (this.url != null)
                {
                    return this.url;
                }
                return "";
            }
            set
            {
                this.url = value;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public override int Version
        {
            get
            {
                return 2;
            }
        }
    }
}

