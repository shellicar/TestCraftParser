using System.IO;

namespace TestCraftParserLib
{
    public class ParserWriter : IParserWriter
    {
        private readonly TextWriter _writer;

        public ParserWriter(string filename)
        {
            _writer = File.CreateText(filename);
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }

        public void WriteLine(string text)
        {
            _writer.WriteLine(text);
        }
    }
}