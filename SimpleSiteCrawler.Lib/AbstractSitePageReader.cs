using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSiteCrawler.Lib
{
    internal abstract class AbstractSitePageReader : ISitePageReader
    {
        protected Stream Response { get; }

        protected AbstractSitePageReader(Stream response)
        {
            Response = response;
        }

        protected abstract Stream CreateReadStream();

        public async Task<string> Read()
        {
            using (var stream = CreateReadStream())
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync()
                        .ConfigureAwait(false);
                }
            }
        }
    }
}