using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Sgml;

namespace SimpleSiteCrawler.Lib
{
    internal static class SitePageTemplateExtractor
    {
        private const string DocType = "XHTML";
        private const string AnchorXpath = @"//a";
        private const string HrefAttribute = "href";

        public static List<SitePage> FindAll(Uri startUri, string pageSource, IEnumerable<ISitePageFilter> filters)
        {
            var nodeIterator = CreateAnchorIterator(pageSource);
            var extracted = Extract(startUri, nodeIterator);
            var collected = Filter(extracted, filters)
                .ToList();

            return collected;
        }

        private static XPathNodeIterator CreateAnchorIterator(string pageSource)
        {
            return CreateXmlDocument(pageSource)
                .CreateNavigator()
                .Select(AnchorXpath);
        }

        private static XmlDocument CreateXmlDocument(string html)
        {
            using (var reader = new StringReader(html))
            {
                var sgmlReader = CreateSgmlReader(reader);

                return CreateAndLoadXml(sgmlReader);
            }
        }

        private static XmlDocument CreateAndLoadXml(XmlReader sgmlReader)
        {
            var doc = new XmlDocument
            {
                PreserveWhitespace = true,
                XmlResolver = null
            };

            doc.Load(sgmlReader);
            return doc;
        }

        private static SgmlReader CreateSgmlReader(TextReader reader)
        {
            return new SgmlReader
            {
                DocType = DocType,
                WhitespaceHandling = WhitespaceHandling.All,
                CaseFolding = CaseFolding.ToLower,
                InputStream = reader
            };
        }

        // TODO: Refactor this
        private static IEnumerable<SitePage> Extract(Uri baseUri, XPathNodeIterator allNodes)
        {
            var list = new List<SitePage>();
            while (allNodes.MoveNext())
            {
                var current = allNodes.Current;
                var href = current?.GetAttribute(HrefAttribute, string.Empty);
                
                if (string.IsNullOrEmpty(href)) continue;
                
                if (!HttpUtils.TryParseUri(href, out var uri))
                    continue;

                if (!IsUriSchemaAllowed(uri))
                    continue;

                if (uri.Scheme == Uri.UriSchemeFile)
                {
                    uri = new Uri(baseUri, href);
                    if (!string.IsNullOrEmpty(uri.Fragment))
                    {
                        uri = new Uri(baseUri, uri.AbsolutePath);
                    }
                }

                list.Add(new SitePage {Uri = uri});
            }

            return list.ToArray();
        }

        private static bool IsUriSchemaAllowed(Uri uri)
        {
            return uri.Scheme == Uri.UriSchemeFile || uri.Scheme.Contains(Uri.UriSchemeHttp);
        }

        private static IEnumerable<SitePage> Filter(IEnumerable<SitePage> uris, IEnumerable<ISitePageFilter> allFilters)
        {
            return allFilters.Aggregate(uris, (current, filter) => filter.Apply(current));
        }
    }
}