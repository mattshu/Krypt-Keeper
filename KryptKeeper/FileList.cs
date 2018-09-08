using System.Collections.Generic;
using System.ComponentModel;

namespace KryptKeeper
{
    internal class FileList
    {
        public List<FileData> GetList() => _fileList;
        public BindingList<FileData> GetBindingList() => _bindingList;
        public int Count => _fileList?.Count ?? 0;
        private List<FileData> _fileList;
        private BindingList<FileData> _bindingList;

        public FileList(List<FileData> fileList)
        {
            _fileList = fileList;
            _bindingList = new BindingList<FileData>(_fileList);
            _bindingList.AllowRemove = true;
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

        public void Reset()
        {
            _fileList = new List<FileData>();
        }
    }
}
