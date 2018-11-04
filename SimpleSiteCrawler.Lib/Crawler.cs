using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleSiteCrawler.Lib.Filter;

namespace SimpleSiteCrawler.Lib
{
    public class Crawler
    {
        private int _numberOfLinksLeft;

        private readonly ISitePageFilter[] _filters;

        public static Crawler CreateDefault(Uri rootUri)
            => new Crawler(new ISitePageFilter[]
            {
                new ExcludeRootSitePageFilter(rootUri),
                new SameDomainFilter(rootUri),
                new AlreadyVisitedSitePageFilter()
            });

        private Crawler(ISitePageFilter[] filters)
        {
            _filters = filters;
        }

        public event EventHandler<SitePage> OnPageDownloadBegin;

        public event EventHandler<SitePage> OnPageDownloadComplete;

        public event EventHandler OnDownloadCompleted;

        public event EventHandler<Exception> OnError;

        public async Task CrawlAsync(SitePage sitePage)
        {
            try
            {
                OnPageDownloadBegin?.Invoke(this, sitePage);
                await Downloader.Execute(sitePage);
                OnPageDownloadComplete?.Invoke(this, sitePage);

                SitePageTemplateExtractor.FindAll(sitePage.Uri, sitePage.Contents, _filters)
                    .ForEach(async i =>
                    {
                        Interlocked.Increment(ref _numberOfLinksLeft);
                        await CrawlAsync(i);
                    });
            }
            catch (Exception exc)
            {
                OnError?.Invoke(this, exc);
            }

            if (Interlocked.Decrement(ref _numberOfLinksLeft) == 0)
                OnDownloadCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}