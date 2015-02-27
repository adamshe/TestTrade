using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds TrackData specific connection options.
    /// </summary>
    [Guid("0ACF8C61-3EED-4e2c-89D7-C676C92A7F0F"), ClassInterface(ClassInterfaceType.None)]
    public class TrackOptions : OptionsBase, IComTrackOptions, ITradingSerializable
    {
        private int checkStatusMilliSeconds;
        private string host;
        private int messageWaitMilliSeconds;
        private int port;
        private int waitMilliSecondsRequest;

        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.TrackOptions" /> with default settings to connect
        /// to the local demo system.
        /// </summary>
        public TrackOptions()
        {
            this.checkStatusMilliSeconds = 0x3e8;
            this.host = "sdk.trackdata.com";
            this.port = 0x26ff;
            this.messageWaitMilliSeconds = 0x3e8;
            this.waitMilliSecondsRequest = 0x44c;
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Provider = ProviderType.All[ProviderTypeId.TrackData];
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public TrackOptions(OptionsBase options) : base(options)
        {
            this.checkStatusMilliSeconds = 0x3e8;
            this.host = "sdk.trackdata.com";
            this.port = 0x26ff;
            this.messageWaitMilliSeconds = 0x3e8;
            this.waitMilliSecondsRequest = 0x44c;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public TrackOptions(Bytes bytes, int version) : base(bytes, version)
        {
            this.checkStatusMilliSeconds = 0x3e8;
            this.host = "sdk.trackdata.com";
            this.port = 0x26ff;
            this.messageWaitMilliSeconds = 0x3e8;
            this.waitMilliSecondsRequest = 0x44c;
            base.Provider = ProviderType.All[ProviderTypeId.TrackData];
            this.checkStatusMilliSeconds = bytes.ReadInt32();
            this.host = bytes.ReadString();
            this.messageWaitMilliSeconds = bytes.ReadInt32();
            this.port = bytes.ReadInt32();
            this.waitMilliSecondsRequest = bytes.ReadInt32();
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.checkStatusMilliSeconds);
            bytes.Write(this.host);
            bytes.Write(this.messageWaitMilliSeconds);
            bytes.Write(this.port);
            bytes.Write(this.waitMilliSecondsRequest);
        }

        /// <summary>
        /// Check connection status every n milliseconds. Default = 1000.
        /// </summary>
        public int CheckStatusMilliSeconds
        {
            get
            {
                return this.checkStatusMilliSeconds;
            }
            set
            {
                this.checkStatusMilliSeconds = value;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.TrackOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.TrackOptions;
            }
        }

        /// <summary>
        /// Name of host to connect to. Default = "sdk.trackdata.com".
        /// </summary>
        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                this.host = value;
            }
        }

        /// <summary>
        /// Milliseconds to wait on reading messages. Default = 1000.
        /// </summary>
        public int MessageWaitMilliSeconds
        {
            get
            {
                return this.messageWaitMilliSeconds;
            }
            set
            {
                this.messageWaitMilliSeconds = value;
            }
        }

        /// <summary>
        /// IP port to connect to. Default = 9983;
        /// </summary>
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }

        /// <summary>
        /// Delay for subsequent requests ofr historical data.
        /// Default: 1100 (milli seconds)
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
    }
}

