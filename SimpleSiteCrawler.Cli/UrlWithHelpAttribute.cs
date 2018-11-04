using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;

namespace SimpleSiteCrawler.Cli
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field)]
    public sealed class UrlWithHelpAttribute : DataTypeAttribute
    {
        private readonly string _helpUri;

        public UrlWithHelpAttribute(string helpUri) : base(DataType.Url)
        {
            _helpUri = helpUri;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            string uriString;
            if ((uriString = value as string) == null)
                return false;

            if (!Uri.TryCreate(uriString, UriKind.Absolute, out var u))
                return false;

            return u.Scheme == Uri.UriSchemeHttp
                   || u.Scheme == Uri.UriSchemeFile
                   || u.Scheme == Uri.UriSchemeHttps;
        }

        public override string FormatErrorMessage(string name)
        {
            var fileName = Path.GetFileName(Assembly.GetEntryAssembly().CodeBase);
            return $"Please, provide site to download!\r\nExample: dotnet {fileName} {_helpUri}";
        }
    }
}