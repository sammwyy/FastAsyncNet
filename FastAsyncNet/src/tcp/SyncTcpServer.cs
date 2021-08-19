using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using System.Threading;

namespace FastAsyncNet
{
    public class SyncTcpServer
    {
        private TcpListener _listener;
        private ChannelHandler _handler;

        public SyncTcpServer(int port, string host)
        {
            this._listener = new TcpListener(IPAddress.Parse(host), port); ;
        }

        public void SetHandler(ChannelHandler handler)
        {
            this._handler = handler;
        }

        public void Listen()
        {
            if (this._handler == null)
            {
                throw new Exception("No handler defined in Server. You maybe forgot to use server.SetHandler(handler:ServerHandler)?");
            };

            this._listener.Start();
            while (true)
            {
                TcpClient client = this._listener.AcceptTcpClient();
                TcpConnection connection = new TcpConnection(client, this._handler);
                this._handler.HandleConnection(connection);
            }
        }
    }
}