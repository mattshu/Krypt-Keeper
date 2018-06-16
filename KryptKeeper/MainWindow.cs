using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private static ColumnHeader[] GenerateHeader(string[] names)
        {
            var headers = new ColumnHeader[names.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                var header = new ColumnHeader {Text = names[i]};
                headers[i] = header;
            }
            return headers;
        }
        public MainWindow()
        {
            InitializeComponent();
            AddFileListHeaders("Name", "Extension", "Size", "Path", "MD5");
        }

        private void AddFileListHeaders(params string[] names)
        {
            var headers = GenerateHeader(names);
            listFiles.Columns.AddRange(headers);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {

        }
    }
}
