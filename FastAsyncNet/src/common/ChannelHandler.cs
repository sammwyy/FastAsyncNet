namespace FastAsyncNet
{
    public class ChannelHandler
    {
        public virtual void Handle(Connection connection, byte[] data) { }
        public virtual void HandleConnection(Connection connection) { }
    }
}