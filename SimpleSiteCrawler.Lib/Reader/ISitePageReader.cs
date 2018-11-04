using System.Threading.Tasks;

namespace SimpleSiteCrawler.Lib.Reader
{
    internal interface ISitePageReader
    {
        Task<string> Read();
    }
}