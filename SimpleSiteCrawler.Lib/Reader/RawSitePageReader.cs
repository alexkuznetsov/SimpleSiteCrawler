using System.IO;

namespace SimpleSiteCrawler.Lib.Reader
{
    internal class RawSitePageReader : AbstractSitePageReader
    {
        public RawSitePageReader(Stream stream) : base(stream)
        {
        }

        protected override Stream CreateReadStream() => Response;
    }
}