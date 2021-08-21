using System.Text;
using System.Net.Sockets;
using System.IO;
namespace FastAsyncNet
{
    public static class StreamUtils
    {
        public static string ReadStreamToEnd(NetworkStream stream)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] data = new byte[1024];
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                memoryStream.Write(data, 0, bytes);
            }
            while (stream.DataAvailable);
            return Encoding.ASCII.GetString(memoryStream.ToArray());
        }
    }
}