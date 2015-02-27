using System;
using System.Net.Sockets;
using System.Threading;
using iTrading.Core.Data;

namespace iTrading.Client
{
    public class ITradingSocket
    {
        private Bytes bytes;
        private object[] receiveSync = new object[0];
        private object[] sendSync = new object[0];
        private Socket socket;

        public ITradingSocket(Socket socket, Bytes bytes)
        {
            this.bytes = bytes;
            this.socket = socket;
        }

        public void Close()
        {
            if (this.socket.Connected)
            {
                this.socket.Close();
            }
        }

        public Bytes Receive()
        {
            Bytes bytes;
            if (!this.socket.Connected)
            {
                this.bytes.Reset(new byte[0]);
                return this.bytes;
            }
            byte[] buffer = new byte[4];
            try
            {
                lock (this.receiveSync)
                {
                    if (this.socket.Receive(buffer, 4, SocketFlags.None) != 4)
                    {
                        throw new SocketClosedException("ITrading client has closed socket");
                    }
                    byte[] buffer2 = new byte[BitConverter.ToInt32(buffer, 0)];
                    if (this.socket.Receive(buffer2, buffer2.Length, SocketFlags.None) != buffer2.Length)
                    {
                        throw new SocketClosedException("ITrading client has closed socket");
                    }
                    this.bytes.Reset(buffer2);
                    bytes = this.bytes;
                }
            }
            catch (SocketException exception)
            {
                throw new SocketClosedException(exception.Message);
            }
            catch (ObjectDisposedException exception2)
            {
                throw new SocketClosedException(exception2.Message);
            }
            catch (ThreadAbortException exception3)
            {
                throw new SocketClosedException(exception3.Message);
            }
            return bytes;
        }

        public void Send()
        {
            if (this.socket.Connected)
            {
                try
                {
                    lock (this.sendSync)
                    {
                        this.socket.Send(BitConverter.GetBytes(this.bytes.OutLength), 4, SocketFlags.None);
                        this.socket.Send(this.bytes.Out, this.bytes.OutLength, SocketFlags.None);
                        this.bytes.Reset();
                    }
                }
                catch (SocketException exception)
                {
                    throw new SocketClosedException(exception.Message);
                }
                catch (ObjectDisposedException exception2)
                {
                    throw new SocketClosedException(exception2.Message);
                }
                catch (ThreadAbortException exception3)
                {
                    throw new SocketClosedException(exception3.Message);
                }
            }
        }

        public Bytes Bytes
        {
            get
            {
                return this.bytes;
            }
        }

        public bool Connected
        {
            get
            {
                return this.socket.Connected;
            }
        }

        public object ReceiveSync
        {
            get
            {
                return this.receiveSync;
            }
        }

        public object SendSync
        {
            get
            {
                return this.sendSync;
            }
        }
    }
}