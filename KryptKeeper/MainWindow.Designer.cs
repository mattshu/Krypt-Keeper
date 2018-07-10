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
            this.TabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.FileListGridView = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowseDecrypt = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.chkRememberSettings = new System.Windows.Forms.CheckBox();
            this.chkUseEncryptSettings = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxDecryptAlgorithms = new System.Windows.Forms.ComboBox();
            this.txtDecryptionKey = new System.Windows.Forms.TextBox();
            this.cbxDecryptionKeyType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowseEncrypt = new System.Windows.Forms.Button();
            this.cbxEncryptAlgorithms = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEncryptionKey = new System.Windows.Forms.TextBox();
            this.chkRemoveAfterEncrypt = new System.Windows.Forms.CheckBox();
            this.cbxEncryptionKeyType = new System.Windows.Forms.ComboBox();
            this.cbxMaskInformation = new System.Windows.Forms.ComboBox();
            this.chkMaskInformation = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.btnRemoveFiles = new System.Windows.Forms.Button();
            this.btnEncryptSelected = new System.Windows.Forms.Button();
            this.bttnDecryptSelected = new System.Windows.Forms.Button();
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
            this.TabMain.TabIndexChanged += new System.EventHandler(this.tabMain_TabIndexChanged);
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
            this.FileListGridView.DataSourceChanged += new System.EventHandler(this.fileListGridView_DataSourceChanged);
            this.FileListGridView.SelectionChanged += new System.EventHandler(this.fileListGridView_SelectionChanged);
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
            this.groupBox2.Controls.Add(this.btnBrowseDecrypt);
            this.groupBox2.Controls.Add(this.btnExit);
            this.groupBox2.Controls.Add(this.chkRememberSettings);
            this.groupBox2.Controls.Add(this.chkUseEncryptSettings);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbxDecryptAlgorithms);
            this.groupBox2.Controls.Add(this.txtDecryptionKey);
            this.groupBox2.Controls.Add(this.cbxDecryptionKeyType);
            this.groupBox2.Location = new System.Drawing.Point(3, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(502, 102);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Decryption Options";
            // 
            // btnBrowseDecrypt
            // 
            this.btnBrowseDecrypt.Enabled = false;
            this.btnBrowseDecrypt.Location = new System.Drawing.Point(342, 68);
            this.btnBrowseDecrypt.Name = "btnBrowseDecrypt";
            this.btnBrowseDecrypt.Size = new System.Drawing.Size(25, 21);
            this.btnBrowseDecrypt.TabIndex = 8;
            this.btnBrowseDecrypt.Text = "...";
            this.btnBrowseDecrypt.UseVisualStyleBackColor = true;
            this.btnBrowseDecrypt.Click += new System.EventHandler(this.btnBrowseDecrypt_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(392, 59);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(104, 30);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // chkRememberSettings
            // 
            this.chkRememberSettings.AutoSize = true;
            this.chkRememberSettings.Location = new System.Drawing.Point(367, 19);
            this.chkRememberSettings.Name = "chkRememberSettings";
            this.chkRememberSettings.Size = new System.Drawing.Size(132, 17);
            this.chkRememberSettings.TabIndex = 6;
            this.chkRememberSettings.Text = "Remember All Settings";
            this.chkRememberSettings.UseVisualStyleBackColor = true;
            // 
            // chkUseEncryptSettings
            // 
            this.chkUseEncryptSettings.AutoSize = true;
            this.chkUseEncryptSettings.Checked = true;
            this.chkUseEncryptSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseEncryptSettings.Location = new System.Drawing.Point(50, 19);
            this.chkUseEncryptSettings.Name = "chkUseEncryptSettings";
            this.chkUseEncryptSettings.Size = new System.Drawing.Size(136, 17);
            this.chkUseEncryptSettings.TabIndex = 6;
            this.chkUseEncryptSettings.Text = "Use encryption settings";
            this.chkUseEncryptSettings.UseVisualStyleBackColor = true;
            this.chkUseEncryptSettings.CheckedChanged += new System.EventHandler(this.chkUseEncryptSettings_CheckedChanged);
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
            // cbxDecryptAlgorithms
            // 
            this.cbxDecryptAlgorithms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDecryptAlgorithms.Enabled = false;
            this.cbxDecryptAlgorithms.Location = new System.Drawing.Point(181, 42);
            this.cbxDecryptAlgorithms.Name = "cbxDecryptAlgorithms";
            this.cbxDecryptAlgorithms.Size = new System.Drawing.Size(155, 21);
            this.cbxDecryptAlgorithms.TabIndex = 5;
            // 
            // txtDecryptionKey
            // 
            this.txtDecryptionKey.Location = new System.Drawing.Point(181, 69);
            this.txtDecryptionKey.Name = "txtDecryptionKey";
            this.txtDecryptionKey.ReadOnly = true;
            this.txtDecryptionKey.Size = new System.Drawing.Size(155, 20);
            this.txtDecryptionKey.TabIndex = 3;
            // 
            // cbxDecryptionKeyType
            // 
            this.cbxDecryptionKeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDecryptionKeyType.Enabled = false;
            this.cbxDecryptionKeyType.Items.AddRange(new object[] {
            "Plaintext key",
            "Key file"});
            this.cbxDecryptionKeyType.Location = new System.Drawing.Point(50, 68);
            this.cbxDecryptionKeyType.Name = "cbxDecryptionKeyType";
            this.cbxDecryptionKeyType.Size = new System.Drawing.Size(121, 21);
            this.cbxDecryptionKeyType.TabIndex = 1;
            this.cbxDecryptionKeyType.SelectedIndexChanged += new System.EventHandler(this.cbxDecryptionKeyType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBrowseEncrypt);
            this.groupBox1.Controls.Add(this.cbxEncryptAlgorithms);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtEncryptionKey);
            this.groupBox1.Controls.Add(this.chkRemoveAfterEncrypt);
            this.groupBox1.Controls.Add(this.cbxEncryptionKeyType);
            this.groupBox1.Controls.Add(this.cbxMaskInformation);
            this.groupBox1.Controls.Add(this.chkMaskInformation);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(502, 129);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Encryption Options";
            // 
            // btnBrowseEncrypt
            // 
            this.btnBrowseEncrypt.Enabled = false;
            this.btnBrowseEncrypt.Location = new System.Drawing.Point(342, 93);
            this.btnBrowseEncrypt.Name = "btnBrowseEncrypt";
            this.btnBrowseEncrypt.Size = new System.Drawing.Size(25, 21);
            this.btnBrowseEncrypt.TabIndex = 8;
            this.btnBrowseEncrypt.Text = "...";
            this.btnBrowseEncrypt.UseVisualStyleBackColor = true;
            this.btnBrowseEncrypt.Click += new System.EventHandler(this.btnBrowseEncrypt_Click);
            // 
            // cbxEncryptAlgorithms
            // 
            this.cbxEncryptAlgorithms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEncryptAlgorithms.Location = new System.Drawing.Point(181, 17);
            this.cbxEncryptAlgorithms.Name = "cbxEncryptAlgorithms";
            this.cbxEncryptAlgorithms.Size = new System.Drawing.Size(155, 21);
            this.cbxEncryptAlgorithms.TabIndex = 5;
            this.cbxEncryptAlgorithms.SelectedIndexChanged += new System.EventHandler(this.cbxEncryptAlgorithms_SelectedIndexChanged);
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
            // txtEncryptionKey
            // 
            this.txtEncryptionKey.Location = new System.Drawing.Point(181, 95);
            this.txtEncryptionKey.Name = "txtEncryptionKey";
            this.txtEncryptionKey.Size = new System.Drawing.Size(155, 20);
            this.txtEncryptionKey.TabIndex = 3;
            this.txtEncryptionKey.TextChanged += new System.EventHandler(this.txtEncryptionKey_TextChanged);
            // 
            // chkRemoveAfterEncrypt
            // 
            this.chkRemoveAfterEncrypt.AutoSize = true;
            this.chkRemoveAfterEncrypt.Location = new System.Drawing.Point(50, 72);
            this.chkRemoveAfterEncrypt.Name = "chkRemoveAfterEncrypt";
            this.chkRemoveAfterEncrypt.Size = new System.Drawing.Size(178, 17);
            this.chkRemoveAfterEncrypt.TabIndex = 2;
            this.chkRemoveAfterEncrypt.Text = "Remove original after encryption";
            this.chkRemoveAfterEncrypt.UseVisualStyleBackColor = true;
            // 
            // cbxEncryptionKeyType
            // 
            this.cbxEncryptionKeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEncryptionKeyType.FormattingEnabled = true;
            this.cbxEncryptionKeyType.Items.AddRange(new object[] {
            "Plaintext key",
            "Key file"});
            this.cbxEncryptionKeyType.Location = new System.Drawing.Point(50, 94);
            this.cbxEncryptionKeyType.Name = "cbxEncryptionKeyType";
            this.cbxEncryptionKeyType.Size = new System.Drawing.Size(121, 21);
            this.cbxEncryptionKeyType.TabIndex = 1;
            this.cbxEncryptionKeyType.SelectedIndexChanged += new System.EventHandler(this.cbxEncryptionKeyType_SelectedIndexChanged);
            // 
            // cbxMaskInformation
            // 
            this.cbxMaskInformation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMaskInformation.Enabled = false;
            this.cbxMaskInformation.Items.AddRange(new object[] {
            "File name",
            "File dates",
            "File name AND dates"});
            this.cbxMaskInformation.Location = new System.Drawing.Point(181, 45);
            this.cbxMaskInformation.Name = "cbxMaskInformation";
            this.cbxMaskInformation.Size = new System.Drawing.Size(155, 21);
            this.cbxMaskInformation.TabIndex = 1;
            // 
            // chkMaskInformation
            // 
            this.chkMaskInformation.AutoSize = true;
            this.chkMaskInformation.Location = new System.Drawing.Point(50, 47);
            this.chkMaskInformation.Name = "chkMaskInformation";
            this.chkMaskInformation.Size = new System.Drawing.Size(125, 17);
            this.chkMaskInformation.TabIndex = 0;
            this.chkMaskInformation.Text = "Mask file information:";
            this.chkMaskInformation.UseVisualStyleBackColor = true;
            this.chkMaskInformation.CheckedChanged += new System.EventHandler(this.chkMaskInformation_CheckedChanged);
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
            this.btnEncrypt.Size = new System.Drawing.Size(145, 30);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Text = "Encrypt All Files";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncryptAll_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecrypt.Enabled = false;
            this.btnDecrypt.Location = new System.Drawing.Point(386, 12);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(145, 30);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "Decrypt All Files";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecryptAll_Click);
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Location = new System.Drawing.Point(12, 12);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(145, 30);
            this.btnAddFiles.TabIndex = 2;
            this.btnAddFiles.Text = "Add Files...";
            this.btnAddFiles.UseVisualStyleBackColor = true;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
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
            this.btnRemoveFiles.Click += new System.EventHandler(this.btnRemoveFiles_Click);
            // 
            // btnEncryptSelected
            // 
            this.btnEncryptSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncryptSelected.Enabled = false;
            this.btnEncryptSelected.Location = new System.Drawing.Point(200, 48);
            this.btnEncryptSelected.Name = "btnEncryptSelected";
            this.btnEncryptSelected.Size = new System.Drawing.Size(145, 30);
            this.btnEncryptSelected.TabIndex = 3;
            this.btnEncryptSelected.Text = "Encrypt Selected Files";
            this.btnEncryptSelected.UseVisualStyleBackColor = true;
            this.btnEncryptSelected.Click += new System.EventHandler(this.btnEncryptSelected_Click);
            // 
            // bttnDecryptSelected
            // 
            this.bttnDecryptSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bttnDecryptSelected.Enabled = false;
            this.bttnDecryptSelected.Location = new System.Drawing.Point(386, 48);
            this.bttnDecryptSelected.Name = "bttnDecryptSelected";
            this.bttnDecryptSelected.Size = new System.Drawing.Size(145, 30);
            this.bttnDecryptSelected.TabIndex = 4;
            this.bttnDecryptSelected.Text = "Decrypt Selected Files";
            this.bttnDecryptSelected.UseVisualStyleBackColor = true;
            this.bttnDecryptSelected.Click += new System.EventHandler(this.btnDecryptSelected_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 368);
            this.Controls.Add(this.bttnDecryptSelected);
            this.Controls.Add(this.btnEncryptSelected);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnRemoveFiles);
            this.Controls.Add(this.btnAddFiles);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.TabMain);
            this.MinimumSize = new System.Drawing.Size(559, 407);
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Krypt Keeper - File Security";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.mainWindow_Shown);
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
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnAddFiles;
        private System.Windows.Forms.Button btnRemoveFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbxMaskInformation;
        private System.Windows.Forms.CheckBox chkMaskInformation;
        private System.Windows.Forms.CheckBox chkRemoveAfterEncrypt;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtEncryptionKey;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox cbxEncryptAlgorithms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxDecryptAlgorithms;
        private System.Windows.Forms.CheckBox chkUseEncryptSettings;
        private System.Windows.Forms.TextBox txtDecryptionKey;
        private System.Windows.Forms.ComboBox cbxDecryptionKeyType;
        private System.Windows.Forms.ComboBox cbxEncryptionKeyType;
        private System.Windows.Forms.CheckBox chkRememberSettings;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBrowseDecrypt;
        private System.Windows.Forms.Button btnBrowseEncrypt;
        private System.Windows.Forms.DataGridView FileListGridView;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnEncryptSelected;
        private System.Windows.Forms.Button bttnDecryptSelected;
    }
}

