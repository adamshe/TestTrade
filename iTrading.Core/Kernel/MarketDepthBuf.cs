using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Timers;
    using iTrading.Core.Data;

    internal class MarketDepthBuf
    {
        private byte[] buf;
        private int bytesInBuf;
        private int bytesInBufRead;
        private int bytesRead;
        private int bytesWritten;
        private ArrayList headers;
        private const int marketMakerLength = 10;
        private string[] marketMakers;
        private int marketMakersCount;
        private const int maxBytesPerEvent = 12;
        private const int maxMarketMakers = 0x80;
        private const int maxPositions = 0x40;
        private MarketDepthEventArgs readCurrent;
        private bool recording;
        private bool replaying;
        private DateTime replayTime;
        private Symbol symbol;
        private DateTime time;
        private DateTime timeLastWrite;
        private int totalBytes2Read;

        internal MarketDepthBuf(Symbol symbol, DateTime time)
        {
            this.time = time;
            this.symbol = symbol;
            this.Init();
        }

        internal void Cancel()
        {
            if (this.recording)
            {
                this.recording = false;
                this.symbol.MarketDepth.MarketDepthItem -= new MarketDepthItemEventHandler(this.MarketDepth);
                this.Write();
            }
        }

        internal void CancelReplay()
        {
            this.symbol.connection.replayTimer.Elapsed -= new ElapsedEventHandler(this.ReplayTimerElapsed);
        }

        internal static void Dump(Connection connection, Symbol symbol, DateTime from, DateTime to, string path)
        {
            if (from.Date > to.Date)
            {
                throw new TMException(ErrorCode.Panic, "'from' date has to be smaller/equal 'to' date");
            }
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path);
            }
            catch (Exception)
            {
                throw new TMException(ErrorCode.Panic, "Unable to create file '" + path + "'");
            }
            CultureInfo provider = new CultureInfo("en-US");
            for (DateTime time = from.Date; time <= to.Date; time = time.AddDays(1.0))
            {
                MarketDepthBuf buf = new MarketDepthBuf(symbol, time);
                buf.Reset();
                buf.MoveNext();
                while (buf.readCurrent != null)
                {
                    writer.WriteLine(string.Concat(new object[] { "", (int) buf.readCurrent.MarketDataType.Id, ";", buf.readCurrent.Time.ToString("yyyyMMddHHmmssff"), ";", (int) buf.readCurrent.Operation, ";", buf.readCurrent.Position, ";", buf.readCurrent.MarketMaker, ";", buf.readCurrent.Price.ToString(provider), ";", buf.readCurrent.Volume }));
                    buf.MoveNext();
                }
            }
            writer.Close();
        }

        private int GetMarketMakerId(string marketMaker)
        {
            if (marketMaker.Length == 0)
            {
                return 0;
            }
            for (int i = 0; i < this.marketMakersCount; i++)
            {
                if (this.marketMakers[i] == marketMaker)
                {
                    return i;
                }
            }
            if (this.marketMakersCount == 0x80)
            {
                return 0;
            }
            this.marketMakers[this.marketMakersCount] = marketMaker;
            return this.marketMakersCount++;
        }

        private void Init()
        {
            this.buf = new byte[0x8000];
            this.bytesInBuf = -1;
            this.bytesInBufRead = 0;
            this.bytesRead = 0;
            this.bytesWritten = 0;
            this.headers = new ArrayList();
            this.marketMakers = new string[0x80];
            this.marketMakers[0] = "";
            this.marketMakersCount = 1;
            this.readCurrent = null;
            this.recording = false;
            this.replaying = false;
            this.timeLastWrite = DateTime.Now;
            this.totalBytes2Read = 0;
            this.replayTime = this.time;
            if (!Directory.Exists(Globals.InstallDir + @"\db\data"))
            {
                Directory.CreateDirectory(Globals.InstallDir + @"\db\data");
            }
            string path = Globals.InstallDir + @"\db\data\" + this.symbol.FullName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.ReadHeaders();
        }

        internal static void Load(Connection connection, Symbol symbol, string path)
        {
            CultureInfo provider = new CultureInfo("en-US");
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path);
            }
            catch (Exception)
            {
                throw new TMException(ErrorCode.Panic, "Unable to open file '" + path + "'");
            }
            DateTime minDate = Globals.MinDate;
            int num = 0;
            MarketDepthBuf buf = null;
            string str = null;
            while ((str = reader.ReadLine()) != null)
            {
                string[] strArray = str.Split(new char[] { ';' });
                if (strArray.Length != 7)
                {
                    reader.Close();
                    throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "' has wrong number of fields (", strArray.Length, "): ", str }));
                }
                Operation operation = (Operation) Convert.ToInt32(strArray[2]);
                int position = Convert.ToInt32(strArray[3]);
                string marketMaker = strArray[4];
                if (position < 0x40)
                {
                    MarketDataType marketDataType = connection.MarketDataTypes[(MarketDataTypeId) Convert.ToInt32(strArray[0])];
                    if (marketDataType == null)
                    {
                        reader.Close();
                        throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "' holds invalid market data type ", Convert.ToInt32(strArray[0]), ": ", str }));
                    }
                    DateTime time = Globals.MinDate;
                    try
                    {
                        int millisecond = 0;
                        if (strArray[1].Length != 14)
                        {
                            if (strArray[1].Length != 15)
                            {
                                if (strArray[1].Length != 0x10)
                                {
                                    if (strArray[1].Length != 0x11)
                                    {
                                        reader.Close();
                                        throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "' holds invalid time stamp value (milli seconds): ", str }));
                                    }
                                    millisecond = Convert.ToInt32(strArray[1].Substring(14, 3));
                                }
                                else
                                {
                                    millisecond = Convert.ToInt32(strArray[1].Substring(14, 2)) * 10;
                                }
                            }
                            else
                            {
                                millisecond = Convert.ToInt32(strArray[1].Substring(14, 1)) * 100;
                            }
                        }
                        time = new DateTime(Convert.ToInt32(strArray[1].Substring(0, 4)), Convert.ToInt32(strArray[1].Substring(4, 2)), Convert.ToInt32(strArray[1].Substring(6, 2)), Convert.ToInt32(strArray[1].Substring(8, 2)), Convert.ToInt32(strArray[1].Substring(10, 2)), Convert.ToInt32(strArray[1].Substring(12, 2)), millisecond);
                    }
                    catch (Exception)
                    {
                        reader.Close();
                        throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "' holds invalid time stamp '", strArray[1], "': ", str }));
                    }
                    if (time < minDate)
                    {
                        reader.Close();
                        throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "': time stamp '", strArray[2], "' is smaller than previous one: ", str }));
                    }
                    double price = 0.0;
                    try
                    {
                        price = Convert.ToDouble(strArray[5], provider);
                    }
                    catch (Exception)
                    {
                        reader.Close();
                        throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "' holds invalid price value '", strArray[5], "': ", str }));
                    }
                    int volume = 0;
                    try
                    {
                        volume = Convert.ToInt32(strArray[6], provider);
                    }
                    catch (Exception)
                    {
                        connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(connection, ErrorCode.Panic, "", string.Concat(new object[] { "Line ", num, " in file '", path, "' holds invalid volume '", strArray[6], "': ", str })));
                        goto Label_04CF;
                    }
                    if (buf == null)
                    {
                        buf = new MarketDepthBuf(symbol, time);
                    }
                    else if (buf.Date != time.Date)
                    {
                        buf.Write();
                        buf = new MarketDepthBuf(symbol, time);
                    }
                    if (!buf.MarketDepthNow(new MarketDepthEventArgs(symbol.MarketDepth, ErrorCode.NoError, "", symbol, position, marketMaker, operation, marketDataType, price, volume, time)))
                    {
                        reader.Close();
                        return;
                    }
                }
            Label_04CF:
                num++;
            }
            reader.Close();
            if (buf != null)
            {
                buf.Write();
            }
        }

        internal void MarketDepth(object sender, MarketDepthEventArgs e)
        {
            if (!this.MarketDepthNow(e))
            {
                this.Cancel();
            }
        }

        internal bool MarketDepthNow(MarketDepthEventArgs e)
        {
            lock (this)
            {
                byte[] buffer;
                IntPtr ptr;
                if ((e.Error != ErrorCode.NoError) || (e.MarketDataType.Id == MarketDataTypeId.Unknown))
                {
                    return true;
                }
                if ((e.MarketDataType.Id != MarketDataTypeId.Ask) && (e.MarketDataType.Id != MarketDataTypeId.Bid))
                {
                    return true;
                }
                this.MoreBytes(12);
                DataFileHeader header = (DataFileHeader) this.headers[(int) e.MarketDataType.Id];
                int num = ++this.bytesInBuf;
                int num2 = ++this.bytesInBuf;
                int num3 = ++this.bytesInBuf;
                int num4 = (int) Math.Round((double) (e.Time.Subtract(header.timeLast).TotalSeconds / Globals.recorderIntervalSeconds), 0);
                int marketMakerId = this.GetMarketMakerId(e.MarketMaker);
                if (num4 < 0)
                {
                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "MarketData (", e.MarketDataType.Id, ") for '", this.symbol.FullName, "' at ", e.Time, ": time is smaller than last recorded time stamp (", header.timeLast, ")" })));
                    return false;
                }
                if (e.Position >= 0x40)
                {
                    return true;
                }
                (buffer = this.buf)[(int) (ptr = (IntPtr) num2)] = (byte) (buffer[(int) ptr] | ((byte) e.Position));
                (buffer = this.buf)[(int) (ptr = (IntPtr) num2)] = (byte) (buffer[(int) ptr] | ((byte) (((int) e.Operation) << 6)));
                (buffer = this.buf)[(int) (ptr = (IntPtr) num3)] = (byte) (buffer[(int) ptr] | ((byte) marketMakerId));
                (buffer = this.buf)[(int) (ptr = (IntPtr) num3)] = (byte) (buffer[(int) ptr] | ((byte) (((int) e.MarketDataType.Id) << 7)));
                long volume = num4;
                if (volume != 0L)
                {
                    if (volume > 0xffL)
                    {
                        if (volume > 0xffffL)
                        {
                            if (volume > 0xffffffL)
                            {
                                this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "MarketDepth (", e.MarketDataType.Id, ") for '", this.symbol.FullName, "' at ", e.Time, " exceeds time gap maximum" })));
                                return false;
                            }
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 3);
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x10000L);
                            volume -= (this.buf[this.bytesInBuf] * 0x100) * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x100L);
                            volume -= this.buf[this.bytesInBuf] * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) volume;
                        }
                        else
                        {
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 2);
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x100L);
                            volume -= this.buf[this.bytesInBuf] * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) volume;
                        }
                    }
                    else
                    {
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 1);
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                }
                header.timeLast = header.timeLast.AddSeconds(num4 * Globals.recorderIntervalSeconds);
                double num7 = this.symbol.Round2TickSize(e.Price);
                if (header.close != double.MinValue)
                {
                    if (e.Operation != Operation.Delete)
                    {
                        header.high = Math.Max(header.high, num7);
                        header.low = Math.Min(header.low, num7);
                    }
                    volume = (int) Math.Round((double) ((num7 - header.close) / this.symbol.TickSize), 0);
                    if (volume != 0L)
                    {
                        if ((-128L < volume) && (volume < 0x80L))
                        {
                            volume += 0x80L;
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 4);
                            this.buf[++this.bytesInBuf] = (byte) volume;
                        }
                        else if ((-16384L < volume) && (volume < 0x4000L))
                        {
                            volume += 0x4000L;
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 8);
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x100L);
                            volume -= this.buf[this.bytesInBuf] * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) volume;
                        }
                        else
                        {
                            volume += 0x40000000L;
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 12);
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x1000000L);
                            volume -= ((this.buf[this.bytesInBuf] * 0x100) * 0x100) * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x10000L);
                            volume -= (this.buf[this.bytesInBuf] * 0x100) * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x100L);
                            volume -= this.buf[this.bytesInBuf] * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) volume;
                        }
                    }
                }
                else
                {
                    header.high = num7;
                    header.low = num7;
                    header.open = num7;
                }
                header.close = num7;
                if (e.Volume != 0)
                {
                    if (e.Volume <= 0xff)
                    {
                        volume = e.Volume;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 0x10);
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                    else if (((((int) (((double) e.Volume) / 50.0)) * 50) == e.Volume) && ((e.Volume / 50) <= 0xff))
                    {
                        volume = e.Volume / 50;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 0x20);
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                    else if (((((int) (((double) e.Volume) / 100.0)) * 100) == e.Volume) && ((e.Volume / 100) <= 0xff))
                    {
                        volume = e.Volume / 100;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 0x30);
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                    else if (((((int) (((double) e.Volume) / 500.0)) * 500) == e.Volume) && ((e.Volume / 500) <= 0xff))
                    {
                        volume = e.Volume / 500;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 0x40);
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                    else if (((((int) (((double) e.Volume) / 1000.0)) * 0x3e8) == e.Volume) && ((e.Volume / 0x3e8) <= 0xff))
                    {
                        volume = e.Volume / 0x3e8;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 80);
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                    else if (e.Volume <= 0xffff)
                    {
                        volume = e.Volume;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 0x60);
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x100L);
                        volume -= this.buf[this.bytesInBuf] * 0x100;
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                    else
                    {
                        volume = e.Volume;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 0x70);
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x1000000L);
                        volume -= ((this.buf[this.bytesInBuf] * 0x100) * 0x100) * 0x100;
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x10000L);
                        volume -= (this.buf[this.bytesInBuf] * 0x100) * 0x100;
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x100L);
                        volume -= this.buf[this.bytesInBuf] * 0x100;
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                }
                header.volume += e.Volume;
                header.count++;
            }
            return true;
        }

        private void MoreBytes(int moreBytes)
        {
            if ((this.bytesInBuf + moreBytes) >= this.buf.Length)
            {
                this.Write();
            }
        }

        private bool MoveNext()
        {
            if (this.bytesRead >= this.totalBytes2Read)
            {
                this.readCurrent = null;
                return false;
            }
            if (((this.bytesInBufRead + 12) > this.buf.Length) && !this.ReadBuf())
            {
                this.readCurrent = null;
                return false;
            }
            int bytesInBufRead = this.bytesInBufRead;
            byte num2 = this.buf[this.bytesInBufRead];
            byte num3 = this.buf[++this.bytesInBufRead];
            byte num4 = this.buf[++this.bytesInBufRead];
            MarketDataTypeId id = (MarketDataTypeId) (num4 >> 7);
            string marketMaker = this.marketMakers[num4 & 0x7f];
            DataFileHeader header = (DataFileHeader) this.headers[(int) id];
            Operation operation = (Operation) (num3 >> 6);
            int position = num3 & 0x3f;
            double priceCurrent = header.priceCurrent;
            DateTime timeCurrent = header.timeCurrent;
            int volume = 0;
            if ((num2 & 3) != 0)
            {
                if ((num2 & 3) == 1)
                {
                    timeCurrent = timeCurrent.AddSeconds(this.buf[++this.bytesInBufRead] * Globals.recorderIntervalSeconds);
                }
                else if ((num2 & 3) == 2)
                {
                    timeCurrent = timeCurrent.AddSeconds(((this.buf[++this.bytesInBufRead] * 0x100) + this.buf[++this.bytesInBufRead]) * Globals.recorderIntervalSeconds);
                }
                else if ((num2 & 3) == 3)
                {
                    timeCurrent = timeCurrent.AddSeconds(((((this.buf[++this.bytesInBufRead] * 0x100) * 0x100) + (this.buf[++this.bytesInBufRead] * 0x100)) + this.buf[++this.bytesInBufRead]) * Globals.recorderIntervalSeconds);
                }
            }
            header.timeCurrent = timeCurrent;
            if ((num2 & 12) == 4)
            {
                priceCurrent += (this.buf[++this.bytesInBufRead] - 0x80) * header.tickSize;
            }
            else if ((num2 & 12) == 8)
            {
                priceCurrent += (((this.buf[++this.bytesInBufRead] * 0x100) + this.buf[++this.bytesInBufRead]) - 0x4000) * header.tickSize;
            }
            else if ((num2 & 12) == 12)
            {
                priceCurrent += (((((((this.buf[++this.bytesInBufRead] * 0x100L) * ((long) 0x100L)) * ((long) 0x100L)) + ((this.buf[++this.bytesInBufRead] * 0x100) * 0x100)) + (this.buf[++this.bytesInBufRead] * 0x100)) + this.buf[++this.bytesInBufRead]) - ((long) 0x40000000L)) * header.tickSize;
            }
            priceCurrent = this.symbol.Round2TickSize(priceCurrent);
            header.priceCurrent = priceCurrent;
            if ((num2 & 240) == 0)
            {
                volume = 0;
            }
            else if ((num2 & 240) == 0x10)
            {
                volume = this.buf[++this.bytesInBufRead];
            }
            else if ((num2 & 240) == 0x20)
            {
                volume = 50 * this.buf[++this.bytesInBufRead];
            }
            else if ((num2 & 240) == 0x30)
            {
                volume = 100 * this.buf[++this.bytesInBufRead];
            }
            else if ((num2 & 240) == 0x40)
            {
                volume = 500 * this.buf[++this.bytesInBufRead];
            }
            else if ((num2 & 240) == 80)
            {
                volume = 0x3e8 * this.buf[++this.bytesInBufRead];
            }
            else if ((num2 & 240) == 0x60)
            {
                volume = (this.buf[++this.bytesInBufRead] * 0x100) + this.buf[++this.bytesInBufRead];
            }
            else if ((num2 & 240) == 0x70)
            {
                volume = (((((this.buf[++this.bytesInBufRead] * 0x100) * 0x100) * 0x100) + ((this.buf[++this.bytesInBufRead] * 0x100) * 0x100)) + (this.buf[++this.bytesInBufRead] * 0x100)) + this.buf[++this.bytesInBufRead];
            }
            this.bytesInBufRead++;
            this.bytesRead += this.bytesInBufRead - bytesInBufRead;
            MarketDataType marketDataType = this.symbol.connection.MarketDataTypes[id];
            if (marketDataType != null)
            {
                this.readCurrent = new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, position, marketMaker, operation, marketDataType, priceCurrent, volume, timeCurrent);
            }
            return true;
        }

        internal bool ReadBuf()
        {
            FileInfo info = new FileInfo(this.FileName);
            if (!info.Exists || (info.Length == 0L))
            {
                return false;
            }
            lock (this)
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(this.FileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    stream.Seek((long) (((2 * DataFileHeader.SizeOf) + 0xa00) + this.bytesRead), SeekOrigin.Begin);
                    stream.Read(this.buf, 0, Math.Min(this.buf.Length, this.totalBytes2Read - this.bytesRead));
                    this.bytesInBufRead = 0;
                    stream.Close();
                }
                catch (Exception)
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                    return false;
                }
                return true;
            }
        }

        private void ReadHeaders()
        {
            lock (this)
            {
                this.headers.Clear();
                FileInfo info = new FileInfo(this.FileName);
                if (!info.Exists || (info.Length == 0L))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        DataFileHeader header = new DataFileHeader();
                        header.tickSize = this.symbol.TickSize;
                        header.timeCurrent = this.Date;
                        header.timeFirst = this.Date;
                        header.timeLast = this.Date;
                        this.headers.Add(header);
                    }
                    this.marketMakersCount = 1;
                    this.marketMakers[0] = "";
                    this.totalBytes2Read = 0;
                }
                else
                {
                    FileStream stream = null;
                    try
                    {
                        byte[] buffer = new byte[DataFileHeader.SizeOf];
                        stream = new FileStream(this.FileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                        stream.Seek(0L, SeekOrigin.Begin);
                        for (int j = 0; j < 2; j++)
                        {
                            stream.Read(buffer, 0, buffer.Length);
                            Bytes bytes = new Bytes();
                            bytes.Reset(buffer);
                            DataFileHeader header2 = (DataFileHeader) bytes.ReadSerializable();
                            header2.priceCurrent = header2.close;
                            header2.timeCurrent = header2.timeFirst;
                            this.headers.Add(header2);
                        }
                        for (int k = 0; k < 0x80; k++)
                        {
                            byte[] buffer2 = new byte[20];
                            stream.Read(buffer2, 0, buffer2.Length);
                            char[] chArray = new char[10];
                            int num4 = 0;
                            while (num4 < 10)
                            {
                                if ((chArray[num4] = BitConverter.ToChar(buffer2, 2 * num4)) == '\0')
                                {
                                    break;
                                }
                                num4++;
                            }
                            this.marketMakers[k] = new string(chArray, 0, num4);
                        }
                        int index = 1;
                        while (index < 0x80)
                        {
                            if (this.marketMakers[index].Length == 0)
                            {
                                break;
                            }
                            index++;
                        }
                        this.marketMakersCount = index;
                        this.totalBytes2Read = (((int) stream.Length) - (2 * buffer.Length)) - 0xa00;
                        stream.Close();
                    }
                    catch (Exception)
                    {
                        this.headers.Clear();
                        for (int m = 0; m < 2; m++)
                        {
                            DataFileHeader header3 = new DataFileHeader();
                            header3.tickSize = this.symbol.TickSize;
                            header3.timeCurrent = this.Date;
                            header3.timeFirst = this.Date;
                            header3.timeLast = this.Date;
                            this.headers.Add(header3);
                        }
                        this.marketMakersCount = 1;
                        this.marketMakers[0] = "";
                        this.totalBytes2Read = 0;
                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }
                }
            }
        }

        private void ReplayTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!this.symbol.connection.replayTimer.Enabled)
            {
                return;
            }
            this.replayTime = this.replayTime.AddSeconds(Globals.recorderIntervalSeconds * this.symbol.connection.SimulationSpeed);
        Label_003F:
            if (this.readCurrent == null)
            {
                this.CancelReplay();
            }
            else if (this.readCurrent.Time <= this.replayTime)
            {
                this.symbol.connection.ProcessEventArgs(this.readCurrent);
                this.MoveNext();
                goto Label_003F;
            }
        }

        private void Reset()
        {
            this.Init();
            this.ReadBuf();
            for (int i = 0; i < 2; i++)
            {
                DataFileHeader header = (DataFileHeader) this.headers[i];
                header.priceCurrent = header.open;
                header.timeCurrent = header.timeFirst;
            }
        }

        internal void Start()
        {
            if (!this.recording)
            {
                if (this.replaying)
                {
                    throw new TMException(ErrorCode.Panic, "Unable to start recorder in replay mode");
                }
                this.Init();
                this.recording = true;
                for (int i = 0; i < 0x40; i++)
                {
                    this.MarketDepthNow(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, 0, "", Operation.Delete, this.symbol.connection.MarketDataTypes[MarketDataTypeId.Ask], 0.0, 0, ((DataFileHeader) this.headers[0]).timeLast));
                    this.MarketDepthNow(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, 0, "", Operation.Delete, this.symbol.connection.MarketDataTypes[MarketDataTypeId.Bid], 0.0, 0, ((DataFileHeader) this.headers[1]).timeLast));
                }
                this.symbol.MarketDepth.MarketDepthItem += new MarketDepthItemEventHandler(this.MarketDepth);
                lock (this.symbol.connection)
                {
                    if (this.symbol.connection.recorderTimer == null)
                    {
                        this.symbol.connection.recorderTimer = new Timer((double) ((this.symbol.connection.Options.RecorderWriteMinutes * 60) * 0x3e8));
                        this.symbol.connection.recorderTimer.AutoReset = true;
                        this.symbol.connection.recorderTimer.Start();
                        this.symbol.connection.recorderTimer.Elapsed += new ElapsedEventHandler(this.symbol.connection.RecorderTimerElapsed);
                    }
                }
            }
        }

        internal void StartReplay()
        {
            if (this.recording)
            {
                throw new TMException(ErrorCode.Panic, "Unable to enter replay mode while recording");
            }
            while (this.symbol.MarketDepth.Ask.Count > 0)
            {
                this.symbol.Connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, 0, "", Operation.Delete, this.symbol.Connection.MarketDataTypes[MarketDataTypeId.Ask], 0.0, 0, this.symbol.Connection.Now));
            }
            while (this.symbol.MarketDepth.Bid.Count > 0)
            {
                this.symbol.Connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, 0, "", Operation.Delete, this.symbol.Connection.MarketDataTypes[MarketDataTypeId.Bid], 0.0, 0, this.symbol.Connection.Now));
            }
            this.Reset();
            this.MoveNext();
            while ((this.readCurrent != null) && (this.readCurrent.Time < this.replayTime))
            {
                this.readCurrent.initOnly = true;
                this.symbol.connection.ProcessEventArgs(this.readCurrent);
                this.MoveNext();
            }
            for (int i = 0; i < this.symbol.MarketDepth.Ask.Count; i++)
            {
                MarketDepthRow row = this.symbol.MarketDepth.Ask[i];
                this.symbol.connection.SynchronizeInvoke.AsyncInvoke(this.symbol.connection.eventArgsHandler, new object[] { new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, i, row.marketMaker, Operation.Insert, this.symbol.Connection.MarketDataTypes[MarketDataTypeId.Ask], row.price, row.volume, row.time) });
            }
            for (int j = 0; j < this.symbol.MarketDepth.Bid.Count; j++)
            {
                MarketDepthRow row2 = this.symbol.MarketDepth.Bid[j];
                this.symbol.connection.SynchronizeInvoke.AsyncInvoke(this.symbol.connection.eventArgsHandler, new object[] { new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, j, row2.marketMaker, Operation.Insert, this.symbol.Connection.MarketDataTypes[MarketDataTypeId.Bid], row2.price, row2.volume, row2.time) });
            }
            lock (this.symbol.connection)
            {
                if (this.symbol.connection.replayTimer == null)
                {
                    this.symbol.connection.replayTimer = new Timer(Globals.recorderIntervalSeconds * 1000.0);
                    this.symbol.connection.replayTimer.AutoReset = true;
                    this.symbol.connection.replayTimer.Start();
                }
                this.symbol.connection.replayTimer.Elapsed += new ElapsedEventHandler(this.ReplayTimerElapsed);
            }
        }

        internal void Write()
        {
            this.timeLastWrite = DateTime.Now;
            lock (this)
            {
                if (this.bytesInBuf != 0)
                {
                    FileStream stream = null;
                    try
                    {
                        stream = new FileStream(this.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        stream.Seek(0L, SeekOrigin.Begin);
                        for (int i = 0; i < 2; i++)
                        {
                            Bytes bytes = new Bytes();
                            bytes.WriteSerializable((DataFileHeader) this.headers[i]);
                            stream.Write(bytes.Out, 0, bytes.OutLength);
                        }
                        string str = "";
                        for (int j = 0; j < 0x80; j++)
                        {
                            byte[] buffer = new byte[20];
                            char[] chArray = (j < this.marketMakersCount) ? this.marketMakers[j].ToCharArray() : str.ToCharArray();
                            int num3 = Math.Min(chArray.Length, 10);
                            for (int m = 0; m < num3; m++)
                            {
                                byte[] buffer2 = BitConverter.GetBytes(chArray[m]);
                                buffer[2 * m] = buffer2[0];
                                buffer[(2 * m) + 1] = buffer2[1];
                            }
                            stream.Write(buffer, 0, buffer.Length);
                        }
                        stream.Seek(0L, SeekOrigin.End);
                        stream.Write(this.buf, 0, this.bytesInBuf + 1);
                        stream.Close();
                        for (int k = 0; k < this.buf.Length; k++)
                        {
                            this.buf[k] = 0;
                        }
                        this.bytesWritten += this.bytesInBuf;
                        this.bytesInBuf = -1;
                    }
                    catch (Exception)
                    {
                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }
                }
            }
        }

        internal DateTime Date
        {
            get
            {
                return this.time.Date;
            }
        }

        private string FileName
        {
            get
            {
                return (Globals.InstallDir + @"\db\data\" + this.symbol.FullName + @"\" + this.Date.ToString("yyyyMMdd") + ".tm2");
            }
        }

        internal bool Recording
        {
            get
            {
                return this.recording;
            }
        }

        internal DateTime TimeLastWrite
        {
            get
            {
                return this.timeLastWrite;
            }
        }
    }
}

