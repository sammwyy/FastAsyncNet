using System.Net;
using System.Net.Sockets;

namespace FastAsyncNet
{
    public class Server
    {
        public readonly string Host;

        public readonly int Port;

        public readonly IPAddress Address;

        public readonly TcpListener Socket;

        public Server(int port, string host)
        {
            this.Port = port;
            this.Host = host;
            this.Address = IPAddress.Parse(host);
            this.Socket = new TcpListener(this.Address, this.Port);
        }
    }
}
