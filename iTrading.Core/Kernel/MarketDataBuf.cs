using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Timers;
    using iTrading.Core.Data;

    internal class MarketDataBuf
    {
        private byte[] buf = new byte[0x8000];
        private int bytesInBuf;
        private int bytesInBufRead;
        private int bytesRead;
        private int bytesWritten;
        private ArrayList headers;
        private const int maxBytesPerEvent = 11;
        private MarketDataEventArgs readCurrent;
        private bool recording;
        private bool replaying;
        private DateTime replayTime;
        private Symbol symbol;
        private DateTime time;
        private DateTime timeLastWrite;
        private int totalBytes2Read;

        internal MarketDataBuf(Symbol symbol, DateTime time)
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
                this.symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(this.MarketData);
                this.Write();
            }
        }

        internal void CancelReplay()
        {
            this.symbol.connection.replayTimer.Elapsed -= new ElapsedEventHandler(this.ReplayTimerElapsed);
        }

        internal static bool Dump(Connection connection, Symbol symbol, DateTime from, DateTime to, string path)
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
                MarketDataBuf buf = new MarketDataBuf(symbol, time);
                buf.Reset();
                buf.MoveNext();
                while (buf.readCurrent != null)
                {
                    writer.WriteLine(string.Concat(new object[] { "", (int) buf.readCurrent.MarketDataType.Id, ";", buf.readCurrent.Time.ToString("yyyyMMddHHmmssff"), ";", buf.readCurrent.Price.ToString(provider), ";", buf.readCurrent.Volume }));
                    buf.MoveNext();
                }
            }
            writer.Close();
            return true;
        }

        private void Init()
        {
            this.buf = new byte[0x8000];
            this.bytesInBuf = -1;
            this.bytesInBufRead = 0;
            this.bytesRead = 0;
            this.bytesWritten = 0;
            this.headers = new ArrayList();
            this.timeLastWrite = DateTime.Now;
            this.totalBytes2Read = 0;
            this.readCurrent = null;
            this.recording = false;
            this.replaying = false;
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
            MarketDataBuf buf = null;
            string str = null;
            while ((str = reader.ReadLine()) != null)
            {
                string[] strArray = str.Split(new char[] { ';' });
                if (strArray.Length != 4)
                {
                    reader.Close();
                    throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "' has wrong number of fields (", strArray.Length, "): ", str }));
                }
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
                    price = Convert.ToDouble(strArray[2], provider);
                }
                catch (Exception)
                {
                    reader.Close();
                    throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Line ", num, " in file '", path, "' holds invalid price value '", strArray[3], "': ", str }));
                }
                int volume = 0;
                try
                {
                    volume = Convert.ToInt32(strArray[3], provider);
                }
                catch (Exception)
                {
                    connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(connection, ErrorCode.Panic, "", string.Concat(new object[] { "Line ", num, " in file '", path, "' holds invalid volume '", strArray[4], "': ", str })));
                    goto Label_04A4;
                }
                if (buf == null)
                {
                    buf = new MarketDataBuf(symbol, time);
                }
                else if (buf.Date != time.Date)
                {
                    buf.Write();
                    buf = new MarketDataBuf(symbol, time);
                }
                if (!buf.MarketDataNow(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, marketDataType, price, volume, time)))
                {
                    reader.Close();
                    return;
                }
            Label_04A4:
                num++;
            }
            reader.Close();
            if (buf != null)
            {
                buf.Write();
            }
        }

        internal void MarketData(object sender, MarketDataEventArgs e)
        {
            if (!this.MarketDataNow(e))
            {
                this.Cancel();
            }
        }

        internal bool MarketDataNow(MarketDataEventArgs e)
        {
            lock (this)
            {
                byte[] buffer;
                IntPtr ptr;
                if ((e.Error != ErrorCode.NoError) || (e.MarketDataType.Id == MarketDataTypeId.Unknown))
                {
                    return true;
                }
                this.MoreBytes(11);
                DataFileHeader header = (DataFileHeader) this.headers[(int) e.MarketDataType.Id];
                int num = ++this.bytesInBuf;
                int num2 = ++this.bytesInBuf;
                int num3 = (int) Math.Round((double) (e.Time.Subtract(header.timeLast).TotalSeconds / Globals.recorderIntervalSeconds), 0);
                int volume = num3;
                if (num3 < 0)
                {
                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "MarketData (", e.MarketDataType.Id, ") for '", this.symbol.FullName, "' at ", e.Time, ": time is smaller than last recorded time stamp (", header.timeLast, ")" })));
                    return false;
                }
                (buffer = this.buf)[(int) (ptr = (IntPtr) num2)] = (byte) (buffer[(int) ptr] | ((byte) (((int) e.MarketDataType.Id) << 5)));
                if (volume != 0)
                {
                    if (volume > 0xff)
                    {
                        if (volume > 0xffff)
                        {
                            if (volume > 0xffffff)
                            {
                                this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "MarketData (", e.MarketDataType.Id, ") for '", this.symbol.FullName, "' at ", e.Time, " exceeds time gap maximum" })));
                                return false;
                            }
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 3);
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x10000);
                            volume -= (this.buf[this.bytesInBuf] * 0x100) * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x100);
                            volume -= this.buf[this.bytesInBuf] * 0x100;
                            this.buf[++this.bytesInBuf] = (byte) volume;
                        }
                        else
                        {
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 2);
                            this.buf[++this.bytesInBuf] = (byte) (volume / 0x100);
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
                header.timeLast = header.timeLast.AddSeconds(num3 * Globals.recorderIntervalSeconds);
                double num5 = this.symbol.Round2TickSize(e.Price);
                if (header.close != double.MinValue)
                {
                    header.high = Math.Max(header.high, num5);
                    header.low = Math.Min(header.low, num5);
                    volume = (int) Math.Round((double) ((num5 - header.close) / this.symbol.TickSize), 0);
                    if (volume != 0)
                    {
                        if ((-16 >= volume) || (volume >= 0x10))
                        {
                            if ((-128 >= volume) || (volume >= 0x80))
                            {
                                if ((-16384 >= volume) || (volume >= 0x4000))
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "MarketData (", e.MarketDataType.Id, ") for '", this.symbol.FullName, "' at ", e.Time, " exceeds max price gap (", e.Price, ")" })));
                                    return false;
                                }
                                volume += 0x4000;
                                (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 12);
                                this.buf[++this.bytesInBuf] = (byte) (volume / 0x100);
                                volume -= this.buf[this.bytesInBuf] * 0x100;
                                this.buf[++this.bytesInBuf] = (byte) volume;
                            }
                            else
                            {
                                volume += 0x80;
                                (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 8);
                                this.buf[++this.bytesInBuf] = (byte) volume;
                            }
                        }
                        else
                        {
                            volume += 0x10;
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 4);
                            (buffer = this.buf)[(int) (ptr = (IntPtr) num2)] = (byte) (buffer[(int) ptr] | ((byte) volume));
                        }
                    }
                }
                else
                {
                    header.high = num5;
                    header.low = num5;
                    header.open = num5;
                }
                header.close = num5;
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
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x100);
                        volume -= this.buf[this.bytesInBuf] * 0x100;
                        this.buf[++this.bytesInBuf] = (byte) volume;
                    }
                    else
                    {
                        volume = e.Volume;
                        (buffer = this.buf)[(int) (ptr = (IntPtr) num)] = (byte) (buffer[(int) ptr] | 0x70);
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x1000000);
                        volume -= ((this.buf[this.bytesInBuf] * 0x100) * 0x100) * 0x100;
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x10000);
                        volume -= (this.buf[this.bytesInBuf] * 0x100) * 0x100;
                        this.buf[++this.bytesInBuf] = (byte) (volume / 0x100);
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
            if (((this.bytesInBufRead + 11) > this.buf.Length) && !this.ReadBuf())
            {
                this.readCurrent = null;
                return false;
            }
            int bytesInBufRead = this.bytesInBufRead;
            byte num2 = this.buf[this.bytesInBufRead];
            byte num3 = this.buf[++this.bytesInBufRead];
            MarketDataTypeId id = (MarketDataTypeId) (num3 >> 5);
            DataFileHeader header = (DataFileHeader) this.headers[(int) id];
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
                priceCurrent += ((num3 & 0x1f) - 0x10) * header.tickSize;
            }
            else if ((num2 & 12) == 8)
            {
                priceCurrent += (this.buf[++this.bytesInBufRead] - 0x80) * header.tickSize;
            }
            else if ((num2 & 12) == 12)
            {
                priceCurrent += (((this.buf[++this.bytesInBufRead] * 0x100) + this.buf[++this.bytesInBufRead]) - 0x4000) * header.tickSize;
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
                this.readCurrent = new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, marketDataType, priceCurrent, volume, timeCurrent);
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
                    stream.Seek((long) ((8 * DataFileHeader.SizeOf) + this.bytesRead), SeekOrigin.Begin);
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
                    for (int i = 0; i < 8; i++)
                    {
                        DataFileHeader header = new DataFileHeader();
                        header.tickSize = this.symbol.TickSize;
                        header.timeCurrent = this.Date;
                        header.timeFirst = this.Date;
                        header.timeLast = this.Date;
                        this.headers.Add(header);
                    }
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
                        for (int j = 0; j < 8; j++)
                        {
                            stream.Read(buffer, 0, buffer.Length);
                            Bytes bytes = new Bytes();
                            bytes.Reset(buffer);
                            DataFileHeader header2 = (DataFileHeader) bytes.ReadSerializable();
                            header2.priceCurrent = header2.close;
                            header2.timeCurrent = header2.timeFirst;
                            this.headers.Add(header2);
                        }
                        this.totalBytes2Read = ((int) stream.Length) - (8 * buffer.Length);
                        stream.Close();
                    }
                    catch (Exception)
                    {
                        this.headers.Clear();
                        for (int k = 0; k < 8; k++)
                        {
                            DataFileHeader header3 = new DataFileHeader();
                            header3.tickSize = this.symbol.TickSize;
                            header3.timeCurrent = this.Date;
                            header3.timeFirst = this.Date;
                            header3.timeLast = this.Date;
                            this.headers.Add(header3);
                        }
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
            for (int i = 0; i < 8; i++)
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
                this.symbol.MarketData.MarketDataItem += new MarketDataItemEventHandler(this.MarketData);
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
            this.Reset();
            this.MoveNext();
            while ((this.readCurrent != null) && (this.readCurrent.Time < this.replayTime))
            {
                this.readCurrent.initOnly = true;
                this.symbol.connection.ProcessEventArgs(this.readCurrent);
                this.MoveNext();
            }
            if (this.symbol.MarketData.Ask != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.Ask);
            }
            if (this.symbol.MarketData.Bid != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.Bid);
            }
            if (this.symbol.MarketData.DailyHigh != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.DailyHigh);
            }
            if (this.symbol.MarketData.DailyLow != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.DailyLow);
            }
            if (this.symbol.MarketData.DailyVolume != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.DailyVolume);
            }
            if (this.symbol.MarketData.Last != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.Last);
            }
            if (this.symbol.MarketData.LastClose != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.LastClose);
            }
            if (this.symbol.MarketData.Opening != null)
            {
                this.symbol.connection.ProcessEventArgs(this.symbol.MarketData.Opening);
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
                        for (int i = 0; i < 8; i++)
                        {
                            Bytes bytes = new Bytes();
                            bytes.WriteSerializable((DataFileHeader) this.headers[i]);
                            stream.Write(bytes.Out, 0, bytes.OutLength);
                        }
                        stream.Seek(0L, SeekOrigin.End);
                        stream.Write(this.buf, 0, this.bytesInBuf + 1);
                        stream.Close();
                        for (int j = 0; j < this.buf.Length; j++)
                        {
                            this.buf[j] = 0;
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
                return (Globals.InstallDir + @"\db\data\" + this.symbol.FullName + @"\" + this.Date.ToString("yyyyMMdd") + ".tmt");
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

