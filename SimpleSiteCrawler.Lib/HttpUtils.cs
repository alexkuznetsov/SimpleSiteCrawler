using System;

namespace SimpleSiteCrawler.Lib
{
    internal static class HttpUtils
    {
        public static bool TryParseUri(string href, out Uri uri)
        {
            try
            {
                uri = new Uri(href);
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    uri = new Uri('/'+href);
                    return true;
                }
                catch (Exception exception)
                {
                    uri = null;
                    return false;
                }
            }
        }
    }
}