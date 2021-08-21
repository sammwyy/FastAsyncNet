using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
namespace FastAsyncNet
{
    public class HTTPClient
    {
        private TcpClient client;
        private Request request;
        private string address;
        private int port;

        public HTTPClient(string address, int port)
        {
            this.address = address;
            this.port = port;
            this.client = new TcpClient();
            this.request = new Request();
        }

        public void AddHeader(string key, object value)
        {
            this.request.AddHeader(key, value);
        }

        public void SetMethod(string method)
        {
            this.request.Method = method;
        }

        public Response Send(int timeout)
        {
            // Connect
            this.client.Connect(this.address, this.port);
            NetworkStream stream = new NetworkStream(this.client.Client);
            // Stream stream = this.client.GetStream();

            // Send request
            byte[] bytes = this.request.ToBytes();
            stream.Write(bytes, 0, bytes.Length);

            // Receive response
            String response = StreamUtils.ReadStreamToEnd(stream);

            return Response.FromString(response);
        }

        public Response Send()
        {
            return this.Send(10 * 1000);
        }

        public static HTTPClient createHTTPClient(string method, string url)
        {
            Uri uri = new Uri(url);
            HTTPClient client = new HTTPClient(uri.Host, uri.Port);
            client.SetMethod(method);
            client.AddHeader("Host", uri.Host);
            return client;
        }

        public static HTTPClient createHTTPClient(string url)
        {
            return HTTPClient.createHTTPClient("get", url);
        }
    }
}