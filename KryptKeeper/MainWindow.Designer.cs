namespace KryptKeeper
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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridFileList = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkConfirmOnExit = new System.Windows.Forms.CheckBox();
            this.btnBrowseOrShowKey = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSaveKey = new System.Windows.Forms.CheckBox();
            this.chkRememberSettings = new System.Windows.Forms.CheckBox();
            this.txtCipherKey = new System.Windows.Forms.TextBox();
            this.cbxCipherKeyType = new System.Windows.Forms.ComboBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMaskInformation = new System.Windows.Forms.CheckBox();
            this.cbxMaskInformation = new System.Windows.Forms.ComboBox();
            this.chkRemoveAfterEncrypt = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnAddFilesOrCancelOperation = new System.Windows.Forms.Button();
            this.btnRemoveFiles = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFileList)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Controls.Add(this.tabPage3);
            this.tabMain.Location = new System.Drawing.Point(12, 81);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(519, 275);
            this.tabMain.TabIndex = 1;
            this.tabMain.TabIndexChanged += new System.EventHandler(this.tabMain_TabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridFileList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(511, 249);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridFileList
            // 
            this.gridFileList.AllowUserToAddRows = false;
            this.gridFileList.AllowUserToDeleteRows = false;
            this.gridFileList.AllowUserToOrderColumns = true;
            this.gridFileList.AllowUserToResizeRows = false;
            this.gridFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFileList.Location = new System.Drawing.Point(3, 3);
            this.gridFileList.Name = "gridFileList";
            this.gridFileList.ReadOnly = true;
            this.gridFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFileList.Size = new System.Drawing.Size(505, 243);
            this.gridFileList.TabIndex = 0;
            this.gridFileList.DataSourceChanged += new System.EventHandler(this.fileListGridView_DataSourceChanged);
            this.gridFileList.SelectionChanged += new System.EventHandler(this.gridFileList_SelectionChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.btnExit);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(511, 249);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.chkConfirmOnExit);
            this.groupBox1.Controls.Add(this.btnBrowseOrShowKey);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkSaveKey);
            this.groupBox1.Controls.Add(this.chkRememberSettings);
            this.groupBox1.Controls.Add(this.txtCipherKey);
            this.groupBox1.Controls.Add(this.cbxCipherKeyType);
            this.groupBox1.Location = new System.Drawing.Point(3, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(505, 101);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(161, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(155, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "AES";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkConfirmOnExit
            // 
            this.chkConfirmOnExit.AutoSize = true;
            this.chkConfirmOnExit.Checked = true;
            this.chkConfirmOnExit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConfirmOnExit.Location = new System.Drawing.Point(364, 64);
            this.chkConfirmOnExit.Name = "chkConfirmOnExit";
            this.chkConfirmOnExit.Size = new System.Drawing.Size(98, 17);
            this.chkConfirmOnExit.TabIndex = 6;
            this.chkConfirmOnExit.Text = "Confirm On Exit";
            this.chkConfirmOnExit.UseVisualStyleBackColor = true;
            // 
            // btnBrowseOrShowKey
            // 
            this.btnBrowseOrShowKey.Location = new System.Drawing.Point(322, 56);
            this.btnBrowseOrShowKey.Name = "btnBrowseOrShowKey";
            this.btnBrowseOrShowKey.Size = new System.Drawing.Size(25, 21);
            this.btnBrowseOrShowKey.TabIndex = 8;
            this.btnBrowseOrShowKey.Text = "...";
            this.btnBrowseOrShowKey.UseVisualStyleBackColor = true;
            this.btnBrowseOrShowKey.Click += new System.EventHandler(this.btnBrowseOrShowKey_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Algorithm:";
            // 
            // chkSaveKey
            // 
            this.chkSaveKey.AutoSize = true;
            this.chkSaveKey.Location = new System.Drawing.Point(364, 41);
            this.chkSaveKey.Name = "chkSaveKey";
            this.chkSaveKey.Size = new System.Drawing.Size(72, 17);
            this.chkSaveKey.TabIndex = 6;
            this.chkSaveKey.Text = "Save Key";
            this.chkSaveKey.UseVisualStyleBackColor = true;
            // 
            // chkRememberSettings
            // 
            this.chkRememberSettings.AutoSize = true;
            this.chkRememberSettings.Location = new System.Drawing.Point(364, 18);
            this.chkRememberSettings.Name = "chkRememberSettings";
            this.chkRememberSettings.Size = new System.Drawing.Size(132, 17);
            this.chkRememberSettings.TabIndex = 6;
            this.chkRememberSettings.Text = "Remember All Settings";
            this.chkRememberSettings.UseVisualStyleBackColor = true;
            // 
            // txtCipherKey
            // 
            this.txtCipherKey.Location = new System.Drawing.Point(161, 57);
            this.txtCipherKey.Name = "txtCipherKey";
            this.txtCipherKey.Size = new System.Drawing.Size(155, 20);
            this.txtCipherKey.TabIndex = 3;
            this.txtCipherKey.UseSystemPasswordChar = true;
            // 
            // cbxCipherKeyType
            // 
            this.cbxCipherKeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCipherKeyType.FormattingEnabled = true;
            this.cbxCipherKeyType.Items.AddRange(new object[] {
            "Plaintext key",
            "Key file"});
            this.cbxCipherKeyType.Location = new System.Drawing.Point(30, 56);
            this.cbxCipherKeyType.Name = "cbxCipherKeyType";
            this.cbxCipherKeyType.Size = new System.Drawing.Size(121, 21);
            this.cbxCipherKeyType.TabIndex = 1;
            this.cbxCipherKeyType.SelectedIndexChanged += new System.EventHandler(this.cbxCipherKeyType_SelectedIndexChanged);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(376, 213);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(129, 30);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkMaskInformation);
            this.groupBox2.Controls.Add(this.cbxMaskInformation);
            this.groupBox2.Controls.Add(this.chkRemoveAfterEncrypt);
            this.groupBox2.Location = new System.Drawing.Point(3, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(505, 105);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encryption Options";
            // 
            // chkMaskInformation
            // 
            this.chkMaskInformation.AutoSize = true;
            this.chkMaskInformation.Location = new System.Drawing.Point(30, 31);
            this.chkMaskInformation.Name = "chkMaskInformation";
            this.chkMaskInformation.Size = new System.Drawing.Size(125, 17);
            this.chkMaskInformation.TabIndex = 0;
            this.chkMaskInformation.Text = "Mask file information:";
            this.chkMaskInformation.UseVisualStyleBackColor = true;
            this.chkMaskInformation.CheckedChanged += new System.EventHandler(this.chkMaskInformation_CheckedChanged);
            // 
            // cbxMaskInformation
            // 
            this.cbxMaskInformation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMaskInformation.Enabled = false;
            this.cbxMaskInformation.Items.AddRange(new object[] {
            "File name",
            "File dates",
            "File name AND dates"});
            this.cbxMaskInformation.Location = new System.Drawing.Point(161, 29);
            this.cbxMaskInformation.Name = "cbxMaskInformation";
            this.cbxMaskInformation.Size = new System.Drawing.Size(155, 21);
            this.cbxMaskInformation.TabIndex = 1;
            // 
            // chkRemoveAfterEncrypt
            // 
            this.chkRemoveAfterEncrypt.AutoSize = true;
            this.chkRemoveAfterEncrypt.Location = new System.Drawing.Point(30, 56);
            this.chkRemoveAfterEncrypt.Name = "chkRemoveAfterEncrypt";
            this.chkRemoveAfterEncrypt.Size = new System.Drawing.Size(178, 17);
            this.chkRemoveAfterEncrypt.TabIndex = 2;
            this.chkRemoveAfterEncrypt.Text = "Remove original after encryption";
            this.chkRemoveAfterEncrypt.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtStatus);
            this.tabPage3.Controls.Add(this.progressBar);
            this.tabPage3.Controls.Add(this.btnExport);
            this.tabPage3.Controls.Add(this.btnClear);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(511, 249);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Status";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(6, 35);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(499, 179);
            this.txtStatus.TabIndex = 7;
            this.txtStatus.TextChanged += new System.EventHandler(this.txtStatus_TextChanged);
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
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(450, 220);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(56, 23);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(388, 220);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(56, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncrypt.Enabled = false;
            this.btnEncrypt.Location = new System.Drawing.Point(200, 12);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(145, 66);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Text = "Encrypt Files";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecrypt.Enabled = false;
            this.btnDecrypt.Location = new System.Drawing.Point(386, 12);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(145, 63);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "Decrypt Files";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnAddFilesOrCancelOperation
            // 
            this.btnAddFilesOrCancelOperation.Location = new System.Drawing.Point(12, 12);
            this.btnAddFilesOrCancelOperation.Name = "btnAddFilesOrCancelOperation";
            this.btnAddFilesOrCancelOperation.Size = new System.Drawing.Size(145, 30);
            this.btnAddFilesOrCancelOperation.TabIndex = 2;
            this.btnAddFilesOrCancelOperation.Text = "Add Files...";
            this.btnAddFilesOrCancelOperation.UseVisualStyleBackColor = true;
            this.btnAddFilesOrCancelOperation.Click += new System.EventHandler(this.btnAddFilesOrCancelOperation_Click);
            // 
            // btnRemoveFiles
            // 
            this.btnRemoveFiles.Enabled = false;
            this.btnRemoveFiles.Location = new System.Drawing.Point(12, 48);
            this.btnRemoveFiles.Name = "btnRemoveFiles";
            this.btnRemoveFiles.Size = new System.Drawing.Size(145, 30);
            this.btnRemoveFiles.TabIndex = 2;
            this.btnRemoveFiles.Text = "Remove Selected Files...";
            this.btnRemoveFiles.UseVisualStyleBackColor = true;
            this.btnRemoveFiles.Click += new System.EventHandler(this.btnRemoveSelectedFiles_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 368);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnRemoveFiles);
            this.Controls.Add(this.btnAddFilesOrCancelOperation);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.tabMain);
            this.MinimumSize = new System.Drawing.Size(559, 407);
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Krypt Keeper - File Security";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.mainWindow_Shown);
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFileList)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnAddFilesOrCancelOperation;
        private System.Windows.Forms.Button btnRemoveFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbxMaskInformation;
        private System.Windows.Forms.CheckBox chkMaskInformation;
        private System.Windows.Forms.CheckBox chkRemoveAfterEncrypt;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtCipherKey;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxCipherKeyType;
        private System.Windows.Forms.CheckBox chkRememberSettings;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBrowseOrShowKey;
        private System.Windows.Forms.DataGridView gridFileList;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.CheckBox chkConfirmOnExit;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkSaveKey;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

