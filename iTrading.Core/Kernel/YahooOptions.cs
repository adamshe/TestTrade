using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds YAHOO specific connection options.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("A96AF0C5-35C2-4ee9-B8D2-2548FAAE317E")]
    public class YahooOptions : OptionsBase, IComYahooOptions, ITradingSerializable
    {
        private int retryTimesOnWebError;
        private int updateMarketDataSeconds;
        private int waitMilliSecondsRequest;
        private int webTimeoutSeconds;

        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.YahooOptions" /> with default settings.
        /// </summary>
        public YahooOptions()
        {
            this.retryTimesOnWebError = 10;
            this.updateMarketDataSeconds = 60;
            this.waitMilliSecondsRequest = 0x3e8;
            this.webTimeoutSeconds = 5;
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Provider = ProviderType.All[ProviderTypeId.Yahoo];
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public YahooOptions(OptionsBase options) : base(options)
        {
            this.retryTimesOnWebError = 10;
            this.updateMarketDataSeconds = 60;
            this.waitMilliSecondsRequest = 0x3e8;
            this.webTimeoutSeconds = 5;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public YahooOptions(Bytes bytes, int version) : base(bytes, version)
        {
            this.retryTimesOnWebError = 10;
            this.updateMarketDataSeconds = 60;
            this.waitMilliSecondsRequest = 0x3e8;
            this.webTimeoutSeconds = 5;
            base.Provider = ProviderType.All[ProviderTypeId.Yahoo];
            this.retryTimesOnWebError = bytes.ReadInt32();
            this.updateMarketDataSeconds = bytes.ReadInt32();
            this.webTimeoutSeconds = bytes.ReadInt32();
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.retryTimesOnWebError);
            bytes.Write(this.updateMarketDataSeconds);
            bytes.Write(this.webTimeoutSeconds);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.YahooOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.YahooOptions;
            }
        }

        /// <summary>
        /// The YAHOO server occationally returns web errors. Retry as many times as set by this value.
        /// Default = 10;
        /// </summary>
        public int RetryTimesOnWebError
        {
            get
            {
                return this.retryTimesOnWebError;
            }
            set
            {
                this.retryTimesOnWebError = value;
            }
        }

        /// <summary>
        /// Request current market data every n seconds.
        /// Requesting the data form the YAHOO servers lasts some seconds, don't assign a too small value.
        /// Default= 60.
        /// </summary>
        public int UpdateMarketDataSeconds
        {
            get
            {
                return this.updateMarketDataSeconds;
            }
            set
            {
                this.updateMarketDataSeconds = value;
            }
        }

        /// <summary>
        /// Min. time in milliseconds between 2 subsequent requests.
        /// YAHOO does not accept rapid subsequent requests.
        /// Default = 1000.
        /// </summary>
        public int WaitMilliSecondsRequest
        {
            get
            {
                return this.waitMilliSecondsRequest;
            }
            set
            {
                this.waitMilliSecondsRequest = value;
            }
        }

        /// <summary>
        /// Timeout for web requests.
        /// Default = 5
        /// </summary>
        public int WebTimeoutSeconds
        {
            get
            {
                return this.webTimeoutSeconds;
            }
            set
            {
                this.webTimeoutSeconds = value;
            }
        }
    }
}

