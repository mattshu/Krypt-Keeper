using System.Collections.Generic;

namespace KryptKeeper
{
    public class FileListComparer : IComparer<FileData>
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
            var bytesA = Utils.GetSizeFromString(sizeA);
            var bytesB = Utils.GetSizeFromString(sizeB);
            return bytesA.CompareTo(bytesB);
        }
    }
}