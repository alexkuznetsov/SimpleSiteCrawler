using System.Net;

namespace SimpleSiteCrawler.Lib.Reader
{
    internal static class SitePageReaderFactory
    {
        public static ISitePageReader Create(HttpWebResponse response)
        {
            var contentEncoding = response.ContentEncoding;
            var responseStream = response.GetResponseStream();

            if (GZipSitePageReader.CanAccept(contentEncoding))
                return new GZipSitePageReader(responseStream);

            if (DeflateSitePageReader.CanAccept(contentEncoding))
                return new DeflateSitePageReader(responseStream);

            return new RawSitePageReader(responseStream);
        }
    }
}