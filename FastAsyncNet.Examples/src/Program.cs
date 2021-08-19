using FastAsyncNet;

namespace example
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            TcpServer server = new TcpServer(8080, "127.0.0.1");
            server.SetHandler(new Handler());
            server.Listen();
            Console.WriteLine("Started Server");
            */

            HTTPClient client = HTTPClient.createHTTPClient("http://google.com/");
            client.Send();
        }
    }

    class Handler : HttpHandler
    {
        public override void Handle(Request req, Response res)
        {
            res.Write("Hello World");
            res.End();
        }
    }
}
