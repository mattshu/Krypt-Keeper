﻿namespace KryptKeeper
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.FileListGridView = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtnBrowseDecrypt = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.ChkRememberSettings = new System.Windows.Forms.CheckBox();
            this.ChkUseEncryptSettings = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CbxDecryptAlgorithms = new System.Windows.Forms.ComboBox();
            this.TxtDecryptionKey = new System.Windows.Forms.TextBox();
            this.CbxDecryptionKeyType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnBrowseEncrypt = new System.Windows.Forms.Button();
            this.CbxEncryptAlgorithms = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtEncryptionKey = new System.Windows.Forms.TextBox();
            this.ChkRemoveAfterEncrypt = new System.Windows.Forms.CheckBox();
            this.CbxEncryptionKeyType = new System.Windows.Forms.ComboBox();
            this.CbxMaskInformation = new System.Windows.Forms.ComboBox();
            this.ChkMaskInformation = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.TxtStatus = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.BtnExport = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnEncrypt = new System.Windows.Forms.Button();
            this.BtnDecrypt = new System.Windows.Forms.Button();
            this.BtnAddFiles = new System.Windows.Forms.Button();
            this.BtnRemoveFiles = new System.Windows.Forms.Button();
            this.BtnEncryptSelected = new System.Windows.Forms.Button();
            this.BtnDecryptSelected = new System.Windows.Forms.Button();
            this.TabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FileListGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabMain
            // 
            this.TabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabMain.Controls.Add(this.tabPage1);
            this.TabMain.Controls.Add(this.tabPage2);
            this.TabMain.Controls.Add(this.tabPage3);
            this.TabMain.Location = new System.Drawing.Point(12, 81);
            this.TabMain.Name = "TabMain";
            this.TabMain.SelectedIndex = 0;
            this.TabMain.Size = new System.Drawing.Size(519, 275);
            this.TabMain.TabIndex = 1;
            this.TabMain.TabIndexChanged += new System.EventHandler(this.TabMain_TabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.FileListGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(511, 249);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // FileListGridView
            // 
            this.FileListGridView.AllowUserToAddRows = false;
            this.FileListGridView.AllowUserToDeleteRows = false;
            this.FileListGridView.AllowUserToOrderColumns = true;
            this.FileListGridView.AllowUserToResizeRows = false;
            this.FileListGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileListGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FileListGridView.Location = new System.Drawing.Point(3, 3);
            this.FileListGridView.Name = "FileListGridView";
            this.FileListGridView.ReadOnly = true;
            this.FileListGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FileListGridView.Size = new System.Drawing.Size(505, 243);
            this.FileListGridView.TabIndex = 0;
            this.FileListGridView.DataSourceChanged += new System.EventHandler(this.FileListGridView_DataSourceChanged);
            this.FileListGridView.SelectionChanged += new System.EventHandler(this.FileList_SelectionChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(511, 249);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BtnBrowseDecrypt);
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Controls.Add(this.ChkRememberSettings);
            this.groupBox2.Controls.Add(this.ChkUseEncryptSettings);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.CbxDecryptAlgorithms);
            this.groupBox2.Controls.Add(this.TxtDecryptionKey);
            this.groupBox2.Controls.Add(this.CbxDecryptionKeyType);
            this.groupBox2.Location = new System.Drawing.Point(3, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(502, 102);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Decryption Options";
            // 
            // BtnBrowseDecrypt
            // 
            this.BtnBrowseDecrypt.Enabled = false;
            this.BtnBrowseDecrypt.Location = new System.Drawing.Point(342, 68);
            this.BtnBrowseDecrypt.Name = "BtnBrowseDecrypt";
            this.BtnBrowseDecrypt.Size = new System.Drawing.Size(25, 21);
            this.BtnBrowseDecrypt.TabIndex = 8;
            this.BtnBrowseDecrypt.Text = "...";
            this.BtnBrowseDecrypt.UseVisualStyleBackColor = true;
            this.BtnBrowseDecrypt.Click += new System.EventHandler(this.BtnBrowseDecrypt_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(392, 59);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(104, 30);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // ChkRememberSettings
            // 
            this.ChkRememberSettings.AutoSize = true;
            this.ChkRememberSettings.Location = new System.Drawing.Point(367, 19);
            this.ChkRememberSettings.Name = "ChkRememberSettings";
            this.ChkRememberSettings.Size = new System.Drawing.Size(132, 17);
            this.ChkRememberSettings.TabIndex = 6;
            this.ChkRememberSettings.Text = "Remember All Settings";
            this.ChkRememberSettings.UseVisualStyleBackColor = true;
            // 
            // ChkUseEncryptSettings
            // 
            this.ChkUseEncryptSettings.AutoSize = true;
            this.ChkUseEncryptSettings.Checked = true;
            this.ChkUseEncryptSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkUseEncryptSettings.Location = new System.Drawing.Point(50, 19);
            this.ChkUseEncryptSettings.Name = "ChkUseEncryptSettings";
            this.ChkUseEncryptSettings.Size = new System.Drawing.Size(136, 17);
            this.ChkUseEncryptSettings.TabIndex = 6;
            this.ChkUseEncryptSettings.Text = "Use encryption settings";
            this.ChkUseEncryptSettings.UseVisualStyleBackColor = true;
            this.ChkUseEncryptSettings.CheckedChanged += new System.EventHandler(this.ChkUseEncryptSettings_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Algorithm:";
            // 
            // CbxDecryptAlgorithms
            // 
            this.CbxDecryptAlgorithms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxDecryptAlgorithms.Enabled = false;
            this.CbxDecryptAlgorithms.Location = new System.Drawing.Point(181, 42);
            this.CbxDecryptAlgorithms.Name = "CbxDecryptAlgorithms";
            this.CbxDecryptAlgorithms.Size = new System.Drawing.Size(155, 21);
            this.CbxDecryptAlgorithms.TabIndex = 5;
            // 
            // TxtDecryptionKey
            // 
            this.TxtDecryptionKey.Location = new System.Drawing.Point(181, 69);
            this.TxtDecryptionKey.Name = "TxtDecryptionKey";
            this.TxtDecryptionKey.ReadOnly = true;
            this.TxtDecryptionKey.Size = new System.Drawing.Size(155, 20);
            this.TxtDecryptionKey.TabIndex = 3;
            // 
            // CbxDecryptionKeyType
            // 
            this.CbxDecryptionKeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxDecryptionKeyType.Enabled = false;
            this.CbxDecryptionKeyType.Items.AddRange(new object[] {
            "Plaintext key",
            "Key file"});
            this.CbxDecryptionKeyType.Location = new System.Drawing.Point(50, 68);
            this.CbxDecryptionKeyType.Name = "CbxDecryptionKeyType";
            this.CbxDecryptionKeyType.Size = new System.Drawing.Size(121, 21);
            this.CbxDecryptionKeyType.TabIndex = 1;
            this.CbxDecryptionKeyType.SelectedIndexChanged += new System.EventHandler(this.CbxDecryptionKeyType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnBrowseEncrypt);
            this.groupBox1.Controls.Add(this.CbxEncryptAlgorithms);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TxtEncryptionKey);
            this.groupBox1.Controls.Add(this.ChkRemoveAfterEncrypt);
            this.groupBox1.Controls.Add(this.CbxEncryptionKeyType);
            this.groupBox1.Controls.Add(this.CbxMaskInformation);
            this.groupBox1.Controls.Add(this.ChkMaskInformation);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(502, 129);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Encryption Options";
            // 
            // BtnBrowseEncrypt
            // 
            this.BtnBrowseEncrypt.Enabled = false;
            this.BtnBrowseEncrypt.Location = new System.Drawing.Point(342, 93);
            this.BtnBrowseEncrypt.Name = "BtnBrowseEncrypt";
            this.BtnBrowseEncrypt.Size = new System.Drawing.Size(25, 21);
            this.BtnBrowseEncrypt.TabIndex = 8;
            this.BtnBrowseEncrypt.Text = "...";
            this.BtnBrowseEncrypt.UseVisualStyleBackColor = true;
            this.BtnBrowseEncrypt.Click += new System.EventHandler(this.BtnBrowseEncrypt_Click);
            // 
            // CbxEncryptAlgorithms
            // 
            this.CbxEncryptAlgorithms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxEncryptAlgorithms.Location = new System.Drawing.Point(181, 17);
            this.CbxEncryptAlgorithms.Name = "CbxEncryptAlgorithms";
            this.CbxEncryptAlgorithms.Size = new System.Drawing.Size(155, 21);
            this.CbxEncryptAlgorithms.TabIndex = 5;
            this.CbxEncryptAlgorithms.SelectedIndexChanged += new System.EventHandler(this.CbxEncryptAlgorithms_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(118, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Algorithm:";
            // 
            // TxtEncryptionKey
            // 
            this.TxtEncryptionKey.Location = new System.Drawing.Point(181, 95);
            this.TxtEncryptionKey.Name = "TxtEncryptionKey";
            this.TxtEncryptionKey.Size = new System.Drawing.Size(155, 20);
            this.TxtEncryptionKey.TabIndex = 3;
            this.TxtEncryptionKey.TextChanged += new System.EventHandler(this.TxtEncryptionKey_TextChanged);
            // 
            // ChkRemoveAfterEncrypt
            // 
            this.ChkRemoveAfterEncrypt.AutoSize = true;
            this.ChkRemoveAfterEncrypt.Location = new System.Drawing.Point(50, 72);
            this.ChkRemoveAfterEncrypt.Name = "ChkRemoveAfterEncrypt";
            this.ChkRemoveAfterEncrypt.Size = new System.Drawing.Size(178, 17);
            this.ChkRemoveAfterEncrypt.TabIndex = 2;
            this.ChkRemoveAfterEncrypt.Text = "Remove original after encryption";
            this.ChkRemoveAfterEncrypt.UseVisualStyleBackColor = true;
            // 
            // CbxEncryptionKeyType
            // 
            this.CbxEncryptionKeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxEncryptionKeyType.FormattingEnabled = true;
            this.CbxEncryptionKeyType.Items.AddRange(new object[] {
            "Plaintext key",
            "Key file"});
            this.CbxEncryptionKeyType.Location = new System.Drawing.Point(50, 94);
            this.CbxEncryptionKeyType.Name = "CbxEncryptionKeyType";
            this.CbxEncryptionKeyType.Size = new System.Drawing.Size(121, 21);
            this.CbxEncryptionKeyType.TabIndex = 1;
            this.CbxEncryptionKeyType.SelectedIndexChanged += new System.EventHandler(this.CbxEncryptionKeyType_SelectedIndexChanged);
            // 
            // CbxMaskInformation
            // 
            this.CbxMaskInformation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbxMaskInformation.Enabled = false;
            this.CbxMaskInformation.Items.AddRange(new object[] {
            "File name",
            "File dates",
            "File name AND dates"});
            this.CbxMaskInformation.Location = new System.Drawing.Point(181, 45);
            this.CbxMaskInformation.Name = "CbxMaskInformation";
            this.CbxMaskInformation.Size = new System.Drawing.Size(155, 21);
            this.CbxMaskInformation.TabIndex = 1;
            // 
            // ChkMaskInformation
            // 
            this.ChkMaskInformation.AutoSize = true;
            this.ChkMaskInformation.Location = new System.Drawing.Point(50, 47);
            this.ChkMaskInformation.Name = "ChkMaskInformation";
            this.ChkMaskInformation.Size = new System.Drawing.Size(125, 17);
            this.ChkMaskInformation.TabIndex = 0;
            this.ChkMaskInformation.Text = "Mask file information:";
            this.ChkMaskInformation.UseVisualStyleBackColor = true;
            this.ChkMaskInformation.CheckedChanged += new System.EventHandler(this.ChkMaskInformation_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.TxtStatus);
            this.tabPage3.Controls.Add(this.progressBar);
            this.tabPage3.Controls.Add(this.BtnExport);
            this.tabPage3.Controls.Add(this.BtnClear);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(511, 249);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Status";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // TxtStatus
            // 
            this.TxtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtStatus.Location = new System.Drawing.Point(6, 35);
            this.TxtStatus.Multiline = true;
            this.TxtStatus.Name = "TxtStatus";
            this.TxtStatus.ReadOnly = true;
            this.TxtStatus.Size = new System.Drawing.Size(499, 179);
            this.TxtStatus.TabIndex = 7;
            this.TxtStatus.TextChanged += new System.EventHandler(this.TxtStatus_TextChanged);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 6);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(499, 23);
            this.progressBar.TabIndex = 6;
            // 
            // BtnExport
            // 
            this.BtnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExport.Enabled = false;
            this.BtnExport.Location = new System.Drawing.Point(450, 220);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(56, 23);
            this.BtnExport.TabIndex = 3;
            this.BtnExport.Text = "Export...";
            this.BtnExport.UseVisualStyleBackColor = true;
            // 
            // BtnClear
            // 
            this.BtnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClear.Location = new System.Drawing.Point(388, 220);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(56, 23);
            this.BtnClear.TabIndex = 4;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            // 
            // BtnEncrypt
            // 
            this.BtnEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnEncrypt.Enabled = false;
            this.BtnEncrypt.Location = new System.Drawing.Point(200, 12);
            this.BtnEncrypt.Name = "BtnEncrypt";
            this.BtnEncrypt.Size = new System.Drawing.Size(145, 30);
            this.BtnEncrypt.TabIndex = 2;
            this.BtnEncrypt.Text = "Encrypt All Files";
            this.BtnEncrypt.UseVisualStyleBackColor = true;
            this.BtnEncrypt.Click += new System.EventHandler(this.BtnEncrypt_Click);
            // 
            // BtnDecrypt
            // 
            this.BtnDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDecrypt.Enabled = false;
            this.BtnDecrypt.Location = new System.Drawing.Point(386, 12);
            this.BtnDecrypt.Name = "BtnDecrypt";
            this.BtnDecrypt.Size = new System.Drawing.Size(145, 30);
            this.BtnDecrypt.TabIndex = 2;
            this.BtnDecrypt.Text = "Decrypt All Files";
            this.BtnDecrypt.UseVisualStyleBackColor = true;
            this.BtnDecrypt.Click += new System.EventHandler(this.BtnDecrypt_Click);
            // 
            // BtnAddFiles
            // 
            this.BtnAddFiles.Location = new System.Drawing.Point(12, 12);
            this.BtnAddFiles.Name = "BtnAddFiles";
            this.BtnAddFiles.Size = new System.Drawing.Size(145, 30);
            this.BtnAddFiles.TabIndex = 2;
            this.BtnAddFiles.Text = "Add Files...";
            this.BtnAddFiles.UseVisualStyleBackColor = true;
            this.BtnAddFiles.Click += new System.EventHandler(this.BtnAddFiles_Click);
            // 
            // BtnRemoveFiles
            // 
            this.BtnRemoveFiles.Enabled = false;
            this.BtnRemoveFiles.Location = new System.Drawing.Point(12, 48);
            this.BtnRemoveFiles.Name = "BtnRemoveFiles";
            this.BtnRemoveFiles.Size = new System.Drawing.Size(145, 30);
            this.BtnRemoveFiles.TabIndex = 2;
            this.BtnRemoveFiles.Text = "Remove Selected Files...";
            this.BtnRemoveFiles.UseVisualStyleBackColor = true;
            this.BtnRemoveFiles.Click += new System.EventHandler(this.BtnRemoveFiles_Click);
            // 
            // BtnEncryptSelected
            // 
            this.BtnEncryptSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnEncryptSelected.Enabled = false;
            this.BtnEncryptSelected.Location = new System.Drawing.Point(200, 48);
            this.BtnEncryptSelected.Name = "BtnEncryptSelected";
            this.BtnEncryptSelected.Size = new System.Drawing.Size(145, 30);
            this.BtnEncryptSelected.TabIndex = 3;
            this.BtnEncryptSelected.Text = "Encrypt Selected Files";
            this.BtnEncryptSelected.UseVisualStyleBackColor = true;
            this.BtnEncryptSelected.Click += new System.EventHandler(this.BtnEncryptSelected_Click);
            // 
            // BtnDecryptSelected
            // 
            this.BtnDecryptSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDecryptSelected.Enabled = false;
            this.BtnDecryptSelected.Location = new System.Drawing.Point(386, 48);
            this.BtnDecryptSelected.Name = "BtnDecryptSelected";
            this.BtnDecryptSelected.Size = new System.Drawing.Size(145, 30);
            this.BtnDecryptSelected.TabIndex = 4;
            this.BtnDecryptSelected.Text = "Decrypt Selected Files";
            this.BtnDecryptSelected.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 368);
            this.Controls.Add(this.BtnDecryptSelected);
            this.Controls.Add(this.BtnEncryptSelected);
            this.Controls.Add(this.BtnDecrypt);
            this.Controls.Add(this.BtnRemoveFiles);
            this.Controls.Add(this.BtnAddFiles);
            this.Controls.Add(this.BtnEncrypt);
            this.Controls.Add(this.TabMain);
            this.MinimumSize = new System.Drawing.Size(559, 407);
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Krypt Keeper - File Security";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.TabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FileListGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl TabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button BtnEncrypt;
        private System.Windows.Forms.Button BtnDecrypt;
        private System.Windows.Forms.Button BtnAddFiles;
        private System.Windows.Forms.Button BtnRemoveFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CbxMaskInformation;
        private System.Windows.Forms.CheckBox ChkMaskInformation;
        private System.Windows.Forms.CheckBox ChkRemoveAfterEncrypt;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button BtnExport;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TxtEncryptionKey;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox CbxEncryptAlgorithms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CbxDecryptAlgorithms;
        private System.Windows.Forms.CheckBox ChkUseEncryptSettings;
        private System.Windows.Forms.TextBox TxtDecryptionKey;
        private System.Windows.Forms.ComboBox CbxDecryptionKeyType;
        private System.Windows.Forms.ComboBox CbxEncryptionKeyType;
        private System.Windows.Forms.CheckBox ChkRememberSettings;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button BtnBrowseDecrypt;
        private System.Windows.Forms.Button BtnBrowseEncrypt;
        private System.Windows.Forms.DataGridView FileListGridView;
        private System.Windows.Forms.TextBox TxtStatus;
        private System.Windows.Forms.Button BtnEncryptSelected;
        private System.Windows.Forms.Button BtnDecryptSelected;
    }
}

