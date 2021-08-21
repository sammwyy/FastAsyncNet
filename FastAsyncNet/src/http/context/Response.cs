using System;
using System.Text;

namespace FastAsyncNet
{
    public class Response : HTTPContextProp
    {
        public string Version = "HTTP/1.1";
        public int Status = 200;
        private Connection connection;
        private bool _hasSentHeaders = false;

        public Response() { }

        public Response(Connection connection)
        {
            this.connection = connection;
        }

        public void End()
        {
            if (!this._hasSentHeaders)
            {
                this.SendHeaders();
            }
            this.connection.Close();
        }

        public override string HeadersToString()
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
            this.Write(Encoding.UTF8.GetBytes(data));
        }

        public static Response FromString(string raw)
        {
            Response res = new Response();
            string[] lines = raw.Split("\n");

            bool firstLine = true;
            bool isBody = false;

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
                    if (!isBody && line.Trim() == "")
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

                    else
                    {
                        res.Body.AppendLine(line);
                    }
                }
            }
            return res;
        }
    }
}