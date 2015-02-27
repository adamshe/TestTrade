

namespace iTrading.Core.Data
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Interface;

    /// <summary>
    /// A collection of price values (open/high/low/close).
    /// </summary>
    [Guid("75AD0F9C-5F10-47a9-949E-EF539D52FD71"), ClassInterface(ClassInterfaceType.None)]
    public class PriceSeries : IComPriceSeries, IDoubleSeries
    {
        private PriceTypeId priceTypeId;
        private Quotes quotes;

        internal PriceSeries(Quotes quotes, PriceTypeId priceTypeId)
        {
            this.priceTypeId = priceTypeId;
            this.quotes = quotes;
        }

        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count
        {
            get
            {
                return this.quotes.Bars.Count;
            }
        }

        /// <summary>
        /// Get Open/High/Low/Close value at index.
        /// </summary>
        public double this[int index]
        {
            get
            {
                switch (this.priceTypeId)
                {
                    case PriceTypeId.Close:
                        return this.quotes.Bars[index].Close;

                    case PriceTypeId.High:
                        return this.quotes.Bars[index].High;

                    case PriceTypeId.Low:
                        return this.quotes.Bars[index].Low;

                    case PriceTypeId.Median:
                        return ((this.quotes.Bars[index].High + this.quotes.Bars[index].Low) / 2.0);

                    case PriceTypeId.Open:
                        return this.quotes.Bars[index].Open;

                    case PriceTypeId.Typical:
                        return (((this.quotes.Bars[index].Low + this.quotes.Bars[index].High) + this.quotes.Bars[index].Close) / 3.0);
                }
                throw new TMException(ErrorCode.Panic, "Core.PriceSeries.Item: unexpected priceTypeId value " + ((int) this.priceTypeId));
            }
        }
    }
}

