using System;
using SimpleSiteCrawler.Lib;

namespace SimpleSiteCrawler.Cli
{
    internal static class DownloadConductor
    {
        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.CreateLogger);

        private static ILogger Logger => LoggerLazy.Value;

        public static void Start(Options options)
        {
            var startPage = new SitePage {Uri = new Uri(options.Site)};
            var crawler = Crawler.CreateDefault(startPage.Uri);

            crawler.OnDownloadCompleted += (s, e) => Logger.Info("Download completed!");
            crawler.OnPageDownloadBegin += (s, p) => Logger.Info($"{p.Uri} download start");
            crawler.OnPageDownloadComplete += (s, p) => Logger.Info($"[OK] {p.Uri.AbsolutePath}");
            crawler.OnPageDownloadComplete += (s, p) => SaveHelper.SaveResult(options, p);
            crawler.OnError += (s, exc) => Logger.Error(exc);

            var task = crawler.CrawlAsync(startPage);

            if (!task.IsCompleted)
                task.Start();
        }
    }
}