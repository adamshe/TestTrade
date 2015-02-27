namespace iTrading.IB
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using iTrading.Core.Kernel;

    internal class IBSocket
    {
        private Adapter adapter;
        private ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        private int bytesInBuffer = 0;
        private int nextByte2Read = 0;
        internal NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
        private byte[] readBuffer = new byte[0x400];
        internal int ServerVersion = 1;
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        internal IBSocket(Adapter adapter)
        {
            this.adapter = adapter;
            this.numberFormatInfo.NumberDecimalSeparator = ".";
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, adapter.Options.SendTimeOutMilliSeconds);
        }

        internal void Clear()
        {
            int num2;
            byte[] buffer = new byte[1];
            int socketOption = (int) this.socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 50);
            do
            {
                num2 = 0;
                try
                {
                    num2 = this.socket.Receive(buffer, 1, SocketFlags.None);
                }
                catch (Exception)
                {
                    break;
                }
            }
            while (num2 != 0);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, socketOption);
        }

        internal void Close()
        {
            try
            {
                this.socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
            }
            this.socket.Close();
        }

        internal void Connect()
        {
            if (!this.Connected)
            {
                IPAddress address = Dns.Resolve(this.adapter.Options.Host).AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(address, this.adapter.Options.Port);
                this.socket.Connect(remoteEP);
            }
        }

        internal double ReadDouble()
        {
            string str = this.ReadString();
            double num = 0.0;
            if (str.Length == 0)
            {
                return 0.0;
            }
            try
            {
                num = Convert.ToDouble(str, this.numberFormatInfo);
            }
            catch (Exception)
            {
                this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "IB.IBSocket.ReadDouble: Unable to convert '" + str + "' to double value"));
            }
            return num;
        }

        internal DateTime ReadExpiry()
        {
            string str = this.ReadString();
            if (str.Length == 0)
            {
                return Globals.MaxDate;
            }
            return new DateTime(Convert.ToInt32(str.Substring(0, 4)), Convert.ToInt32(str.Substring(4, 2)), 1);
        }

        internal int ReadInteger()
        {
            string str = this.ReadString();
            int num = 0;
            if (str.Length != 0)
            {
                try
                {
                    num = Convert.ToInt32(str, this.numberFormatInfo);
                }
                catch (Exception)
                {
                    string str2 = "IB.IBSocket.ReadInteger: Unable to convert '" + str + "' to integer value";
                    if (!this.Connected)
                    {
                        throw new SocketClosedException("IB.IBSocket.ReadInteger: " + str2 + " '" + str + "'");
                    }
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "IB.IBSocket.ReadInteger: Unable to convert '" + str + "' to integer value"));
                }
            }
            return num;
        }

        internal iTrading.IB.Right ReadRight()
        {
            string str = this.ReadString();
            if (str == "?")
            {
                str = "";
            }
            try
            {
                return (iTrading.IB.Right) Names.Rights.GetKey(Names.Rights.IndexOfValue(str));
            }
            catch (Exception)
            {
                this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "IB.IBSocket.ReadRight: Unkown right '" + str + "'"));
                return iTrading.IB.Right.ANY;
            }
        }

        internal string ReadString()
        {
            string str = "";
            if (this.Connected)
            {
                bool flag;
                do
                {
                    if (this.nextByte2Read >= this.bytesInBuffer)
                    {
                        try
                        {
                            this.bytesInBuffer = this.socket.Receive(this.readBuffer, this.readBuffer.Length, SocketFlags.None);
                        }
                        catch (SocketException exception)
                        {
                            throw new SocketClosedException(string.Concat(new object[] { "System.Net.Sockets.SocketException: ", exception.Message, " (", exception.ErrorCode, "/", exception.NativeErrorCode, ")" }));
                        }
                        catch (ObjectDisposedException exception2)
                        {
                            throw new SocketClosedException("System.ObjectDisposedException: " + exception2.Message);
                        }
                        catch (ThreadAbortException exception3)
                        {
                            throw new SocketClosedException("System.Threading.ThreadAbortException: " + exception3.Message);
                        }
                        catch (Exception exception4)
                        {
                            this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "IB.IBSocket.ReadString: " + exception4.Message));
                            str = "";
                            break;
                        }
                        if (this.bytesInBuffer == 0)
                        {
                            throw new SocketClosedException("No more data to read");
                        }
                        this.nextByte2Read = 0;
                    }
                    int count = 0;
                    flag = false;
                    while ((this.nextByte2Read + count) < this.bytesInBuffer)
                    {
                        if (this.readBuffer[this.nextByte2Read + count] == 0)
                        {
                            flag = true;
                            break;
                        }
                        count++;
                    }
                    str = str + this.asciiEncoding.GetString(this.readBuffer, this.nextByte2Read, count);
                    this.nextByte2Read += count + 1;
                }
                while (!flag);
                if (Globals.TraceSwitch.Native)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.Id, ") IB.IBSocket.Read: msg='", str, "'" }));
                }
            }
            return str;
        }

        internal void Send(double val)
        {
            this.Send(val.ToString(this.numberFormatInfo));
        }

        internal void Send(int val)
        {
            this.Send(val.ToString(this.numberFormatInfo));
        }

        internal void Send(string buf)
        {
            if (this.Connected)
            {
                if (Globals.TraceSwitch.Native)
                {
                    Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") IB.IBSocket.Send: msg='" + buf + "'");
                }
                byte[] bytes = new byte[buf.Length + 2];
                int index = this.asciiEncoding.GetBytes(buf, 0, buf.Length, bytes, 0);
                bytes[index] = 0;
                try
                {
                    this.socket.Send(bytes, index + 1, SocketFlags.None);
                }
                catch (SocketException exception)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", string.Concat(new object[] { "IB.IBSocket.Send1: ", exception.Message, " (", exception.ErrorCode, "/", exception.NativeErrorCode, ")" })));
                }
                catch (Exception exception2)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "IB.IBSocket.Send2: " + exception2.Message));
                }
            }
        }

        internal bool Connected
        {
            get
            {
                return this.socket.Connected;
            }
        }
    }
}

