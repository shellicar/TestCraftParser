using System;

namespace TestCraftParserLib
{
    public interface IParserWriter : IDisposable
    {
        void WriteLine(string text);
    }
}