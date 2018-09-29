using System;

namespace KryptKeeper
{
    public partial class MainWindow
    {
        #region Option Tab Form Events
        private void chkMaskFileInformation_CheckedChanged(object sender, EventArgs e)
        {
            chkMaskFileDate.Enabled = chkMaskFileName.Enabled = chkMaskFileInformation.Checked;
            if (chkMaskFileInformation.Checked)
                chkMaskFileName.Checked = true;
            else
                chkMaskFileName.Checked = chkMaskFileDate.Checked = false;
        }

        private void radKeyFile_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowseKeyFile.Enabled = radKeyFile.Checked;
            txtCipherKey.Clear();
            txtCipherKey.ReadOnly = radKeyFile.Checked;
            txtCipherKey.ShowButton = radPlaintextKey.Checked;
            txtCipherKey.UseSystemPasswordChar = radPlaintextKey.Checked;
            if (radKeyFile.Checked)
            {
                txtCipherKey.Text = Utils.BrowseFiles(multiSelect: false);
                txtCipherKey.WaterMark = @"You must browse for a key file...";
            }
            else
                txtCipherKey.WaterMark = "";
        }

        private void txtCipherKey_ButtonClick(object sender, EventArgs e)
        {
            txtCipherKey.UseSystemPasswordChar = !txtCipherKey.UseSystemPasswordChar;
        }

        private void txtCipherKey_Click(object sender, EventArgs e)
        {
            if (radKeyFile.Checked && string.IsNullOrWhiteSpace(txtCipherKey.Text))
                txtCipherKey.Text = Utils.BrowseFiles(@"Select a key file", multiSelect: false);
        }

        private void btnBrowseKeyFile_Click(object sender, EventArgs e)
        {
            txtCipherKey.Text = Utils.BrowseFiles(multiSelect: false);
        }

        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            buildFileList();
        }
        #endregion
    }
}
