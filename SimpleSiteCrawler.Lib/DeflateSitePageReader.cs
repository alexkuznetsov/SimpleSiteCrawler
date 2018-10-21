using System.IO;
using System.IO.Compression;

namespace SimpleSiteCrawler.Lib
{
    internal class DeflateSitePageReader : AbstractSitePageReader
    {
        public DeflateSitePageReader(Stream response) : base(response)
        {
        }

        protected override Stream CreateReadStream()
        {
            return new DeflateStream(Response, CompressionMode.Decompress);
        }
    }
}