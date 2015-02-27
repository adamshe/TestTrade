namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Identifies the symbol lookup policy. <seealso cref="M:iTrading.Core.Kernel.Connection.GetSymbol(System.String,System.DateTime,iTrading.Core.Kernel.SymbolType,iTrading.Core.Kernel.Exchange,System.Double,iTrading.Core.Kernel.RightId,iTrading.Core.Kernel.LookupPolicyId)" />
    /// </summary>
    public enum LookupPolicyId
    {
        CacheOnly,
        ProviderOnly,
        NoCheck,
        RepositoryAndProvider,
        RepositoryOnly
    }
}

