using System;
using System.Text;
using System.Collections.Generic;

namespace FastAsyncNet
{
    public class Response
    {
        public string Version = "HTTP/1.1";
        public int Status = 200;
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

        public static Response FromString(string raw)
        {
            Response res = new Response();
            string[] lines = raw.Split("\n");

            bool firstLine = true;
            bool isBody = false;
            string prevLine = "";

            foreach (string line in lines)
            {
                if (firstLine)
                {
                    firstLine = false;
                    string[] lineParts = line.Split(" ");
                    if (lineParts.Length >= 2)
                    {
                        res.Version = lineParts[0];
                        res.Status = Int32.Parse(lineParts[1]);
                    }
                }
                else
                {
                    if (!isBody && line == "" && prevLine == "")
                    {
                        isBody = true;
                    }

                    if (!isBody)
                    {
                        string[] header = line.Split(": ");
                        if (header.Length == 2)
                        {
                            string key = header[0];
                            string value = header[1];
                            res.AddHeader(key, value);
                        }
                    }
                }

                prevLine = line;
            }

            return res;
        }
    }
}