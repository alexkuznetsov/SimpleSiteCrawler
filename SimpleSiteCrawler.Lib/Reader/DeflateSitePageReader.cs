using System.IO;
using System.IO.Compression;

namespace SimpleSiteCrawler.Lib.Reader
{
    internal class DeflateSitePageReader : AbstractSitePageReader
    {
        private const string EncodingDeflate = "deflate";

        public static bool CanAccept(string contentEncoding) =>
            contentEncoding.ToLower().Contains(EncodingDeflate);

        public DeflateSitePageReader(Stream response) : base(response)
        {
        }

        protected override Stream CreateReadStream()
        {
            return new DeflateStream(Response, CompressionMode.Decompress);
        }
    }
}