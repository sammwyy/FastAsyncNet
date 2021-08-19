using System.Text;
using System.Collections.Generic;

namespace FastAsyncNet
{
    public class Response
    {
        public string Version = "HTTP/1.1";
        private int Status = 200;
        private Dictionary<string, string> Headers;
        private Connection connection;
        private bool _hasSentHeaders = false;

        public Response()
        {
            this.Headers = new Dictionary<string, string>();
        }

        public Response(Connection connection) : this()
        {
            this.connection = connection;
        }

        public void AddHeader(string key, object value)
        {
            if (this.HasHeader(key))
            {
                this.DeleteHeader(key);
            }
            this.Headers.Add(key, value.ToString());
        }

        public void End()
        {
            if (!this._hasSentHeaders)
            {
                this.SendHeaders();
            }
            this.connection.Close();
        }

        public void DeleteHeader(string key)
        {
            this.Headers.Remove(key);
        }

        public string GetHeader(string key)
        {
            if (this.HasHeader(key))
            {
                return this.Headers[key];
            }
            else
            {
                return null;
            }
        }

        public bool HasHeader(string key)
        {
            return this.Headers.ContainsKey(key);
        }

        public string HeadersToString()
        {
            string result = this.Version.ToUpper() + " " + this.Status + " OK\n";

            foreach (var header in this.Headers)
            {
                result += header.Key + ": " + header.Value + "\n";
            }

            return result + "\n";
        }

        public void SendHeaders()
        {
            this.connection.Write(this.HeadersToString());
            this._hasSentHeaders = true;
        }

        public void Write(byte[] data)
        {
            if (!this._hasSentHeaders)
            {
                this.SendHeaders();
            }

            this.connection.Write(data);
        }

        public void Write(string data)
        {
            this.Write(Encoding.ASCII.GetBytes(data));
        }

        public override string ToString()
        {
            string result = this.HeadersToString();
            return result;
        }
    }
}