using System;
using System.IO;

namespace KryptKeeper
{
    public class FileData
    {
        public string Name { get; }
        public string Extension { get; }
        public string Size { get; }
        public string Path { get; }

        public FileData(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            Name = fileInfo.Name;
            Extension = fileInfo.Extension;
            Size = bytesToString(fileInfo.Length);
            Path = fileInfo.DirectoryName;
        }

        public string GetFilePath()
        {
            return Path + "\\" + Name;
        }

        private static string bytesToString(long byteCount)
        {
            string[] suf = {"B", "KB", "MB", "GB", "TB", "PB", "EB"}; //Longs run out around EB
            if (byteCount == 0)
                return "0";
            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            if (place < 1) return "1 KB";
            var num = Math.Round(bytes / Math.Pow(1024, place));
            return Math.Sign(byteCount) * num + " " + suf[place];
        }
    }
}