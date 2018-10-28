namespace SimpleSiteCrawler.Cli
{
    internal class Options
    {
        [UrlWithHelp("https://codyhouse.co/")] public string Site { get; set; }

        [FolderName] public string DownloadsFolderName { get; set; }
    }
}