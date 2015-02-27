namespace iTrading.Dtn
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using iTrading.Core.Kernel;

    internal class DtnSocket
    {
        private Adapter adapter;
        private ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        private int byteCarriageReturn = 13;
        private int byteNewLine = 10;
        private int bytesInBuffer = 0;
        private string host;
        private int nextByte2Read = 0;
        private int port;
        private byte[] readBuffer = new byte[0x400];
        private Socket socket = null;

        internal DtnSocket(Adapter adapter, string host, int port)
        {
            this.adapter = adapter;
            this.host = host;
            this.port = port;
        }

        internal void Close()
        {
            if (this.socket != null)
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
        }

        internal void Connect()
        {
            if (!this.Connected)
            {
                IPEndPoint remoteEP = new IPEndPoint(Dns.Resolve(this.host).AddressList[0], this.port);
                this.socket = new Socket(remoteEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(remoteEP);
            }
        }

        internal string Read()
        {
            string str;
            if (!this.Connected)
            {
                return "";
            }
            do
            {
                bool flag;
                str = "";
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
                            this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "Dtn.DtnSocket.Read: " + exception4.Message));
                            return "";
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
                        if (this.readBuffer[this.nextByte2Read + count] == this.byteNewLine)
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
                if ((str.Length >= 1) && (str[str.Length - 1] == this.byteCarriageReturn))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                if (Globals.TraceSwitch.Native)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.Id, ") Dtn.DtnSocket.Read", this.port, ": msg='", str, "'" }));
                }
            }
            while ((str.Length <= 0) || !(str != "0"));
            return str;
        }

        internal void Write(string buf)
        {
            if (this.Connected)
            {
                if (Globals.TraceSwitch.Native)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.Id, ") Dtn.DtnSocket.Write", this.port, ": msg='", buf, "'" }));
                }
                byte[] bytes = new byte[buf.Length + 2];
                int size = this.asciiEncoding.GetBytes(buf, 0, buf.Length, bytes, 0);
                try
                {
                    this.socket.Send(bytes, size, SocketFlags.None);
                }
                catch (SocketException exception)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Dtn.DtnSocket.Write1: ", exception.Message, " (", exception.ErrorCode, "/", exception.NativeErrorCode, ")" })));
                }
                catch (Exception exception2)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "Dtn.DtnSocket.Write2: " + exception2.Message));
                }
            }
        }

        internal bool Connected
        {
            get
            {
                if (this.socket == null)
                {
                    return false;
                }
                return this.socket.Connected;
            }
        }
    }
}

