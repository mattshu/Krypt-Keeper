using System.Collections.Generic;

namespace KryptKeeper
{
    internal class FileList
    {
        public List<FileData> GetList() => _fileList;
        public int Count => _fileList?.Count ?? 0;
        private readonly List<FileData> _fileList;

        public FileList(List<FileData> fileList)
        {
            _fileList = fileList;
        }

        public void Add(FileData file)
        {
            _fileList.Add(file);
        }

        public void Clear()
        {
            _fileList.Clear();
        }

        public void RemoveAt(int i)
        {
            _fileList.RemoveAt(i);
        }

        public void Sort(FileListComparer fileComparer)
        {
            _fileList.Sort(fileComparer);
        }
    }
}
