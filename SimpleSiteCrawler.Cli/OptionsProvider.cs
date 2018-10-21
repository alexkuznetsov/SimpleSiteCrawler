using System;
using System.Text;

namespace SimpleSiteCrawler.Cli
{
    internal static class OptionsProvider
    {
        private const string DefaultDownloadsFolderName = "download";

        public static void Parse(string[] args
            , Action<Options> onSuccess
            , Action<string> onParseOptionError
            , Action<Exception> onError)
        {
            var argsNotNull = args != null;
            var options = new Options
            {
                Site = argsNotNull && args.Length >= 1 ? args[0]?.Trim() : null,
                DownloadsFolderName = argsNotNull && args.Length >= 2 ? args[1]?.Trim() : DefaultDownloadsFolderName
            };

            if (string.IsNullOrEmpty(options.Site))
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Please, provide site to download!");
                stringBuilder.AppendLine("Example: dotnet SimpleSiteCrawler.Cli.dll https://codyhouse.co/");
                onParseOptionError?.Invoke(stringBuilder.ToString());
            }
            else if (string.IsNullOrEmpty(options.DownloadsFolderName))
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Please, provide valid folder name!");
                stringBuilder.AppendLine("Example: dotnet SimpleSiteCrawler.Cli.dll https://codyhouse.co/ downloads");
                onParseOptionError?.Invoke(stringBuilder.ToString());
            }
            else
            {
                try
                {
                    onSuccess?.Invoke(options);
                }
                catch (Exception e)
                {
                    onError?.Invoke(e);
                }
            }
        }
    }
}