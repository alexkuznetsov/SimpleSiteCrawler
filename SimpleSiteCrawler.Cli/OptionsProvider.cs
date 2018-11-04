using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
            var state = GetValidationState(options);

            if (state.Item1) return state.Item1;
            
            var buffer = new StringBuilder();

            foreach (var error in state.Item2)
            {
                buffer.AppendLine(error.ToString());
            }

            onParseOptionError?.Invoke(buffer.ToString());

            return state.Item1;
        }

        private static (bool, IEnumerable<ValidationResult>) GetValidationState(Options options)
        {
            var validationContext = new ValidationContext(options);
            var errors = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(options, validationContext, errors, true);

            return (isValid, errors);
        }
    }
}