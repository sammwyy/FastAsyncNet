using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FastAsyncNet
{
    public static class URLUtils
    {
        private static Regex
            _sanitizePathRegex = new Regex(@"[^\\/]+(?<!\.\.)[\\/]\.\.[\\/]");

        public static string Unescape(string text)
        {
            return Uri.UnescapeDataString(text);
        }

        public static string Escape(string text)
        {
            return Uri.EscapeUriString(text);
        }

        public static string SanitizePath(string path)
        {
            while (true)
            {
                string newPath = _sanitizePathRegex.Replace(path, "");
                if (newPath == path) break;
                path = newPath;
            }
            path = path.Replace("../", "").Replace("//", "/");

            if (path == "")
            {
                return "/";
            }
            else
            {
                return path;
            }
        }

        public static string StripQuery(string path)
        {
            string[] parts = path.Split("?");
            if (parts.Length == 1)
            {
                return "";
            }
            else
            {
                return parts[1];
            }
        }

        public static string StripPath(string path)
        {
            return path.Split("?")[0];
        }

        public static Dictionary<string, string> ParseQuery(string rawQuery)
        {
            Dictionary<string, string> query = new Dictionary<string, string>();
            string[] parts = rawQuery.Split("&");
            foreach (string part in parts)
            {
                string[] keyValue = part.Split("=");
                string key = keyValue[0];
                string value = null;

                if (keyValue.Length == 2)
                {
                    value = keyValue[1];
                }

                query.Add(key, value);
            }
            return query;
        }
    }
}
