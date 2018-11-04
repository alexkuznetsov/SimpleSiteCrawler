using System.Collections.Generic;

namespace SimpleSiteCrawler.Lib.Filter
{
    public interface ISitePageFilter
    {
        IEnumerable<SitePage> Apply(IEnumerable<SitePage> input);
    }
}