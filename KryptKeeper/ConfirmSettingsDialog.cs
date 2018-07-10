using System;
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

        private void chkDoNotAskAgain_CheckedChanged(object sender, EventArgs e)
        {
            ShowAgain = !chkDoNotAskAgain.Checked;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}