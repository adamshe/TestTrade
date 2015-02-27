using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Xml;
    using iTrading.Core.Interface;

    /// <summary>
    /// Options to configure the order simulation account.
    /// </summary>
    [Guid("1F9CC835-0D9D-4115-AC70-BA7B61E13786"), ClassInterface(ClassInterfaceType.None)]
    public class SimulationAccountOptions : IComSimulationAccountOptions, ITradingSerializable
    {
        private int delayCommunication;
        private int delayExchange;
        private double initialCashValue;
        private double maintenanceMargin;
        private double margin;
        private int waitForMarketDataSeconds;

        /// <summary>
        /// Initialize with settings stored in the Config.xml file.
        /// </summary>
        public SimulationAccountOptions()
        {
            this.delayCommunication = 150;
            this.delayExchange = 500;
            this.initialCashValue = 100000.0;
            this.margin = 0.5;
            this.maintenanceMargin = 0.3;
            this.waitForMarketDataSeconds = 0;
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            try
            {
                this.delayCommunication = Convert.ToInt32(document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayCommunication"].InnerText, CultureInfo.InvariantCulture);
                this.delayExchange = Convert.ToInt32(document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayExchange"].InnerText, CultureInfo.InvariantCulture);
                this.initialCashValue = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["General"]["InitialCash"].InnerText, CultureInfo.InvariantCulture);
                this.margin = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["General"]["Margin"].InnerText, CultureInfo.InvariantCulture);
                this.maintenanceMargin = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["General"]["MaintenanceMargin"].InnerText, CultureInfo.InvariantCulture);
                this.waitForMarketDataSeconds = Convert.ToInt32(document["TradeMagic"]["Options"]["Simulator"]["General"]["WaitForMarketDataSeconds"].InnerText, CultureInfo.InvariantCulture);
            }
            catch
            {
            }
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public SimulationAccountOptions(Bytes bytes, int version)
        {
            this.delayCommunication = 150;
            this.delayExchange = 500;
            this.initialCashValue = 100000.0;
            this.margin = 0.5;
            this.maintenanceMargin = 0.3;
            this.waitForMarketDataSeconds = 0;
            this.delayCommunication = bytes.ReadInt32();
            this.delayExchange = bytes.ReadInt32();
            this.initialCashValue = bytes.ReadDouble();
            this.margin = bytes.ReadDouble();
            this.maintenanceMargin = bytes.ReadDouble();
            this.waitForMarketDataSeconds = bytes.ReadInt32();
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public virtual void Serialize(Bytes bytes, int version)
        {
            bytes.Write(this.DelayCommunication);
            bytes.Write(this.DelayExchange);
            bytes.Write(this.InitialCashValue);
            bytes.Write(this.Margin);
            bytes.Write(this.MaintenanceMargin);
            bytes.Write(this.WaitForMarketDataSeconds);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.SimulationAccountOptions.ClassId" /> of current object.
        /// </summary>
        public iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.SimulationAccountOptions;
            }
        }

        /// <summary>
        /// One way delay on internet communication to the exchange in milliseconds. Must be greater than 0.
        /// Default = 150.
        /// </summary>
        public int DelayCommunication
        {
            get
            {
                return this.delayCommunication;
            }
            set
            {
                if (value == 0)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.SimulationException.DelayCommunication: Value can't be set to 0");
                }
                this.delayCommunication = value;
            }
        }

        /// <summary>
        /// Delay on exchange processing an order -, order change - or order cancel request. Must be greater than 0.
        /// Default = 500.
        /// </summary>
        public int DelayExchange
        {
            get
            {
                return this.delayExchange;
            }
            set
            {
                if (value == 0)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.SimulationException.DelayExchange: Value can't be set to 0");
                }
                this.delayExchange = value;
            }
        }

        /// <summary>
        /// Initial cash value of account. Must be greater than 0.
        /// Default = 100.000.
        /// </summary>
        public double InitialCashValue
        {
            get
            {
                return this.initialCashValue;
            }
            set
            {
                if (value == 0.0)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.SimulationException.InitialCashValue: Value can't be set to 0");
                }
                this.initialCashValue = value;
            }
        }

        /// <summary>
        /// Maintenance margin on this account.
        /// Default = 0.3.
        /// </summary>
        public double MaintenanceMargin
        {
            get
            {
                return this.maintenanceMargin;
            }
            set
            {
                this.maintenanceMargin = value;
            }
        }

        /// <summary>
        /// Margin on this account.
        /// Default = 0.5.
        /// </summary>
        public double Margin
        {
            get
            {
                return this.margin;
            }
            set
            {
                this.margin = value;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public int Version
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// A running market data stream is mandatory for the simulator. Although the simulator turns on any non-running market data stream on order placement for the required
        /// symbol, data might not be available right away. On setting this option, the simulator can be advised to wait until finaly the first market data item was seen. 
        /// Default = 0. Note: Setting this option will delay order submission until the timer expired or the first market data was seen.
        /// </summary>
        public int WaitForMarketDataSeconds
        {
            get
            {
                return this.waitForMarketDataSeconds;
            }
            set
            {
                this.waitForMarketDataSeconds = value;
            }
        }
    }
}

