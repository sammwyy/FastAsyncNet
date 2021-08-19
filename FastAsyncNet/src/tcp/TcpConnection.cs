using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FastAsyncNet
{
    public class TcpConnection : Connection
    {
        private TcpClient _client;
        private ChannelHandler _handler;
        private Thread _thread;

        public TcpConnection(TcpClient client, ChannelHandler handler)
        {
            this._client = client;
            this._handler = handler;
            // this._stream = client.GetStream();
            this._thread = new Thread(this.ReadClient);
            this._thread.Start();
        }

        private void ReadClient()
        {
            while (this.IsConnected())
            {
                try
                {
                    using (NetworkStream stream = this._client.GetStream())
                    {

                        Byte[] bytes = new Byte[1024];
                        int length;

                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var data = new byte[length];
                            Array.Copy(bytes, 0, data, 0, length);
                            this._handler.Handle(this, data);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        public override void Close()
        {
            this._client.Close();
        }

        public override bool IsConnected()
        {
            return this._client.Connected;
        }

        public override void Write(byte[] data)
        {
            NetworkStream stream = this._client.GetStream();
            if (stream.CanWrite)
            {
                this._client.GetStream().Write(data, 0, data.Length);
            }
        }
    }
}
