using System.Text;

namespace FastAsyncNet
{
    public abstract class Connection
    {
        public abstract void Close();

        public abstract bool IsConnected();

        public abstract void Send(byte[] data);

        public void Send(string data)
        {
            this.Send(Encoding.ASCII.GetBytes(data));
        }
    }
}
