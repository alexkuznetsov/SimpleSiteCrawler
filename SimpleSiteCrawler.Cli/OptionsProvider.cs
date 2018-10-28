using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
            var options = new Options
            {
                Site = args.GetValueSafe(0),
                DownloadsFolderName = args.GetValueSafe(1) ??
                                      DefaultDownloadsFolderName
            };

            if (!IsOptionsValid(options, onParseOptionError)) return;

            try
            {
                onSuccess?.Invoke(options);
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }
        }

        private static bool IsOptionsValid(Options options, Action<string> onParseOptionError)
        {
            var validationContext = new ValidationContext(options);
            var errors = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(options, validationContext, errors, true);

            if (isValid) return true;

            var errorHandler = CreateError(onParseOptionError);

            foreach (var error in errors)
            {
                errorHandler.Add(error.ToString());
            }

            errorHandler.Rise();
            return false;
        }

        private static ErrorContext CreateError(Action<string> handler) => new ErrorContext(handler);
    }
}