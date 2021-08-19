using System;
using FastAsyncNet;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServer server = new TcpServer(8080, "127.0.0.1");
            server.SetHandler(new ChannelHandler());
            server.Listen();
            Console.WriteLine("Started Server");
        }
    }

    class Handler : ChannelHandler
    {
        public override void Handle(Connection connection, byte[] data)
        {
            connection.Write("HTTP/1.1 200 OK\nContent-Length: 11\n\nHello World");
        }
    }
}
