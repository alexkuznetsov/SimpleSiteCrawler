using System;
using System.IO;
using System.Text;

namespace SimpleSiteCrawler.Cli
{
    internal static class LoggerFactory
    {
        public static ILogger CreateLogger()
        {
            return new Logger();
        }

        private class Logger : ILogger
        {
            private readonly TextWriter _writer = Console.Out;

            private static StringBuilder BuildMessage(string level, string stringMessage = null,
                Exception exception = null)
            {
                var builder = new StringBuilder();
                builder.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t");
                builder.Append(level + '\t');

                var hasStringMessage = !string.IsNullOrEmpty(stringMessage);

                if (hasStringMessage)
                {
                    builder.Append(stringMessage);
                }

                if (exception == null) return builder;
                if (hasStringMessage)
                {
                    builder.AppendLine();
                    builder.AppendLine("Exception:");
                }

                builder.Append(exception);

                return builder;
            }

            public void Log(string message)
            {
                var msg = BuildMessage("LOG", stringMessage: message);
                _writer.WriteLine(msg.ToString());
                _writer.Flush();
            }

            public void Info(string message)
            {
                var msg = BuildMessage("INFO", stringMessage: message);
                _writer.WriteLine(msg.ToString());
                _writer.Flush();
            }

            public void Error(string message)
            {
                var msg = BuildMessage("ERROR", stringMessage: message);
                _writer.WriteLine(msg.ToString());
                _writer.Flush();
            }

            public void Error(Exception exception)
            {
                var msg = BuildMessage("ERROR", exception: exception);
                _writer.WriteLine(msg.ToString());
                _writer.Flush();
            }
        }
    }
}