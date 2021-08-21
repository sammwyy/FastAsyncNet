using System;
using FastAsyncNet;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServer server = new TcpServer(8080, "127.0.0.1");
            server.SetHandler(new Handler());
            server.Listen();
            Console.WriteLine("Started Server");
        }
    }

    class Handler : HttpHandler
    {
        public override void Handle(Request req, Response res)
        {
            HTTPClient client = HTTPClient.createHTTPClient("http://2lstudios.dev/");
            Response proxyResponse = client.Send();
            res.Write(proxyResponse.Body.GetAsString());
            res.End();
        }
    }
}
