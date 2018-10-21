using System;

namespace SimpleSiteCrawler.Cli
{
    // ReSharper disable once ArrangeTypeModifiers
    class Program
    {
        private static readonly Lazy<ILogger> LoggerLazy = new Lazy<ILogger>(LoggerFactory.CreateLogger());

        private static ILogger Logger => LoggerLazy.Value;

        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main(string[] args)
        {
            OptionsProvider.Parse(args,
                DownloadConductor.Start,
                Logger.Error,
                Logger.Error);

            Console.ReadLine();
        }
    }
}