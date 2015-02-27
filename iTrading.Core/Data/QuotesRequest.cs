namespace iTrading.Core.Data
{
    using System;

    internal class QuotesRequest
    {
        internal Quotes adapterQuotes;
        internal Quotes quotes;

        internal QuotesRequest(Quotes adapterQuotes, Quotes quotes)
        {
            this.adapterQuotes = adapterQuotes;
            this.quotes = quotes;
        }
    }
}

