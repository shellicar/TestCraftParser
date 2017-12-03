using System.Collections.Generic;
using System.IO;

namespace TestCraftParserLib
{
    public interface IFolderParser
    {
        IEnumerable<FileInfo> Files(string pattern);
    }
}