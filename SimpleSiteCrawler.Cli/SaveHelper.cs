using System.IO;
using System.Text.RegularExpressions;
using SimpleSiteCrawler.Lib;

namespace SimpleSiteCrawler.Cli
{
    internal static class SaveHelper
    {
        private const string DefaultPageName = "index";
        private const string DefaultPageExt = ".htm";

        private static Regex _invalidCharsRegex;
        private static readonly object LockObj = new object();

        private static Regex InvalidCharsRegex
        {
            get
            {
                if (_invalidCharsRegex == null)
                {
                    lock (LockObj)
                    {
                        if (_invalidCharsRegex == null)
                        {
                            var regexSearch = new string(Path.GetInvalidFileNameChars()) +
                                              new string(Path.GetInvalidPathChars());
                            _invalidCharsRegex = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                        }
                    }
                }

                return _invalidCharsRegex;
            }
        }

        public static void SaveResult(Options options, SitePage page)
        {
            var downloadToFolder = GetDownloadFolderPath(options);

            EnsureDirectoryExists(downloadToFolder);

            var path = MakePath(page.Uri.AbsolutePath == "/" ? DefaultPageName : page.Uri.AbsolutePath) +
                       DefaultPageExt;
            var fileAbsPath = Path.Combine(downloadToFolder, path);

            if (File.Exists(fileAbsPath))
                return;

            using (var file = File.OpenWrite(fileAbsPath))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(page.Contents);
                    writer.Flush();
                }
            }
        }

        private static string GetCurrentFolder() => Path.GetDirectoryName(typeof(Program).Assembly.Location);

        private static string GetDownloadFolderPath(Options options) =>
            Path.Combine(GetCurrentFolder(), options.DownloadsFolderName, MakePath(options.Site));

        private static string MakePath(string absoluteUriPath) =>
            InvalidCharsRegex.Replace(absoluteUriPath, "_").TrimStart('_');

        private static void EnsureDirectoryExists(string downloadToFolder)
        {
            if (!Directory.Exists(downloadToFolder)) Directory.CreateDirectory(downloadToFolder);
        }
    }
}