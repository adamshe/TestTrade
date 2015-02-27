using iTrading.Core.Data;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Symbol" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("B10A90ED-08F7-48d4-BB67-45C5692F4D3A")]
    public interface IComSymbol
    {
        /// <summary>
        /// Commission for this symbol. Used for simulation. Overrides the global setting.
        /// </summary>
        double Commission { get; set; }
        /// <summary>
        /// Get provider dependent symbol description.
        /// Equals Name if not supported.
        /// </summary>
        string CompanyName { get; set; }
        /// <summary>
        /// <see cref="T:iTrading.Core.Kernel.Connection" /> where market data is requested from.
        /// </summary>
        iTrading.Core.Kernel.Connection Connection { get; }
        /// <summary>
        /// The currency of the symbol.
        /// </summary>
        iTrading.Core.Kernel.Currency Currency { get; set; }
        /// <summary>
        /// <see cref="P:iTrading.Core.Kernel.Request.CustomLink" />. This property may be used to attach any object to the request.
        /// </summary>
        object CustomLink { get; set; }
        /// <summary>
        /// Get/set a custom text.
        /// </summary>
        string CustomText { get; set; }
        /// <summary>
        /// Identifies the <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Checks if two symbols are identical.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Equals(object obj);
        /// <summary>
        /// Identifies the exchange value of the current symbol.
        /// </summary>
        iTrading.Core.Kernel.Exchange Exchange { get; }
        /// <summary>
        /// The dictionary of exchanges where this symbol is tradable. 
        /// Please note: Not all providers support this feature. You may exprience, that <see cref="P:iTrading.Core.Kernel.Symbol.Exchanges" />
        /// dictionary only holds the <see cref="P:iTrading.Core.Kernel.Symbol.Exchange" /> exchange.
        /// </summary>
        ExchangeDictionary Exchanges { get; }
        /// <summary>
        /// Expiry date of the current symbol. Used only for futures.
        /// </summary>
        DateTime Expiry { get; }
        /// <summary>
        /// Formats price according the <see cref="P:iTrading.Core.Kernel.Symbol.TickSize" /> value.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        string FormatPrice(double price);
        /// <summary>
        /// Get the format string according the <see cref="P:iTrading.Core.Interface.IComSymbol.TickSize" /> value.
        /// </summary>
        string FormatString { get; }
        /// <summary>
        /// Returns the full name.
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// Get the provider dependant symbol name.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        string GetProviderName(ProviderTypeId provider);
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>Hash code value</returns>
        int GetHashCode();
        /// <summary>
        /// Compares two symbols. Opposed to <see cref="M:iTrading.Core.Kernel.Symbol.CompareTo(System.Object)" /> the exchange is NOT significant. 
        /// This is required to e.g. build account positions.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        bool IsEqual(Symbol symbol);
        /// <summary>
        /// Margin requirement for this symbol. Used for simulations. Overrídes the global setting.
        /// Note: Applies to futures and options only.
        /// </summary>
        double Margin { get; set; }
        /// <summary>
        /// Returns an object to handle the <see cref="T:iTrading.Core.Kernel.MarketDataEventArgs" /> data stream for this object.
        /// </summary>
        /// <returns>Object for handling the data stream.</returns>
        iTrading.Core.Kernel.MarketData MarketData { get; }
        /// <summary>
        /// Returns an object to handle the <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> data stream for this object.
        /// </summary>
        /// <returns>Object for handling the data stream.</returns>
        iTrading.Core.Kernel.MarketDepth MarketDepth { get; }
        /// <summary>
        /// Name of identifier of the current symbol.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Get nearest expiry date.
        /// </summary>
        /// <param name="afterDate"></param>
        /// <returns></returns>
        DateTime NearestExpiry(DateTime afterDate);
        /// <summary>
        /// To indicate that a provider does not support this symbol.
        /// </summary>
        string NoProviderName { get; }
        /// <summary>
        /// Value of one point (e.g. $50 for ES). <see cref="P:iTrading.Core.Kernel.Symbol.PointValue" /> = 1 for stocks.
        /// </summary>
        double PointValue { get; }
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
        void RequestQuotes(DateTime fromDate, DateTime toDate, Period period, bool splitAdjusted, LookupPolicyId lookupPolicyId, object customLink);
        /// <summary>
        /// Slippage as ticks per side for this symbol. Used for simulation. Overrides the global setting.
        /// </summary>
        double Slippage { get; set; }
        /// <summary>
        /// Options only.
        /// </summary>
        iTrading.Core.Kernel.Right Right { get; }
        /// <summary>
        /// Get/set the number of months for rollover, e.g. "ES" rolls over every 3 months.
        /// </summary>
        int RolloverMonths { get; set; }
        /// <summary>
        /// Align price to <see cref="P:iTrading.Core.Kernel.Symbol.TickSize" />.
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        double Round2TickSize(double price);
        /// <summary>
        /// Set the provider dependant symbol name.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerName"></param>
        void SetProviderName(ProviderTypeId provider, string providerName);
        /// <summary>
        /// Strike price. Options only.
        /// </summary>
        double StrikePrice { get; }
        /// <summary>
        /// Identifies the type of a symbol. 
        /// </summary>
        iTrading.Core.Kernel.SymbolType SymbolType { get; }
        /// <summary>
        /// Min. size of a tick.
        /// </summary>
        double TickSize { get; }
        /// <summary>
        /// Get/set url to symbol specifications.
        /// </summary>
        string Url { get; set; }
    }
}

