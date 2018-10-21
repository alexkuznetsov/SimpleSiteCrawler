using System.IO;
using System.IO.Compression;

namespace SimpleSiteCrawler.Lib
{
    internal class GZipSitePageReader : AbstractSitePageReader
    {
        public GZipSitePageReader(Stream response) : base(response)
        {
        }

        protected override Stream CreateReadStream()
        {
            return new GZipStream(Response, CompressionMode.Decompress);
        }
    }
}