using System.Collections.Generic;
using System.IO;

namespace TestCraftParserLib
{
    public class FolderParser : IFolderParser
    {
        public IEnumerable<FileInfo> Files(string pattern)
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            return dir.GetFiles(pattern);
        }
    }
}