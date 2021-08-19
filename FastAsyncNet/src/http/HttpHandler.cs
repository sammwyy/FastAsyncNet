using System.Text;
namespace FastAsyncNet
{
    public class HttpHandler : ChannelHandler
    {

        public virtual void Handle(Request req, Response res)
        {
            res.End();
        }

        public override void Handle(Connection connection, byte[] data)
        {
            Request request = Request.FromBytes(data);
            Response response = new Response(connection);
            this.Handle(request, response);
        }
    }
}