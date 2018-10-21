using System;

namespace SimpleSiteCrawler.Cli
{
    internal interface ILogger
    {
        void Log(string message);
        void Info(string message);
        void Error(string message);
        void Error(Exception exception);
    }
}