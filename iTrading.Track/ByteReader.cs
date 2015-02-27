namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Text;
    using iTrading.Core.Kernel;

    internal class ByteReader
    {
        private Adapter adapter = null;
        private ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        private byte[] bytes = new byte[0x960];
        private int idx = 0;

        internal ByteReader(Adapter adapter)
        {
            this.adapter = adapter;
        }

        internal iTrading.Track.ErrorCode GetMessage()
        {
            this.idx = 0;
            return (iTrading.Track.ErrorCode) Api.GetMessage(this.bytes, this.adapter.Options.MessageWaitMilliSeconds);
        }

        internal StringCollection ReadBrokerTableFields(int countFields)
        {
            int num = 0;
            int idx = this.idx;
            StringCollection strings = new StringCollection();
            int num3 = this.idx;
            int num4 = num3;
            while (num4 < (num3 + 400))
            {
                if (this.idx >= this.bytes.Length)
                {
                    Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
                }
                if (num >= countFields)
                {
                    return strings;
                }
                if (this.bytes[this.idx] == 0)
                {
                    strings.Add(this.asciiEncoding.GetString(this.bytes, idx, this.idx - idx).Trim());
                    idx = this.idx + 1;
                    num++;
                }
                num4++;
                this.idx++;
            }
            return strings;
        }

        internal byte ReadByte()
        {
            byte num = this.bytes[this.idx];
            this.idx++;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
            return num;
        }

        internal DateTime ReadCharDate()
        {
            if (((this.bytes[this.idx] == 0) && (this.bytes[this.idx + 1] == 0)) && (this.bytes[this.idx + 2] == 0))
            {
                this.idx += 3;
                return Globals.MaxDate;
            }
            int num = 0x7d0;
            if (this.bytes[this.idx + 2] > 10)
            {
                num = 0x76c;
            }
            DateTime time = new DateTime(num + this.bytes[this.idx + 2], this.bytes[this.idx], this.bytes[this.idx + 1]);
            this.idx += 3;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
            return time;
        }

        internal DateTime ReadCharTime()
        {
            DateTime now = this.adapter.connection.Now;
            DateTime time2 = new DateTime(now.Year, now.Month, now.Day, this.bytes[this.idx], this.bytes[this.idx + 1], this.bytes[this.idx + 2]);
            this.idx += 3;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
            return time2;
        }

        internal int ReadInteger()
        {
            int num = BitConverter.ToInt32(this.bytes, this.idx);
            this.idx += 4;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
            return num;
        }

        internal short ReadShort()
        {
            short num = BitConverter.ToInt16(this.bytes, this.idx);
            this.idx += 2;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
            return num;
        }

        internal DateTime ReadShortDate()
        {
            int year = this.ReadShort();
            int month = this.ReadShort();
            int day = this.ReadShort();
            DateTime maxDate = Globals.MaxDate;
            try
            {
                maxDate = new DateTime(year, month, day);
            }
            catch
            {
                Trace.WriteLine(string.Concat(new object[] { "WARNING: Track.ByteReader.ReadShortDate: Invalid data  '", year, "/", month, "/", day, "'" }));
            }
            return maxDate;
        }

        internal string ReadString(int length)
        {
            int num = 0;
            num = 0;
            while (num <= length)
            {
                if ((this.bytes[this.idx + num] == 0) || char.IsControl((char) this.bytes[this.idx + num]))
                {
                    break;
                }
                num++;
            }
            string str = this.asciiEncoding.GetString(this.bytes, this.idx, Math.Min(num, length));
            this.idx += length;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
            return str;
        }

        internal int ReadUShort()
        {
            int num = Convert.ToInt32(BitConverter.ToUInt16(this.bytes, this.idx).ToString());
            this.idx += 2;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
            return num;
        }

        internal void Skip(int count)
        {
            this.idx += count;
            if (this.idx >= this.bytes.Length)
            {
                Trace.Assert(false, "Track.ByteReader: byte limited exceeded");
            }
        }
    }
}

