using System;

namespace SimpleSiteCrawler.Lib
{
    public class SitePage
    {
        public Uri Uri { get; set; }

        public string Contents { get; set; }

        public override string ToString()
        {
            return Uri.ToString();
        }
    }
}