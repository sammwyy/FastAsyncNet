using System.Text;
using System.Collections.Generic;

namespace FastAsyncNet
{
    public abstract class HTTPContextProp
    {
        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        public abstract string HeadersToString();
        public void AddHeader(string key, object value)
        {
            if (this.HasHeader(key))
            {
                this.DeleteHeader(key);
            }
            this.Headers.Add(key, value.ToString());
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

        public override string ToString()
        {
            string result = this.HeadersToString();
            return result;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString());
        }
    }
}