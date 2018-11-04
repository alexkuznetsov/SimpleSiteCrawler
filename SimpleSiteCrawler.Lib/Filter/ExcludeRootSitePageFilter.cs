using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleSiteCrawler.Lib.Filter
{
    internal class ExcludeRootSitePageFilter : ISitePageFilter
    {
        private readonly Uri _root;

        public ExcludeRootSitePageFilter(Uri root)
        {
            _root = root;
        }

        public IEnumerable<SitePage> Apply(IEnumerable<SitePage> input)
        {
            var collected = input.Where(i => i.Uri != _root).ToArray();

            return collected;
        }
    }
}