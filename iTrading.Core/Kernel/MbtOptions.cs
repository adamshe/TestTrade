using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Holds MB Trading specific connection options.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("7C8EDD22-7619-4a55-B04A-6E0F42A97390")]
    public class MbtOptions : OptionsBase, ITradingSerializable
    {
        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.MbtOptions" /> with default settings to connect
        /// to the local demo system.
        /// </summary>
        public MbtOptions()
        {
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Provider = ProviderType.All[ProviderTypeId.MBTrading];
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public MbtOptions(OptionsBase options) : base(options)
        {
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public MbtOptions(Bytes bytes, int version) : base(bytes, version)
        {
            base.Provider = ProviderType.All[ProviderTypeId.MBTrading];
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.MbtOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.MbtOptions;
            }
        }
    }
}

