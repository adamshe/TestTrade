namespace iTrading.CT
{
    using System;

    internal class ByteReader
    {
        private Adapter adapter;
        private static byte[] bytes = new byte[0x2710];
        private int idx;
        private unsafe byte* pointer;

        internal unsafe ByteReader(Adapter adapter, byte* pointer)
        {
            this.adapter = adapter;
            this.idx = 0;
            this.pointer = pointer;
        }

        internal unsafe byte ReadByte()
        {
            byte num = this.pointer[this.idx];
            this.idx++;
            return num;
        }

        internal unsafe char ReadChar()
        {
            bytes[0] = this.pointer[this.idx++];
            return this.adapter.asciiEncoding.GetString(bytes, 0, 1)[0];
        }

        internal unsafe double ReadDouble()
        {
            int index = 0;
            while (index < 8)
            {
                bytes[index] = this.pointer[this.idx];
                index++;
                this.idx++;
            }
            return BitConverter.ToDouble(bytes, 0);
        }

        internal unsafe int ReadInteger()
        {
            int index = 0;
            while (index < 4)
            {
                bytes[index] = this.pointer[this.idx];
                index++;
                this.idx++;
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        internal unsafe short ReadShort()
        {
            int index = 0;
            while (index < 2)
            {
                bytes[index] = this.pointer[this.idx];
                index++;
                this.idx++;
            }
            return BitConverter.ToInt16(bytes, 0);
        }

        internal unsafe string ReadString(int length)
        {
            int index = 0;
            index = 0;
            while ((index < length) && (index < bytes.Length))
            {
                if ((this.pointer + this.idx)[index] == 0)
                {
                    break;
                }
                bytes[index] = (this.pointer + this.idx)[index];
                index++;
            }
            string str = this.adapter.asciiEncoding.GetString(bytes, 0, Math.Min(index, length));
            this.idx += length;
            return str;
        }

        internal DateTime ReadTime()
        {
            int second = this.ReadInteger();
            int minute = this.ReadInteger();
            int hour = this.ReadInteger();
            int day = this.ReadInteger();
            int num5 = this.ReadInteger();
            int num6 = this.ReadInteger();
            this.ReadInteger();
            this.ReadInteger();
            this.ReadInteger();
            return new DateTime(num6 + 0x76c, num5 + 1, day, hour, minute, second);
        }

        internal void Skip(int count)
        {
            this.idx += count;
        }
    }
}

