namespace FastAsyncNet
{
    public class ServerHandler
    {
        public virtual void Handle(Connection connection, byte[] data) { }
        public virtual void HandleConnection(Connection connection) { }
        public virtual void HandleDisconnection(Connection disconnection) { }
    }
}