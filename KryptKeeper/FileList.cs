using System.Collections.Generic;

namespace KryptKeeper
{
    internal class FileList
    {
        public List<FileData> GetList() => _fileList;
        public int Count => _fileList.Count;
        private readonly List<FileData> _fileList;
        public FileList()
        {
            _fileList = new List<FileData>();
        }
        public FileList(List<FileData> fileList)
        {
            _fileList = fileList;
        }
        public void Sort(FileListComparer fileComparer)
        {
            _fileList.Sort(fileComparer);
        }
        public void RemoveAt(int i)
        {
            _fileList.RemoveAt(i);
        }
    }
}
