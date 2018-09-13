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
            Size = Utils.BytesToFileSize(fileInfo.Length);
            Path = fileInfo.DirectoryName;
        }

        public string Name { get; }
        public string Extension { get; }
        public string Size { get; }
        public string Path { get; }

        public string GetFilePath()
        {
            return Path + (Path[Path.Length - 1] == '\\' ? "" : "\\") + Name;
        }
    }
}