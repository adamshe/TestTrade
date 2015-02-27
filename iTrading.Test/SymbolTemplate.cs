namespace iTrading.Test
{
    using System;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Template from symbol creation.
    /// </summary>
    public class SymbolTemplate
    {
        private iTrading.Core.Kernel.ExchangeId exchangeId;
        private DateTime expiry;
        private string name;
        private iTrading.Core.Kernel.SymbolTypeId symbolTypeId;
        private bool valid;

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="symbolTypeId"></param>
        /// <param name="expiry"></param>
        /// <param name="exchangeId"></param>
        /// <param name="valid"></param>
        public SymbolTemplate(string name, iTrading.Core.Kernel.SymbolTypeId symbolTypeId, DateTime expiry, iTrading.Core.Kernel.ExchangeId exchangeId, bool valid)
        {
            this.exchangeId = exchangeId;
            this.expiry = expiry;
            this.name = name;
            this.symbolTypeId = symbolTypeId;
            this.valid = valid;
        }

        /// <summary></summary>
        public iTrading.Core.Kernel.ExchangeId ExchangeId
        {
            get
            {
                return this.exchangeId;
            }
        }

        /// <summary></summary>
        public DateTime Expiry
        {
            get
            {
                return this.expiry;
            }
        }

        /// <summary></summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary></summary>
        public iTrading.Core.Kernel.SymbolTypeId SymbolTypeId
        {
            get
            {
                return this.symbolTypeId;
            }
        }

        /// <summary></summary>
        public bool Valid
        {
            get
            {
                return this.valid;
            }
        }
    }
}

