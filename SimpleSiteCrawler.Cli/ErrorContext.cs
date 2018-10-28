using System;
using System.Text;

namespace SimpleSiteCrawler.Cli
{
    internal class ErrorContext
    {
        private readonly Action<string> _handler;
        private readonly StringBuilder _buffer;

        public ErrorContext(Action<string> handler)
        {
            _handler = handler;
            _buffer = new StringBuilder();
        }

        public void Rise() => _handler?.Invoke(_buffer.ToString());

        public ErrorContext Add(string msg)
        {
            _buffer.AppendLine(msg);
            return this;
        }
    }
}