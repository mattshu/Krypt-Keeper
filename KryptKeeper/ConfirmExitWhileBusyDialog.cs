using System;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class ConfirmExitWhileBusyDialog : Form
    {
        public ConfirmExitWhileBusyDialog()
        {
            InitializeComponent();
        }

        private void btnStopExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFinishExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}