using System;

namespace KryptKeeper
{
    public partial class ConfirmExitWhileBusy : MetroFramework.Forms.MetroForm
    {
        public ConfirmExitWhileBusy()
        {
            InitializeComponent();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        { 
            Close(); // Abort and Exit
        }

        private void btnStop_Click(object sender, EventArgs e)
        { 
            Close(); // Finish File and Exit
        }

        private void btnFinish_Click(object sender, EventArgs e)
        { 
            Close(); // Finish in Background
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
