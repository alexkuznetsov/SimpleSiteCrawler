using System;
using System.Collections.Concurrent;

namespace SimpleSiteCrawler.Lib
{
    public class SitePage
    {
        public Uri Uri { get; set; }

        /// <summary>
        /// Media resources - css, js, images, etc...
        /// </summary>
        public ConcurrentDictionary<Uri, string> MediaUrl { get; } = new ConcurrentDictionary<Uri, string>();

        public string Contents { get; set; }

        public override string ToString()
        {
            return Uri.ToString();
        }
    }
}