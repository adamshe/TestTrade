namespace TradeMagic.Pats
{
    using System;

    internal class Symbol
    {
        public PatsApi.Contract contract;
        public int index;

        internal Symbol(int idx, PatsApi.Contract contract)
        {
            this.contract = contract;
            this.index = idx;
        }

        internal bool IsEqual(PatsApi.ContractUpdate priceUpdate)
        {
            if (!PatsApi.IsEqual(this.contract.contractName, priceUpdate.contractName))
            {
                return false;
            }
            if (!PatsApi.IsEqual(this.contract.exchangeName, priceUpdate.exchangeName))
            {
                return false;
            }
            if (!PatsApi.IsEqual(this.contract.contractDate, priceUpdate.contractDate))
            {
                return false;
            }
            return true;
        }
    }
}

