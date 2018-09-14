using System.Collections.Generic;

namespace KryptKeeper
{
    internal class FileListComparer : IComparer<FileData>
    {
        private readonly bool _descending;
        private readonly Cipher.ProcessOrder _processOrder;

        public FileListComparer(Cipher.ProcessOrder processOrder, bool descending)
        {
            _processOrder = processOrder;
            _descending = descending;
        }

        public int Compare(FileData fileA, FileData fileB)
        {
            if (fileA == null && fileB == null)
                return 0;
            if (fileA == null)
                return -1;
            if (fileB == null)
                return 1;
            if (fileA == fileB)
                return 0;
            int value;
            switch (_processOrder)
            {
                case Cipher.ProcessOrder.FileSize:
                    value = compareSizeFromString(fileA.Size, fileB.Size);
                    break;
                case Cipher.ProcessOrder.FileName:
                    value = string.CompareOrdinal(fileA.Name, fileB.Name);
                    break;
                default:
                    return 0;
            }
            if (_descending) value *= -1;
            return value;
        }

        private static int compareSizeFromString(string sizeA, string sizeB)
        {
            var bytesA = getSizeFromString(sizeA);
            var bytesB = getSizeFromString(sizeB);
            return bytesA.CompareTo(bytesB);
        }

        private static long getSizeFromString(string sizeString)
        {
            var split = sizeString.Split();
            if (split.Length < 1) return -1;
            var size = long.Parse(split[0].Replace(",",""));
            var sizeSuffix = split[1];
            if (sizeSuffix == "KB")
                return size * 1024;
            return size;
        }
    }
}