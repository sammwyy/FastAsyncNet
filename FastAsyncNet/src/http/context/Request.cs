using System.Text;
using System.Collections.Generic;

namespace FastAsyncNet
{
    public class Request : HTTPContextProp
    {
        // Mandatory data
        public string Method = "get";

        private string _path = "/";

        public string Version = "HTTP/1.1";

        // Optional data

        public Dictionary<string, string> Query;

        public Request(string method, string path)
        {
            this.Method = method;
            this.Path = path;
            this.Query = new Dictionary<string, string>();
            this.Headers = new Dictionary<string, string>();
            this.Headers.Add("User-Agent", "FastAsyncNet");
        }

        public Request() : this(null, null) { }


        public static Request FromString(string data)
        {
            Request req = new Request();

            string[] lines = data.Split("\n");
            string prevLine = "";
            bool isBody = false;

            foreach (string line in lines)
            {
                if (req.Method == null)
                {
                    string[] firstLine = line.Split(" ");
                    if (firstLine.Length == 3)
                    {
                        req.Method = firstLine[0];
                        req.Path = firstLine[1];
                        req.Version = firstLine[2];
                    }
                }
                else
                {
                    if (!isBody)
                    {
                        if (line == "" && prevLine == "")
                        {
                            isBody = true;
                        }
                        else
                        {
                            string[] header = line.Split(": ");
                            if (header.Length == 2)
                            {
                                string key = header[0];
                                string value = header[1];
                                req.AddHeader(key, value);
                            }
                        }
                    }
                    else
                    {
                        req.Body.Append(line);
                    }
                }

                prevLine = line;
            }

            return req;
        }

        public static Request FromBytes(byte[] buffer)
        {
            return Request.FromString(Encoding.UTF8.GetString(buffer));
        }

        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                if (value != null)
                {
                    string raw = URLUtils.Unescape(value);

                    string rawPath = URLUtils.StripPath(raw);
                    string rawQuery = URLUtils.StripQuery(raw);

                    this._path = URLUtils.SanitizePath(rawPath);
                    this.Query = URLUtils.ParseQuery(rawQuery);
                }
            }
        }

        public void AddQuery(string key, object value)
        {
            if (this.HasQuery(key))
            {
                this.DeleteQuery(key);
            }
            this.Query.Add(key, value.ToString());
        }
        public void DeleteQuery(string key)
        {
            this.Query.Remove(key);
        }

        public bool HasQuery(string key)
        {
            return this.Query.ContainsKey(key);
        }

        public string GetQuery(string key)
        {
            if (this.HasQuery(key))
            {
                return this.Query[key];
            }
            else
            {
                return null;
            }
        }

        public string FullPath
        {
            get
            {
                return this.Path + this.QueryToString();
            }
        }

        public string QueryToString()
        {
            string result = "";
            foreach (var query in this.Query)
            {
                if (result == "")
                {
                    result = "?";
                }
                else
                {
                    result += "&";
                }

                result += query.Key + "=" + query.Value;
            }
            return result;
        }

        public override string HeadersToString()
        {
            string result =
                this.Method.ToUpper() +
                " " +
                URLUtils.Escape(this.FullPath) +
                " " +
                this.Version +
                "\n";
            foreach (var header in this.Headers)
            {
                result += header.Key + ": " + header.Value + "\n";
            }

            return result + "\n";
        }
    }
}
