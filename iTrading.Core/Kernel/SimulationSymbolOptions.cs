using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml;
    using iTrading.Core.Interface;

    /// <summary>
    /// Options to configure the order execution simulator.
    /// </summary>
    [Guid("DC87D868-99F1-411b-822C-23A60B848ED5"), ClassInterface(ClassInterfaceType.None)]
    public class SimulationSymbolOptions : IComSimulationSymbolOptions, ITradingSerializable
    {
        private double commissionMin;
        private double commissionPerUnit;
        private double margin;
        private double slippage;

        /// <summary>
        /// </summary>
        public SimulationSymbolOptions()
        {
            this.commissionMin = 0.0;
            this.commissionPerUnit = 0.0;
            this.margin = 0.0;
            this.slippage = 0.0;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public SimulationSymbolOptions(Bytes bytes, int version)
        {
            this.commissionMin = 0.0;
            this.commissionPerUnit = 0.0;
            this.margin = 0.0;
            this.slippage = 0.0;
            this.margin = bytes.ReadDouble();
            this.commissionMin = bytes.ReadDouble();
            this.commissionPerUnit = bytes.ReadDouble();
            this.slippage = bytes.ReadDouble();
        }

        internal double GetAvgPrice(double rawPrice, Order order, MarketPosition marketPosition)
        {
            return (rawPrice + (((((marketPosition.Id == MarketPositionId.Long) ? ((double) 1) : ((double) (-1))) * (((order.OrderType.Id == OrderTypeId.Market) || (order.OrderType.Id == OrderTypeId.Stop)) ? this.slippage : 0.0)) * order.Symbol.TickSize) * order.Symbol.PointValue));
        }

        /// <summary>
        /// Returns a default <see cref="T:iTrading.Core.Kernel.SimulationSymbolOptions" />.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static SimulationSymbolOptions GetDefaultSimulationSymbolOptions(Symbol symbol)
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
            SimulationSymbolOptions options = new SimulationSymbolOptions();
            document.Load(reader);
            reader.Close();
            try
            {
                switch (symbol.SymbolType.Id)
                {
                    case SymbolTypeId.Future:
                        options.CommissionMin = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["CommissionMin"].InnerText, CultureInfo.InvariantCulture);
                        options.CommissionPerUnit = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Commission"].InnerText, CultureInfo.InvariantCulture);
                        options.Margin = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Margin"].InnerText, CultureInfo.InvariantCulture);
                        options.Slippage = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Slippage"].InnerText, CultureInfo.InvariantCulture);
                        goto Label_0361;

                    case SymbolTypeId.Stock:
                        options.CommissionMin = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["CommissionMin"].InnerText, CultureInfo.InvariantCulture);
                        options.CommissionPerUnit = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Commission"].InnerText, CultureInfo.InvariantCulture);
                        options.Slippage = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Slippage"].InnerText, CultureInfo.InvariantCulture);
                        goto Label_0361;

                    case SymbolTypeId.Index:
                        goto Label_0361;

                    case SymbolTypeId.Option:
                        options.CommissionMin = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Options"]["CommissionMin"].InnerText, CultureInfo.InvariantCulture);
                        options.CommissionPerUnit = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Options"]["Commission"].InnerText, CultureInfo.InvariantCulture);
                        options.Slippage = Convert.ToDouble(document["TradeMagic"]["Options"]["Simulator"]["Options"]["Slippage"].InnerText, CultureInfo.InvariantCulture);
                        goto Label_0361;
                }
                throw new TMException(ErrorCode.Panic, "Cbi.SimulationSymbolOptions.GetDefaultSimulationSymbolOptions: symbol type '" + symbol.SymbolType.Id + "' not supported.");
            }
            catch
            {
            }
        Label_0361:
            if (symbol.Commission != 0.0)
            {
                options.CommissionPerUnit = symbol.Commission;
            }
            if (symbol.Margin != 0.0)
            {
                options.Margin = symbol.Margin;
            }
            if (symbol.Slippage != 0.0)
            {
                options.Slippage = symbol.Slippage;
            }
            return options;
        }

        internal static SimulationSymbolOptions Restore(string customText)
        {
            SimulationSymbolOptions options;
            new SimulationSymbolOptions();
            try
            {
                XmlDocument document = new XmlDocument();
                XmlTextReader reader = new XmlTextReader(new StringReader(customText));
                document.Load(reader);
                reader.Close();
                Bytes bytes = new Bytes();
                bytes.Reset(Convert.FromBase64String(document["TradeMagic"]["_" + ModeTypeId.Simulation.ToString()].InnerText));
                options = (SimulationSymbolOptions) bytes.ReadSerializable();
            }
            catch (Exception)
            {
                throw new TMException(ErrorCode.Panic, "CustomText property is not well formatted xml text");
            }
            return options;
        }

        internal string Save(string customText)
        {
            string str;
            try
            {
                XmlDocument document = new XmlDocument();
                if (customText.Length > 0)
                {
                    XmlTextReader reader = new XmlTextReader(new StringReader(customText));
                    document.Load(reader);
                    reader.Close();
                }
                if (document["TradeMagic"] == null)
                {
                    document.AppendChild(document.CreateElement("TradeMagic"));
                }
                if (document["TradeMagic"]["_" + ModeTypeId.Simulation.ToString()] == null)
                {
                    document["TradeMagic"].AppendChild(document.CreateElement("_" + ModeTypeId.Simulation.ToString()));
                }
                Bytes bytes = new Bytes();
                bytes.WriteSerializable(this);
                document["TradeMagic"]["_" + ModeTypeId.Simulation.ToString()].InnerText = Convert.ToBase64String(bytes.Out, 0, bytes.OutLength);
                StringWriter writer = new StringWriter();
                document.Save(writer);
                str = writer.ToString();
            }
            catch (Exception)
            {
                throw new TMException(ErrorCode.Panic, "CustomText property is not well formatted xml text");
            }
            return str;
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public virtual void Serialize(Bytes bytes, int version)
        {
            bytes.Write(this.Margin);
            bytes.Write(this.CommissionMin);
            bytes.Write(this.CommissionPerUnit);
            bytes.Write(this.Slippage);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.ClassId" /> of current object.
        /// </summary>
        public iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.SimulationSymbolOptions;
            }
        }

        /// <summary>
        /// Minimum fixed commission per side.
        /// Total commission = <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionMin" /> + <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionPerUnit" /> * units.
        /// </summary>
        public double CommissionMin
        {
            get
            {
                return this.commissionMin;
            }
            set
            {
                this.commissionMin = value;
            }
        }

        /// <summary>
        /// Commission per unit and side.
        /// Total commission = <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionMin" /> + <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionPerUnit" /> * units.
        /// </summary>
        public double CommissionPerUnit
        {
            get
            {
                return this.commissionPerUnit;
            }
            set
            {
                this.commissionPerUnit = value;
            }
        }

        /// <summary>
        /// Margin requirement per unit of the traded symbol.
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
        /// Slippage in ticks per side.
        /// </summary>
        public double Slippage
        {
            get
            {
                return this.slippage;
            }
            set
            {
                this.slippage = value;
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
    }
}

