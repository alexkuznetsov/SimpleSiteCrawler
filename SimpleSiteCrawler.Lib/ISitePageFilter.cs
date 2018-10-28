using System.Collections.Generic;

namespace SimpleSiteCrawler.Lib
{
    internal interface ISitePageFilter
    {
        IEnumerable<SitePage> Apply(IEnumerable<SitePage> input);
    }
}