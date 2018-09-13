using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace KryptKeeper
{
    internal class FileList
    {
        public List<FileData> GetList() => _fileList;
        public int Count => _fileList?.Count ?? 0;
        private List<FileData> _fileList;
        private readonly DataGridView _dataGridView;

        public FileList(List<FileData> fileList, DataGridView dataGridView)
        {
            _fileList = fileList;
            _dataGridView = dataGridView;
            UpdateDataSource();
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
            UpdateDataSource();
        }

        public void UpdateDataSource()
        {
            _dataGridView.DataSource = null;
            _dataGridView.DataSource = _fileList.ToList();
        }
    }
}
