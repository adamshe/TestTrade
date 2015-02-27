using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds eSignal specific connection options.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("2C0FFC3F-9456-468b-8925-0CCDF90E091A")]
    public class ESignalOptions : OptionsBase, IComESignalOptions, ITradingSerializable
    {
        private int checkStatusSeconds;
        private int connectTimeoutSeconds;

        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.ESignalOptions" /> with default settings.
        /// </summary>
        public ESignalOptions()
        {
            this.checkStatusSeconds = 5;
            this.connectTimeoutSeconds = 10;
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Provider = ProviderType.All[ProviderTypeId.ESignal];
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public ESignalOptions(OptionsBase options) : base(options)
        {
            this.checkStatusSeconds = 5;
            this.connectTimeoutSeconds = 10;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public ESignalOptions(Bytes bytes, int version) : base(bytes, version)
        {
            this.checkStatusSeconds = 5;
            this.connectTimeoutSeconds = 10;
            base.Provider = ProviderType.All[ProviderTypeId.ESignal];
            this.checkStatusSeconds = bytes.ReadInt32();
            this.connectTimeoutSeconds = bytes.ReadInt32();
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.checkStatusSeconds);
            bytes.Write(this.connectTimeoutSeconds);
        }

        /// <summary>
        /// Check connection status every n seconds. Default = 5.
        /// </summary>
        public int CheckStatusSeconds
        {
            get
            {
                return this.checkStatusSeconds;
            }
            set
            {
                this.checkStatusSeconds = value;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.ESignalOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.ESignalOptions;
            }
        }

        /// <summary>
        /// Timeout to wait until eSignal application is up and confirms entitlements for API access.
        /// Default = 10 seconds.
        /// </summary>
        public int ConnectTimeoutSeconds
        {
            get
            {
                return this.connectTimeoutSeconds;
            }
            set
            {
                this.connectTimeoutSeconds = value;
            }
        }
    }
}

