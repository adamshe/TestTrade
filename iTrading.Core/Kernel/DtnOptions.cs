using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds DTN specific connection options.
    /// </summary>
    [Guid("D24C1C10-F377-4a19-B313-160515D75521"), ClassInterface(ClassInterfaceType.None)]
    public class DtnOptions : OptionsBase, IComDtnOptions, ITradingSerializable
    {
        private string host;
        private int newsDaysBack;
        private int newsIntervalSeconds;
        private int newsMaxBack;
        private int portFeed;
        private int portLevel2;
        private int portLookup;

        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.DtnOptions" /> with default settings.
        /// </summary>
        public DtnOptions()
        {
            this.host = "localhost";
            this.newsDaysBack = 1;
            this.newsIntervalSeconds = 60;
            this.newsMaxBack = 10;
            this.portFeed = 0x1391;
            this.portLevel2 = 0x23f0;
            this.portLookup = 0x238c;
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Provider = ProviderType.All[ProviderTypeId.Dtn];
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public DtnOptions(OptionsBase options) : base(options)
        {
            this.host = "localhost";
            this.newsDaysBack = 1;
            this.newsIntervalSeconds = 60;
            this.newsMaxBack = 10;
            this.portFeed = 0x1391;
            this.portLevel2 = 0x23f0;
            this.portLookup = 0x238c;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public DtnOptions(Bytes bytes, int version) : base(bytes, version)
        {
            this.host = "localhost";
            this.newsDaysBack = 1;
            this.newsIntervalSeconds = 60;
            this.newsMaxBack = 10;
            this.portFeed = 0x1391;
            this.portLevel2 = 0x23f0;
            this.portLookup = 0x238c;
            base.Provider = ProviderType.All[ProviderTypeId.Dtn];
            this.host = bytes.ReadString();
            this.newsDaysBack = bytes.ReadInt32();
            this.newsIntervalSeconds = bytes.ReadInt32();
            this.newsMaxBack = bytes.ReadInt32();
            this.portFeed = bytes.ReadInt32();
            this.portLevel2 = bytes.ReadInt32();
            this.portLookup = bytes.ReadInt32();
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.host);
            bytes.Write(this.newsDaysBack);
            bytes.Write(this.newsIntervalSeconds);
            bytes.Write(this.newsMaxBack);
            bytes.Write(this.portFeed);
            bytes.Write(this.portLevel2);
            bytes.Write(this.portLookup);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.DtnOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.DtnOptions;
            }
        }

        /// <summary>
        /// Name of DTN datafeed host to connect to. Do not enter an IP address string, but the hostname itself. 
        /// Default = "localhost".
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
        /// Retrieve news n days back.
        /// Default = 1.
        /// </summary>
        public int NewsDaysBack
        {
            get
            {
                return this.newsDaysBack;
            }
            set
            {
                this.newsDaysBack = value;
            }
        }

        /// <summary>
        /// Check for new every n seconds.
        /// Default = 60.
        /// </summary>
        public int NewsIntervalSeconds
        {
            get
            {
                return this.newsIntervalSeconds;
            }
            set
            {
                this.newsIntervalSeconds = value;
            }
        }

        /// <summary>
        /// Retrieve max # of news back.
        /// Default= 10.
        /// </summary>
        public int NewsMaxBack
        {
            get
            {
                return this.newsMaxBack;
            }
            set
            {
                this.newsMaxBack = value;
            }
        }

        /// <summary>
        /// The port for streaming data. Default: 5009
        /// </summary>
        public int PortFeed
        {
            get
            {
                return this.portFeed;
            }
            set
            {
                this.portFeed = value;
            }
        }

        /// <summary>
        /// The port for level 2 data. Default: 9200
        /// </summary>
        public int PortLevel2
        {
            get
            {
                return this.portLevel2;
            }
            set
            {
                this.portLevel2 = value;
            }
        }

        /// <summary>
        /// The port for data lookup. Default: 9100
        /// </summary>
        public int PortLookup
        {
            get
            {
                return this.portLookup;
            }
            set
            {
                this.portLookup = value;
            }
        }
    }
}

