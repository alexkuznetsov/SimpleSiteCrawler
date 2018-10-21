using System.Threading.Tasks;

namespace SimpleSiteCrawler.Lib
{
    internal interface ISitePageReader
    {
        Task<string> Read();
    }
}