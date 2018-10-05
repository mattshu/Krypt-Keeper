using System;

namespace KryptKeeper
{
    public partial class ConfirmStopWhileBusy : MetroFramework.Forms.MetroForm
    {
        public ConfirmStopWhileBusy()
        {
            InitializeComponent();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}