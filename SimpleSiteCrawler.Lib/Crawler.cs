using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSiteCrawler.Lib
{
    public class Crawler
    {
        private int _numberOfLinksLeft;

        private readonly ISitePageFilter[] _filters;

        public Crawler(SitePage startPage)
            : this(startPage
                , new ExcludeRootSitePageFilter(startPage.Uri)
                , new SameDomainFilter(startPage.Uri)
                , new AlreadyVisitedSitePageFilter())
        {
        }

        private Crawler(SitePage startPage, params ISitePageFilter[] filters)
        {
            StartPage = startPage;
            _filters = filters;
        }

        private SitePage StartPage { get; }

        public event EventHandler<SitePage> OnPageDownloadBegin;

        public event EventHandler<SitePage> OnPageDownloadComplete;

        public event EventHandler OnDownloadCompleted;

        public event EventHandler<Exception> OnError;

        public void Execute()
        {
            var task = CrawlAsync(StartPage);

            if (!task.IsCompleted)
                task.Start();
        }

        private async Task CrawlAsync(SitePage sitePage)
        {
            try
            {
                OnPageDownloadBegin?.Invoke(this, sitePage);
                await Downloader.Execute(sitePage);
                OnPageDownloadComplete?.Invoke(this, sitePage);
                var result = SitePageTemplateExtractor.FindAll(StartPage.Uri, sitePage.Contents, _filters);

                result.ForEach(async i =>
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