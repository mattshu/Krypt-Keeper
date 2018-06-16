using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace KryptKeeper
{
    public partial class MainWindow : Form
    {

        public MainWindow()
        {
            InitializeComponent();
            AddFileListColumns();
        }

        private void AddFileListColumns()
        {
            var headers = GenerateColumnHeaders();
            listFiles.Columns.AddRange(headers);
        }

        private static ColumnHeader[] GenerateColumnHeaders()
        {
            var columns = Properties.Settings.Default.fileListColumns;
            var columnWidths = Properties.Settings.Default.fileListColumnWidths;
            var length = columns.Count;
            if (length <= 0 || columns.Count != columnWidths.Count)
                return new ColumnHeader[0];

            var headers = new ColumnHeader[length];
            for (int i = 0; i < headers.Length; i++)
            {
                var header = new ColumnHeader
                {
                    Text = columns[i],
                    Width = int.Parse(columnWidths[i])
                };
                headers[i] = header;
            }
            return headers;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {

        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {

        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {

        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveFileListColumnWidths();
        }

        private void SaveFileListColumnWidths()
        {
            var enumer = listFiles.Columns.GetEnumerator();
            var widths = new StringCollection();
            while (enumer.MoveNext())
            {
                var header = (ColumnHeader)enumer.Current;
                if (header == null) break;
                widths.Add(header.Width.ToString());
            }
            if (widths.Count <= 0) return;
            Properties.Settings.Default.fileListColumnWidths = widths;
            Properties.Settings.Default.Save();
        }
    }
}
