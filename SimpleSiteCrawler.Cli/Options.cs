namespace SimpleSiteCrawler.Cli
{
    internal class Options
    {
        private const string HelpUri = "https://codyhouse.co/";
        
        [UrlWithHelp(HelpUri)] public string Site { get; set; }

        [FolderName] public string DownloadsFolderName { get; set; }
    }
}