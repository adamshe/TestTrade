using iTrading.Core.Data;

namespace iTrading.Core.Data
{
    using System;
    using iTrading.Core.Kernel;

    internal class DataFileHeader : ITradingSerializable
    {
        internal double close;
        internal int count;
        internal double high;
        internal double low;
        internal double open;
        internal double priceCurrent;
        internal double splitFactor;
        internal double tickSize;
        internal DateTime timeCurrent;
        internal DateTime timeFirst;
        internal DateTime timeLast;
        internal int volume;

        internal DataFileHeader()
        {
            this.close = double.MinValue;
            this.count = 0;
            this.high = double.MinValue;
            this.low = double.MaxValue;
            this.open = 0.0;
            this.splitFactor = 1.0;
            this.tickSize = 0.0;
            this.timeFirst = Globals.MinDate;
            this.timeLast = Globals.MinDate;
            this.volume = 0;
            this.priceCurrent = 0.0;
            this.timeCurrent = Globals.MinDate;
        }

        internal DataFileHeader(Bytes bytes, int version)
        {
            this.close = double.MinValue;
            this.count = 0;
            this.high = double.MinValue;
            this.low = double.MaxValue;
            this.open = 0.0;
            this.splitFactor = 1.0;
            this.tickSize = 0.0;
            this.timeFirst = Globals.MinDate;
            this.timeLast = Globals.MinDate;
            this.volume = 0;
            this.priceCurrent = 0.0;
            this.timeCurrent = Globals.MinDate;
            this.close = bytes.ReadDouble();
            this.count = bytes.ReadInt32();
            this.high = bytes.ReadDouble();
            this.low = bytes.ReadDouble();
            this.open = bytes.ReadDouble();
            this.splitFactor = bytes.ReadDouble();
            this.tickSize = bytes.ReadDouble();
            this.timeFirst = bytes.ReadDateTime();
            this.timeLast = bytes.ReadDateTime();
            this.volume = bytes.ReadInt32();
        }

        public void Serialize(Bytes bytes, int version)
        {
            bytes.Write(this.close);
            bytes.Write(this.count);
            bytes.Write(this.high);
            bytes.Write(this.low);
            bytes.Write(this.open);
            bytes.Write(this.splitFactor);
            bytes.Write(this.tickSize);
            bytes.Write(this.timeFirst);
            bytes.Write(this.timeLast);
            bytes.Write(this.volume);
        }

        public iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.DataFileHeader;
            }
        }

        internal static int SizeOf
        {
            get
            {
                return ((((((((((Bytes.HeaderSize + 8) + 8) + 8) + 8) + 8) + 8) + 8) + 4) + 8) + 4);
            }
        }

        public int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

