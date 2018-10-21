using System.IO;
using System.Text.RegularExpressions;
using SimpleSiteCrawler.Lib;

namespace SimpleSiteCrawler.Cli
{
    internal static class SaveHelper
    {
        private const string DefaultPageName = "index";
        private const string DefaultPageExt = ".htm";
        

        public static void SaveResult(string downloadToFolder, SitePage page)
        {
            EnsureDirectoryExists(downloadToFolder);

            var path = MakePath(page.Uri.AbsolutePath == "/" ? DefaultPageName : page.Uri.AbsolutePath) + DefaultPageExt;
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

        public static string MakePath(string absoluteUriPath)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));

            return r.Replace(absoluteUriPath, "_").TrimStart('_');
        }

        private static void EnsureDirectoryExists(string downloadToFolder)
        {
            if (!Directory.Exists(downloadToFolder))
            {
                Directory.CreateDirectory(downloadToFolder);
            }
        }
    }
}