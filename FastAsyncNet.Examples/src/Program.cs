using System;
using FastAsyncNet;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            SyncTcpServer server = new SyncTcpServer(8080, "127.0.0.1");
            server.SetHandler(new Handler());
            server.Listen();
            Console.WriteLine("Started Server");
        }
    }

    class Handler : HttpHandler
    {
        public override void Handle(Request req, Response res)
        {
            res.AddHeader("X-Testing", "Hello World");
            res.Write("Hello World");
            res.End();
        }
    }
}
