using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class ConfirmSettingsDialog : Form
    {

        public bool ShowAgain { get; private set; } = true;

        public ConfirmSettingsDialog()
        {
            InitializeComponent();
        }

        private void ChkDoNotAskAgain_CheckedChanged(object sender, EventArgs e)
        {
            ShowAgain = !ChkDoNotAskAgain.Checked;
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
