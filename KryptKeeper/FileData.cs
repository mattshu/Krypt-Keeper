using System;
using System.IO;

namespace KryptKeeper
{
    public class FileData
    {
        public FileData(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            Name = fileInfo.Name;
            Extension = fileInfo.Extension;
            Size = Helper.BytesToString(fileInfo.Length);
            Path = fileInfo.DirectoryName;
        }

        public string Extension { get; }
        public string Name { get; }
        public string Path { get; }
        public string Size { get; }

        public string GetFilePath()
        {
            return Path + (Path[Path.Length - 1] == '\\' ? "" : "\\") + Name;
        }

    }
}