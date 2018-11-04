using System;
using System.Net;
using System.Threading.Tasks;
using SimpleSiteCrawler.Lib.Reader;

namespace SimpleSiteCrawler.Lib
{
    internal static class Downloader
    {
        private const string DefaultUserAgent =
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36 OPR/54.0.2952.46";

        private const string VerbGet = "GET";

        private const string AcceptAllowAll = "*/*";

        private const string AllowedEncodings = "gzip,deflate";

        public static async Task Execute(SitePage page)
        {
            var request = WebRequest.CreateHttp(page.Uri);

            request.Accept = AcceptAllowAll;
            request.AllowAutoRedirect = true;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, AllowedEncodings);

            request.Method = VerbGet;
            request.Timeout = Convert.ToInt32(TimeSpan.FromMinutes(1).TotalMilliseconds);
            request.UserAgent = DefaultUserAgent;

            using (var response = (HttpWebResponse) request.GetResponse())
            {
                page.Contents = await SitePageReaderFactory.Create(response).Read();
            }
        }
    }
}