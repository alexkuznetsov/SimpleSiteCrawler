using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace SimpleSiteCrawler.Lib.Filter
{
    internal class AlreadyVisitedSitePageFilter : ISitePageFilter
    {
        private readonly ObjectCache _memoryCache = MemoryCache.Default;

        public IEnumerable<SitePage> Apply(IEnumerable<SitePage> input)
        {
            var filtered = input.Where(i => NotPresentInCache(i.Uri.ToString(), i.Uri))
                .ToArray();

            return filtered;
        }

        private bool NotPresentInCache(string key, Uri value)
        {
            var hasNotVisited = _memoryCache.AddOrGetExisting(key, value,
                                    policy: new CacheItemPolicy() {Priority = CacheItemPriority.NotRemovable})
                                == null;
            return hasNotVisited;
        }
    }
}