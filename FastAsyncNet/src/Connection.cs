using System.Text;

namespace FastAsyncNet
{
    public abstract class Connection
    {
        public abstract void Close();

        public abstract bool IsConnected();

        public abstract void Write(byte[] data);

        public void Write(string data)
        {
            this.Write(Encoding.ASCII.GetBytes(data));
        }
    }
}
