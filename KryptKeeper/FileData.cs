using System.IO;

namespace KryptKeeper
{
    public class FileData
    {
        public string Name { get; }
        public string Size { get; }
        public string Path { get; }
        public string GetFilePath() => Path + (Path[Path.Length - 1] == '\\' ? "" : "\\") + Name;
        public string GetExtension() => new FileInfo(GetFilePath()).Extension;

        public FileData(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var fileInfo = new FileInfo(filePath);
            Name = fileInfo.Name;
            Size = fileInfo.Length.BytesToSizeString(limitToKB: true);
            Path = fileInfo.DirectoryName;
        }
    }
}
