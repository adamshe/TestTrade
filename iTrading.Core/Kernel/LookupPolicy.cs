namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents a symbol lookup policy.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("2BFAA969-186B-4f6e-A6C6-2D3B2ED1B6F2")]
    public class LookupPolicy : IComLookupPolicy
    {
        private static LookupPolicyDictionary all = null;
        private LookupPolicyId id;

        internal LookupPolicy(LookupPolicyId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available symbol lookup policies.
        /// </summary>
        public static LookupPolicyDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new LookupPolicyDictionary();
                        all.Add(new LookupPolicy(LookupPolicyId.ProviderOnly));
                        all.Add(new LookupPolicy(LookupPolicyId.CacheOnly));
                        all.Add(new LookupPolicy(LookupPolicyId.NoCheck));
                        all.Add(new LookupPolicy(LookupPolicyId.RepositoryAndProvider));
                        all.Add(new LookupPolicy(LookupPolicyId.RepositoryOnly));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the LookupPolicy.
        /// </summary>
        public LookupPolicyId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the LookupPolicy.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case LookupPolicyId.CacheOnly:
                        return "Cache Only";

                    case LookupPolicyId.ProviderOnly:
                        return "Provider Only";

                    case LookupPolicyId.NoCheck:
                        return "No Check";

                    case LookupPolicyId.RepositoryAndProvider:
                        return "Repository And Provider";

                    case LookupPolicyId.RepositoryOnly:
                        return "Repository Only";
                }
                return "Repository Only";
            }
        }
    }
}

