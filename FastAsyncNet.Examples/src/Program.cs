using System;
using FastAsyncNet;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServer server = new TcpServer(8080, "127.0.0.1");
            Console.WriteLine("1Test");
            server.Listen();
        }
    }
}
