using System;
using System.IO;
using SimpleSiteCrawler.Lib;

namespace SimpleSiteCrawler.Cli
{
    // ReSharper disable once ArrangeTypeModifiers
    class Program
    {
        private static string GetCurrentFolder() => Path.GetDirectoryName(typeof(Program).Assembly.Location);

        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main(string[] args)
        {
            var site = args != null && args.Length >= 1 ? args[0] : null;

            if (string.IsNullOrEmpty(site))
            {
                Console.WriteLine("Please, provide site to download!");
                Console.WriteLine("Example: dotnet SimpleSiteCrawler.Cli.dll https://codyhouse.co/");
                return;
            }

            var downloadToFolder = Path.Combine(GetCurrentFolder(), "download", SaveHelper.MakePath(site));

            var crawler = new Crawler(new SitePage
            {
                Uri = new Uri(site)
            });

            crawler.OnDownloadCompleated += (sender, eventArgs) => Console.WriteLine("Download compleated!");
            crawler.OnPageDownloadBegin += (sender, page) => Console.WriteLine($"{page.Uri} download start");
            crawler.OnPageDownloadCompleate += (sender, page) =>
            {
                Console.WriteLine($"{DateTime.Now}\t [OK] {page.Uri.AbsolutePath}");
                SaveHelper.SaveResult(downloadToFolder, page);
            };

            crawler.Execute();

            Console.ReadLine();
        }
    }
}