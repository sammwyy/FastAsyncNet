using System.Text;
namespace FastAsyncNet
{
    public class HttpHandler : ChannelHandler
    {

        public virtual void Handle(Request req) { }

        public override void Handle(Connection connection, byte[] data)
        {
            Request request = Request.FromBytes(data);
            this.Handle(request);
        }
    }
}