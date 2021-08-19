using System;
using FastAsyncNet;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8080, "127.0.0.1");
            Console.WriteLine("1Test");
            server.StartAsync();
            Console.WriteLine("2Test");
        }
    }
}
