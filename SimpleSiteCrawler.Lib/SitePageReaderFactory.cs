using System.Net;

namespace SimpleSiteCrawler.Lib
{
    internal static class SitePageReaderFactory
    {
        private const string EncodingGzip = "gzip";
        private const string EncodingDeflate = "deflate";

        public static ISitePageReader Create(HttpWebResponse response)
        {
            var contentEncoding = response.ContentEncoding;

            if (contentEncoding.ToLower().Contains(EncodingGzip))
                return new GZipSitePageReader(response.GetResponseStream());
            else if (contentEncoding.ToLower().Contains(EncodingDeflate))
                return new DeflateSitePageReader(response.GetResponseStream());
            else
                return new RawSitePageReader(response.GetResponseStream());
        }
    }
}