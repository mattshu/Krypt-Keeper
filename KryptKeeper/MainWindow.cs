﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        
        private static DataGridViewColumn[] GenerateColumnHeaders()
        {
            var columns = Properties.Settings.Default.fileListColumns;
            var columnWidths = Properties.Settings.Default.fileListColumnWidths;
            var length = columns.Count;
            if (length <= 0 || columns.Count != columnWidths.Count)
                return new DataGridViewColumn[0];

            var headers = new DataGridViewColumn[length];
            for (int i = 0; i < headers.Length; i++)
            {
                var header = new DataGridViewTextBoxColumn
                {
                    Name = columns[i],
                    HeaderText = columns[i],
                    Width = int.Parse(columnWidths[i])
                };
                headers[i] = header;
            }
            return headers;
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            var settings = Properties.Settings.Default;

            var algorithms = settings.algorithms;

            foreach (var a in algorithms)
            {
                cbxEncryptAlgorithms.Items.Add(a);
                cbxDecryptAlgorithms.Items.Add(a);
            }

            if (!settings.rememberSettings) return;

            chkRememberSettings.Checked = true;

            cbxEncryptAlgorithms.SelectedIndex = settings.encryptionAlgorithm;
            chkMaskInformation.Checked = settings.encryptionMaskInformation;
            cbxMaskInformation.SelectedIndex = settings.encryptionMaskInfoType;
            chkRemoveAfterEncrypt.Checked = settings.encryptionRemoveAfterEncrypt;
            cbxEncryptionKeyType.SelectedIndex = settings.encryptionKeyType;
            txtEncryptionKey.Text = settings.encryptionKey;

            if (settings.useEncryptionSettings)
                CopyEncryptionSettings();

            else
            { 
                chkUseEncryptSettings.Checked = false;
                cbxDecryptionKeyType.Enabled = true;
                cbxDecryptAlgorithms.Enabled = true;
                txtDecryptionKey.ReadOnly = false;

                cbxDecryptAlgorithms.SelectedIndex = settings.decryptionAlgorithm;
                cbxDecryptionKeyType.SelectedIndex = settings.decryptionKeyType;
                txtDecryptionKey.Text = settings.decryptionKey;
            }

            chkRemoveAfterEncrypt.Checked = settings.encryptionRemoveAfterEncrypt;
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            AddFilesToList();
        }

        private void AddFilesToList()
        {
            var openFileDialog = new OpenFileDialog { Multiselect = true };
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK || openFileDialog.FileNames.Length <= 0) return;
            foreach (var path in openFileDialog.FileNames)
            {
            }
            //listFiles.DataSource =
        }

        private void btnRemoveFiles_Click(object sender, EventArgs e)
        {

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {

        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {

        }

        private void cbxEncryptAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkUseEncryptSettings.Checked)
                cbxDecryptAlgorithms.SelectedIndex = cbxEncryptAlgorithms.SelectedIndex;
        }

        private void chkMaskInformation_CheckedChanged(object sender, EventArgs e)
        {
            cbxMaskInformation.Enabled = chkMaskInformation.Checked;
        }

        private void chkUseEncryptSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseEncryptSettings.Checked) CopyEncryptionSettings();

            cbxDecryptAlgorithms.Enabled = !chkUseEncryptSettings.Checked;
            cbxDecryptionKeyType.Enabled = !chkUseEncryptSettings.Checked;
            txtDecryptionKey.ReadOnly = chkUseEncryptSettings.Checked;
        }

        private void CopyEncryptionSettings()
        {
            cbxDecryptAlgorithms.SelectedIndex = cbxEncryptAlgorithms.SelectedIndex;
            cbxDecryptionKeyType.SelectedIndex = cbxEncryptionKeyType.SelectedIndex;
            txtDecryptionKey.Text = txtEncryptionKey.Text;
            btnBrowseDecrypt.Enabled = btnBrowseEncrypt.Enabled;
        }

        private void cbxEncryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFormBasedOnKeyType(cbxEncryptionKeyType, btnBrowseEncrypt, txtEncryptionKey);
            if (chkUseEncryptSettings.Checked)
            {
                cbxDecryptionKeyType.SelectedIndex = cbxEncryptionKeyType.SelectedIndex;
            }
        }

        private void txtEncryptionKey_TextChanged(object sender, EventArgs e)
        {
            if (chkUseEncryptSettings.Checked)
                txtDecryptionKey.Text = txtEncryptionKey.Text;
        }

        private void cbxDecryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFormBasedOnKeyType(cbxDecryptionKeyType, btnBrowseDecrypt, txtDecryptionKey);
        }

        private static void UpdateFormBasedOnKeyType(ComboBox cbxKeyType, Button btnBrowse, TextBox txtKey)
        {
            btnBrowse.Enabled = cbxKeyType.SelectedIndex == 1; // Key file
            if (cbxKeyType.SelectedIndex == 1) txtKey.Text = "";
        }

        private void btnBrowseEncrypt_Click(object sender, EventArgs e)
        {
            txtEncryptionKey.Text = BrowseForKeyFile();
        }

        private void btnBrowseDecrypt_Click(object sender, EventArgs e)
        {
            txtDecryptionKey.Text = BrowseForKeyFile();
        }

        private static string BrowseForKeyFile()
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() != DialogResult.OK || !openFile.CheckFileExists) return "";
            return openFile.FileName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFileListColumnWidths();

            if (chkRememberSettings.Checked)
            {
                SaveSettings();
                return;
            }

            var result = MessageBox.Show(@"Do you want to save your settings?", @"Save Settings?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                SaveSettings();
            else if (result == DialogResult.Cancel)
                e.Cancel = true;
            else
                ResetSettings();
        }

        private void SaveFileListColumnWidths()
        {
            var enumer = listFiles.Columns.GetEnumerator();
            var widths = new StringCollection();
            while (enumer.MoveNext())
            {
                var header = (DataGridViewTextBoxColumn)enumer.Current;
                if (header == null) break;
                widths.Add(header.Width.ToString());
            }
            if (widths.Count <= 0) return;
            Properties.Settings.Default.fileListColumnWidths = widths;
            Properties.Settings.Default.Save();
        }

        private void SaveSettings()
        {
            var settings = Properties.Settings.Default;

            settings.encryptionAlgorithm = cbxEncryptAlgorithms.SelectedIndex;
            settings.encryptionKey = txtEncryptionKey.Text;
            settings.encryptionKeyType = cbxEncryptionKeyType.SelectedIndex;

            settings.encryptionMaskInformation = chkMaskInformation.Checked;
            settings.encryptionMaskInfoType = cbxMaskInformation.SelectedIndex;

            settings.useEncryptionSettings = chkUseEncryptSettings.Checked;

            settings.decryptionAlgorithm = cbxDecryptAlgorithms.SelectedIndex;
            settings.decryptionKey = txtDecryptionKey.Text;
            settings.decryptionKeyType = cbxDecryptionKeyType.SelectedIndex;

            settings.rememberSettings = chkRememberSettings.Checked;

            settings.Save();
        }

        private static void ResetSettings()
        {
            var settings = Properties.Settings.Default;

            settings.encryptionAlgorithm = settings.decryptionAlgorithm = -1;
            settings.encryptionKeyType = settings.decryptionKeyType = -1;
            settings.encryptionKey = settings.decryptionKey = "";
            settings.encryptionMaskInfoType = -1;
            settings.encryptionMaskInformation = false;
            settings.encryptionRemoveAfterEncrypt = true;
            settings.rememberSettings = false;
            settings.useEncryptionSettings = true;

            settings.Save();
        }
    }
}
