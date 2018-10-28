using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace SimpleSiteCrawler.Cli
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field, AllowMultiple = false)]
    public sealed class FolderNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string directoryName)
                return IsValidDirectoryName(directoryName);

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Please, provide valid folder name!\r\nValid names: downloads";
        }

        private static bool IsValidDirectoryName(string directoryName) =>
            !string.IsNullOrEmpty(directoryName) &&
            !Path.GetInvalidPathChars().Any(directoryName.Contains);
    }
}