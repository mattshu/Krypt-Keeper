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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabMain = new MetroFramework.Controls.MetroTabControl();
            this.tabOptions = new MetroFramework.Controls.MetroTabPage();
            this.btnBrowseKeyFile = new MetroFramework.Controls.MetroButton();
            this.btnSelectFiles = new MetroFramework.Controls.MetroButton();
            this.panelKeyRads = new MetroFramework.Controls.MetroPanel();
            this.radKeyFile = new MetroFramework.Controls.MetroRadioButton();
            this.radPlaintextKey = new MetroFramework.Controls.MetroRadioButton();
            this.txtCipherKey = new MetroFramework.Controls.MetroTextBox();
            this.chkConfirmExit = new MetroFramework.Controls.MetroCheckBox();
            this.chkRememberSettings = new MetroFramework.Controls.MetroCheckBox();
            this.chkRemoveAfterDecryption = new MetroFramework.Controls.MetroCheckBox();
            this.chkRemoveAfterEncryption = new MetroFramework.Controls.MetroCheckBox();
            this.chkMaskFileDate = new MetroFramework.Controls.MetroCheckBox();
            this.chkMaskFileName = new MetroFramework.Controls.MetroCheckBox();
            this.chkMaskFileInformation = new MetroFramework.Controls.MetroCheckBox();
            this.tabFileProcessing = new MetroFramework.Controls.MetroTabPage();
            this.lblFileCount = new MetroFramework.Controls.MetroLabel();
            this.lblPayload = new MetroFramework.Controls.MetroLabel();
            this.btnRemoveSelectedFiles = new MetroFramework.Controls.MetroButton();
            this.btnDecrypt = new MetroFramework.Controls.MetroButton();
            this.btnEncrypt = new MetroFramework.Controls.MetroButton();
            this.btnAddFiles = new MetroFramework.Controls.MetroButton();
            this.panelFiles = new MetroFramework.Controls.MetroPanel();
            this.datagridFileList = new MetroFramework.Controls.MetroGrid();
            this.tabStatus = new MetroFramework.Controls.MetroTabPage();
            this.btnExit = new MetroFramework.Controls.MetroButton();
            this.txtStatus = new MetroFramework.Controls.MetroTextBox();
            this.btnCancelOperation = new MetroFramework.Controls.MetroButton();
            this.btnExport = new MetroFramework.Controls.MetroButton();
            this.btnClear = new MetroFramework.Controls.MetroButton();
            this.lblTotalPercentage = new MetroFramework.Controls.MetroLabel();
            this.lblCurrentPercentage = new MetroFramework.Controls.MetroLabel();
            this.lblFilesToBeProcessed = new MetroFramework.Controls.MetroLabel();
            this.lblTimeElapsed = new MetroFramework.Controls.MetroLabel();
            this.lblOperationStatus = new MetroFramework.Controls.MetroLabel();
            this.lblProcessingFile = new MetroFramework.Controls.MetroLabel();
            this.progressTotal = new MetroFramework.Controls.MetroProgressSpinner();
            this.progressCurrent = new MetroFramework.Controls.MetroProgressSpinner();
            this.lblVersionInformation = new MetroFramework.Controls.MetroLabel();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tabMain.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.panelKeyRads.SuspendLayout();
            this.tabFileProcessing.SuspendLayout();
            this.panelFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridFileList)).BeginInit();
            this.tabStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabOptions);
            this.tabMain.Controls.Add(this.tabFileProcessing);
            this.tabMain.Controls.Add(this.tabStatus);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.FontSize = MetroFramework.MetroTabControlSize.Tall;
            this.tabMain.ItemSize = new System.Drawing.Size(300, 50);
            this.tabMain.Location = new System.Drawing.Point(20, 60);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 2;
            this.tabMain.Size = new System.Drawing.Size(746, 370);
            this.tabMain.Style = MetroFramework.MetroColorStyle.Green;
            this.tabMain.TabIndex = 0;
            this.tabMain.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabMain.UseSelectable = true;
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.btnBrowseKeyFile);
            this.tabOptions.Controls.Add(this.btnSelectFiles);
            this.tabOptions.Controls.Add(this.panelKeyRads);
            this.tabOptions.Controls.Add(this.txtCipherKey);
            this.tabOptions.Controls.Add(this.chkConfirmExit);
            this.tabOptions.Controls.Add(this.chkRememberSettings);
            this.tabOptions.Controls.Add(this.chkRemoveAfterDecryption);
            this.tabOptions.Controls.Add(this.chkRemoveAfterEncryption);
            this.tabOptions.Controls.Add(this.chkMaskFileDate);
            this.tabOptions.Controls.Add(this.chkMaskFileName);
            this.tabOptions.Controls.Add(this.chkMaskFileInformation);
            this.tabOptions.HorizontalScrollbarBarColor = true;
            this.tabOptions.HorizontalScrollbarHighlightOnWheel = false;
            this.tabOptions.HorizontalScrollbarSize = 10;
            this.tabOptions.Location = new System.Drawing.Point(4, 54);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new System.Drawing.Size(738, 312);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.tabOptions.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabOptions.VerticalScrollbarBarColor = true;
            this.tabOptions.VerticalScrollbarHighlightOnWheel = false;
            this.tabOptions.VerticalScrollbarSize = 10;
            // 
            // btnBrowseKeyFile
            // 
            this.btnBrowseKeyFile.Location = new System.Drawing.Point(490, 120);
            this.btnBrowseKeyFile.Name = "btnBrowseKeyFile";
            this.btnBrowseKeyFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseKeyFile.TabIndex = 15;
            this.btnBrowseKeyFile.Text = "Browse...";
            this.btnBrowseKeyFile.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnBrowseKeyFile.UseSelectable = true;
            this.btnBrowseKeyFile.Click += new System.EventHandler(this.btnBrowseKeyFile_Click);
            // 
            // btnSelectFiles
            // 
            this.btnSelectFiles.Highlight = true;
            this.btnSelectFiles.Location = new System.Drawing.Point(476, 228);
            this.btnSelectFiles.Name = "btnSelectFiles";
            this.btnSelectFiles.Size = new System.Drawing.Size(262, 81);
            this.btnSelectFiles.Style = MetroFramework.MetroColorStyle.Lime;
            this.btnSelectFiles.TabIndex = 14;
            this.btnSelectFiles.Text = "Select Files";
            this.btnSelectFiles.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnSelectFiles.UseSelectable = true;
            this.btnSelectFiles.Click += new System.EventHandler(this.btnSelectFiles_Click);
            // 
            // panelKeyRads
            // 
            this.panelKeyRads.Controls.Add(this.radKeyFile);
            this.panelKeyRads.Controls.Add(this.radPlaintextKey);
            this.panelKeyRads.HorizontalScrollbarBarColor = false;
            this.panelKeyRads.HorizontalScrollbarHighlightOnWheel = false;
            this.panelKeyRads.HorizontalScrollbarSize = 10;
            this.panelKeyRads.Location = new System.Drawing.Point(74, 103);
            this.panelKeyRads.Name = "panelKeyRads";
            this.panelKeyRads.Size = new System.Drawing.Size(199, 40);
            this.panelKeyRads.TabIndex = 13;
            this.panelKeyRads.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.panelKeyRads.VerticalScrollbarBarColor = false;
            this.panelKeyRads.VerticalScrollbarHighlightOnWheel = false;
            this.panelKeyRads.VerticalScrollbarSize = 10;
            // 
            // radKeyFile
            // 
            this.radKeyFile.AutoSize = true;
            this.radKeyFile.Checked = true;
            this.radKeyFile.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.radKeyFile.Location = new System.Drawing.Point(3, 18);
            this.radKeyFile.Name = "radKeyFile";
            this.radKeyFile.Size = new System.Drawing.Size(68, 19);
            this.radKeyFile.TabIndex = 12;
            this.radKeyFile.TabStop = true;
            this.radKeyFile.Text = "Key file";
            this.radKeyFile.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.radKeyFile.UseSelectable = true;
            this.radKeyFile.CheckedChanged += new System.EventHandler(this.radKeyFile_CheckedChanged);
            // 
            // radPlaintextKey
            // 
            this.radPlaintextKey.AutoSize = true;
            this.radPlaintextKey.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.radPlaintextKey.Location = new System.Drawing.Point(93, 18);
            this.radPlaintextKey.Name = "radPlaintextKey";
            this.radPlaintextKey.Size = new System.Drawing.Size(103, 19);
            this.radPlaintextKey.TabIndex = 12;
            this.radPlaintextKey.Text = "Plaintext Key";
            this.radPlaintextKey.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.radPlaintextKey.UseSelectable = true;
            // 
            // txtCipherKey
            // 
            // 
            // 
            // 
            this.txtCipherKey.CustomButton.Image = null;
            this.txtCipherKey.CustomButton.Location = new System.Drawing.Point(170, 1);
            this.txtCipherKey.CustomButton.Name = "";
            this.txtCipherKey.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtCipherKey.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCipherKey.CustomButton.TabIndex = 1;
            this.txtCipherKey.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCipherKey.CustomButton.UseSelectable = true;
            this.txtCipherKey.CustomButton.Visible = false;
            this.txtCipherKey.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtCipherKey.Lines = new string[0];
            this.txtCipherKey.Location = new System.Drawing.Point(292, 120);
            this.txtCipherKey.MaxLength = 32767;
            this.txtCipherKey.Name = "txtCipherKey";
            this.txtCipherKey.PasswordChar = '\0';
            this.txtCipherKey.WaterMark = "You must browse for a key file...";
            this.txtCipherKey.ReadOnly = true;
            this.txtCipherKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCipherKey.SelectedText = "";
            this.txtCipherKey.SelectionLength = 0;
            this.txtCipherKey.SelectionStart = 0;
            this.txtCipherKey.ShortcutsEnabled = true;
            this.txtCipherKey.Size = new System.Drawing.Size(192, 23);
            this.txtCipherKey.TabIndex = 10;
            this.txtCipherKey.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtCipherKey.UseSelectable = true;
            this.txtCipherKey.WaterMark = "You must browse for a key file...";
            this.txtCipherKey.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCipherKey.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtCipherKey.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.txtCipherKey_ButtonClick);
            // 
            // chkConfirmExit
            // 
            this.chkConfirmExit.AutoSize = true;
            this.chkConfirmExit.Checked = true;
            this.chkConfirmExit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConfirmExit.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkConfirmExit.Location = new System.Drawing.Point(74, 240);
            this.chkConfirmExit.Name = "chkConfirmExit";
            this.chkConfirmExit.Size = new System.Drawing.Size(119, 19);
            this.chkConfirmExit.TabIndex = 6;
            this.chkConfirmExit.Text = "Confirm on exit";
            this.chkConfirmExit.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkConfirmExit.UseSelectable = true;
            // 
            // chkRememberSettings
            // 
            this.chkRememberSettings.AutoSize = true;
            this.chkRememberSettings.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkRememberSettings.Location = new System.Drawing.Point(74, 206);
            this.chkRememberSettings.Name = "chkRememberSettings";
            this.chkRememberSettings.Size = new System.Drawing.Size(160, 19);
            this.chkRememberSettings.TabIndex = 6;
            this.chkRememberSettings.Text = "Remember all settings";
            this.chkRememberSettings.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkRememberSettings.UseSelectable = true;
            // 
            // chkRemoveAfterDecryption
            // 
            this.chkRemoveAfterDecryption.AutoSize = true;
            this.chkRemoveAfterDecryption.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkRemoveAfterDecryption.Location = new System.Drawing.Point(74, 172);
            this.chkRemoveAfterDecryption.Name = "chkRemoveAfterDecryption";
            this.chkRemoveAfterDecryption.Size = new System.Drawing.Size(261, 19);
            this.chkRemoveAfterDecryption.TabIndex = 6;
            this.chkRemoveAfterDecryption.Text = "Remove encrypted file after decryption";
            this.chkRemoveAfterDecryption.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkRemoveAfterDecryption.UseSelectable = true;
            // 
            // chkRemoveAfterEncryption
            // 
            this.chkRemoveAfterEncryption.AutoSize = true;
            this.chkRemoveAfterEncryption.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkRemoveAfterEncryption.Location = new System.Drawing.Point(74, 69);
            this.chkRemoveAfterEncryption.Name = "chkRemoveAfterEncryption";
            this.chkRemoveAfterEncryption.Size = new System.Drawing.Size(245, 19);
            this.chkRemoveAfterEncryption.TabIndex = 6;
            this.chkRemoveAfterEncryption.Text = "Remove original file after encryption";
            this.chkRemoveAfterEncryption.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkRemoveAfterEncryption.UseSelectable = true;
            // 
            // chkMaskFileDate
            // 
            this.chkMaskFileDate.AutoSize = true;
            this.chkMaskFileDate.Enabled = false;
            this.chkMaskFileDate.Location = new System.Drawing.Point(373, 39);
            this.chkMaskFileDate.Name = "chkMaskFileDate";
            this.chkMaskFileDate.Size = new System.Drawing.Size(133, 15);
            this.chkMaskFileDate.TabIndex = 5;
            this.chkMaskFileDate.Text = "File date information";
            this.chkMaskFileDate.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkMaskFileDate.UseSelectable = true;
            // 
            // chkMaskFileName
            // 
            this.chkMaskFileName.AutoSize = true;
            this.chkMaskFileName.Enabled = false;
            this.chkMaskFileName.Location = new System.Drawing.Point(293, 39);
            this.chkMaskFileName.Name = "chkMaskFileName";
            this.chkMaskFileName.Size = new System.Drawing.Size(74, 15);
            this.chkMaskFileName.TabIndex = 5;
            this.chkMaskFileName.Text = "File name";
            this.chkMaskFileName.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkMaskFileName.UseSelectable = true;
            // 
            // chkMaskFileInformation
            // 
            this.chkMaskFileInformation.AutoSize = true;
            this.chkMaskFileInformation.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkMaskFileInformation.Location = new System.Drawing.Point(74, 35);
            this.chkMaskFileInformation.Name = "chkMaskFileInformation";
            this.chkMaskFileInformation.Size = new System.Drawing.Size(199, 19);
            this.chkMaskFileInformation.TabIndex = 5;
            this.chkMaskFileInformation.Text = "Mask identifyable file details:";
            this.chkMaskFileInformation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkMaskFileInformation.UseSelectable = true;
            this.chkMaskFileInformation.CheckedChanged += new System.EventHandler(this.chkMaskFileInformation_CheckedChanged);
            // 
            // tabFileProcessing
            // 
            this.tabFileProcessing.Controls.Add(this.lblFileCount);
            this.tabFileProcessing.Controls.Add(this.lblPayload);
            this.tabFileProcessing.Controls.Add(this.btnRemoveSelectedFiles);
            this.tabFileProcessing.Controls.Add(this.btnDecrypt);
            this.tabFileProcessing.Controls.Add(this.btnEncrypt);
            this.tabFileProcessing.Controls.Add(this.btnAddFiles);
            this.tabFileProcessing.Controls.Add(this.panelFiles);
            this.tabFileProcessing.HorizontalScrollbarBarColor = true;
            this.tabFileProcessing.HorizontalScrollbarHighlightOnWheel = false;
            this.tabFileProcessing.HorizontalScrollbarSize = 10;
            this.tabFileProcessing.Location = new System.Drawing.Point(4, 54);
            this.tabFileProcessing.Margin = new System.Windows.Forms.Padding(30, 344, 3, 3);
            this.tabFileProcessing.Name = "tabFileProcessing";
            this.tabFileProcessing.Padding = new System.Windows.Forms.Padding(130, 0, 0, 0);
            this.tabFileProcessing.Size = new System.Drawing.Size(738, 312);
            this.tabFileProcessing.TabIndex = 1;
            this.tabFileProcessing.Text = "File Processing";
            this.tabFileProcessing.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabFileProcessing.VerticalScrollbarBarColor = true;
            this.tabFileProcessing.VerticalScrollbarHighlightOnWheel = false;
            this.tabFileProcessing.VerticalScrollbarSize = 10;
            // 
            // lblFileCount
            // 
            this.lblFileCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Location = new System.Drawing.Point(233, 267);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(0, 0);
            this.lblFileCount.TabIndex = 4;
            this.lblFileCount.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblPayload
            // 
            this.lblPayload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPayload.AutoSize = true;
            this.lblPayload.Location = new System.Drawing.Point(243, 286);
            this.lblPayload.Name = "lblPayload";
            this.lblPayload.Size = new System.Drawing.Size(0, 0);
            this.lblPayload.TabIndex = 4;
            this.lblPayload.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // btnRemoveSelectedFiles
            // 
            this.btnRemoveSelectedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveSelectedFiles.Enabled = false;
            this.btnRemoveSelectedFiles.Location = new System.Drawing.Point(3, 273);
            this.btnRemoveSelectedFiles.Name = "btnRemoveSelectedFiles";
            this.btnRemoveSelectedFiles.Size = new System.Drawing.Size(224, 36);
            this.btnRemoveSelectedFiles.TabIndex = 3;
            this.btnRemoveSelectedFiles.Text = "Remove Selected Files";
            this.btnRemoveSelectedFiles.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnRemoveSelectedFiles.UseSelectable = true;
            this.btnRemoveSelectedFiles.Click += new System.EventHandler(this.btnRemoveSelectedFiles_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecrypt.Enabled = false;
            this.btnDecrypt.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnDecrypt.Location = new System.Drawing.Point(610, 228);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(128, 81);
            this.btnDecrypt.TabIndex = 3;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnDecrypt.UseSelectable = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncrypt.Enabled = false;
            this.btnEncrypt.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnEncrypt.Location = new System.Drawing.Point(476, 228);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(128, 81);
            this.btnEncrypt.TabIndex = 3;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnEncrypt.UseSelectable = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddFiles.Location = new System.Drawing.Point(3, 228);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(224, 39);
            this.btnAddFiles.TabIndex = 3;
            this.btnAddFiles.Text = "Add Files...";
            this.btnAddFiles.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnAddFiles.UseSelectable = true;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // panelFiles
            // 
            this.panelFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFiles.Controls.Add(this.datagridFileList);
            this.panelFiles.HorizontalScrollbarBarColor = true;
            this.panelFiles.HorizontalScrollbarHighlightOnWheel = false;
            this.panelFiles.HorizontalScrollbarSize = 10;
            this.panelFiles.Location = new System.Drawing.Point(3, 3);
            this.panelFiles.Name = "panelFiles";
            this.panelFiles.Size = new System.Drawing.Size(735, 219);
            this.panelFiles.TabIndex = 2;
            this.panelFiles.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.panelFiles.VerticalScrollbarBarColor = true;
            this.panelFiles.VerticalScrollbarHighlightOnWheel = false;
            this.panelFiles.VerticalScrollbarSize = 10;
            // 
            // datagridFileList
            // 
            this.datagridFileList.AllowUserToAddRows = false;
            this.datagridFileList.AllowUserToDeleteRows = false;
            this.datagridFileList.AllowUserToOrderColumns = true;
            this.datagridFileList.AllowUserToResizeRows = false;
            this.datagridFileList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.datagridFileList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.datagridFileList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.datagridFileList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(208)))), ((int)(((byte)(104)))));
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridFileList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.datagridFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(208)))), ((int)(((byte)(104)))));
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datagridFileList.DefaultCellStyle = dataGridViewCellStyle23;
            this.datagridFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridFileList.EnableHeadersVisualStyles = false;
            this.datagridFileList.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.datagridFileList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.datagridFileList.Location = new System.Drawing.Point(0, 0);
            this.datagridFileList.Name = "datagridFileList";
            this.datagridFileList.ReadOnly = true;
            this.datagridFileList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(208)))), ((int)(((byte)(104)))));
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridFileList.RowHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.datagridFileList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.datagridFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagridFileList.Size = new System.Drawing.Size(733, 217);
            this.datagridFileList.Style = MetroFramework.MetroColorStyle.Green;
            this.datagridFileList.TabIndex = 2;
            this.datagridFileList.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.datagridFileList.DataSourceChanged += new System.EventHandler(this.datagridFileList_DataSourceChanged);
            this.datagridFileList.SelectionChanged += new System.EventHandler(this.datagridFileList_SelectionChanged);
            // 
            // tabStatus
            // 
            this.tabStatus.Controls.Add(this.btnExit);
            this.tabStatus.Controls.Add(this.txtStatus);
            this.tabStatus.Controls.Add(this.btnCancelOperation);
            this.tabStatus.Controls.Add(this.btnExport);
            this.tabStatus.Controls.Add(this.btnClear);
            this.tabStatus.Controls.Add(this.lblTotalPercentage);
            this.tabStatus.Controls.Add(this.lblCurrentPercentage);
            this.tabStatus.Controls.Add(this.lblFilesToBeProcessed);
            this.tabStatus.Controls.Add(this.lblTimeElapsed);
            this.tabStatus.Controls.Add(this.lblOperationStatus);
            this.tabStatus.Controls.Add(this.lblProcessingFile);
            this.tabStatus.Controls.Add(this.progressTotal);
            this.tabStatus.Controls.Add(this.progressCurrent);
            this.tabStatus.HorizontalScrollbarBarColor = true;
            this.tabStatus.HorizontalScrollbarHighlightOnWheel = false;
            this.tabStatus.HorizontalScrollbarSize = 10;
            this.tabStatus.Location = new System.Drawing.Point(4, 54);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.Size = new System.Drawing.Size(738, 312);
            this.tabStatus.TabIndex = 2;
            this.tabStatus.Text = "Operational Status";
            this.tabStatus.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabStatus.VerticalScrollbarBarColor = true;
            this.tabStatus.VerticalScrollbarHighlightOnWheel = false;
            this.tabStatus.VerticalScrollbarSize = 10;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExit.Enabled = false;
            this.btnExit.Location = new System.Drawing.Point(3, 273);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(224, 36);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit";
            this.btnExit.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnExit.UseSelectable = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtStatus.CustomButton.Image = null;
            this.txtStatus.CustomButton.Location = new System.Drawing.Point(425, 1);
            this.txtStatus.CustomButton.Name = "";
            this.txtStatus.CustomButton.Size = new System.Drawing.Size(79, 79);
            this.txtStatus.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtStatus.CustomButton.TabIndex = 1;
            this.txtStatus.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtStatus.CustomButton.UseSelectable = true;
            this.txtStatus.CustomButton.Visible = false;
            this.txtStatus.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            this.txtStatus.Lines = new string[0];
            this.txtStatus.Location = new System.Drawing.Point(233, 228);
            this.txtStatus.MaxLength = 32767;
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.PasswordChar = '\0';
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.SelectedText = "";
            this.txtStatus.SelectionLength = 0;
            this.txtStatus.SelectionStart = 0;
            this.txtStatus.ShortcutsEnabled = true;
            this.txtStatus.Size = new System.Drawing.Size(505, 81);
            this.txtStatus.TabIndex = 7;
            this.txtStatus.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtStatus.UseSelectable = true;
            this.txtStatus.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtStatus.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtStatus.TextChanged += new System.EventHandler(this.txtStatus_TextChanged);
            // 
            // btnCancelOperation
            // 
            this.btnCancelOperation.Enabled = false;
            this.btnCancelOperation.Highlight = true;
            this.btnCancelOperation.Location = new System.Drawing.Point(3, 228);
            this.btnCancelOperation.Name = "btnCancelOperation";
            this.btnCancelOperation.Size = new System.Drawing.Size(224, 39);
            this.btnCancelOperation.Style = MetroFramework.MetroColorStyle.Red;
            this.btnCancelOperation.TabIndex = 6;
            this.btnCancelOperation.Text = "CANCEL OPERATIONS";
            this.btnCancelOperation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnCancelOperation.UseSelectable = true;
            this.btnCancelOperation.Click += new System.EventHandler(this.btnCancelOperation_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnExport.Location = new System.Drawing.Point(582, 204);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 18);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export...";
            this.btnExport.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnExport.UseSelectable = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnClear.Location = new System.Drawing.Point(663, 204);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 18);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear";
            this.btnClear.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnClear.UseSelectable = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblTotalPercentage
            // 
            this.lblTotalPercentage.AutoSize = true;
            this.lblTotalPercentage.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lblTotalPercentage.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblTotalPercentage.Location = new System.Drawing.Point(616, 19);
            this.lblTotalPercentage.Name = "lblTotalPercentage";
            this.lblTotalPercentage.Size = new System.Drawing.Size(0, 0);
            this.lblTotalPercentage.TabIndex = 5;
            this.lblTotalPercentage.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblCurrentPercentage
            // 
            this.lblCurrentPercentage.AutoSize = true;
            this.lblCurrentPercentage.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lblCurrentPercentage.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblCurrentPercentage.Location = new System.Drawing.Point(80, 19);
            this.lblCurrentPercentage.Name = "lblCurrentPercentage";
            this.lblCurrentPercentage.Size = new System.Drawing.Size(0, 0);
            this.lblCurrentPercentage.TabIndex = 5;
            this.lblCurrentPercentage.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblFilesToBeProcessed
            // 
            this.lblFilesToBeProcessed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFilesToBeProcessed.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblFilesToBeProcessed.Location = new System.Drawing.Point(204, 63);
            this.lblFilesToBeProcessed.Name = "lblFilesToBeProcessed";
            this.lblFilesToBeProcessed.Size = new System.Drawing.Size(337, 26);
            this.lblFilesToBeProcessed.Style = MetroFramework.MetroColorStyle.Blue;
            this.lblFilesToBeProcessed.TabIndex = 5;
            this.lblFilesToBeProcessed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblFilesToBeProcessed.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeElapsed.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblTimeElapsed.Location = new System.Drawing.Point(298, 106);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new System.Drawing.Size(148, 26);
            this.lblTimeElapsed.Style = MetroFramework.MetroColorStyle.Blue;
            this.lblTimeElapsed.TabIndex = 5;
            this.lblTimeElapsed.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTimeElapsed.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblOperationStatus
            // 
            this.lblOperationStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOperationStatus.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lblOperationStatus.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblOperationStatus.Location = new System.Drawing.Point(174, 149);
            this.lblOperationStatus.Name = "lblOperationStatus";
            this.lblOperationStatus.Size = new System.Drawing.Size(396, 29);
            this.lblOperationStatus.TabIndex = 5;
            this.lblOperationStatus.Text = "Awaiting instruction";
            this.lblOperationStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblOperationStatus.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.Location = new System.Drawing.Point(3, 178);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(738, 23);
            this.lblProcessingFile.TabIndex = 4;
            this.lblProcessingFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProcessingFile.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // progressTotal
            // 
            this.progressTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTotal.EnsureVisible = false;
            this.progressTotal.Location = new System.Drawing.Point(576, 47);
            this.progressTotal.Maximum = 100;
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(128, 128);
            this.progressTotal.Spinning = false;
            this.progressTotal.Style = MetroFramework.MetroColorStyle.Lime;
            this.progressTotal.TabIndex = 2;
            this.progressTotal.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.progressTotal.UseSelectable = true;
            this.progressTotal.Value = -1;
            // 
            // progressCurrent
            // 
            this.progressCurrent.Backwards = true;
            this.progressCurrent.EnsureVisible = false;
            this.progressCurrent.Location = new System.Drawing.Point(40, 47);
            this.progressCurrent.Maximum = 100;
            this.progressCurrent.Name = "progressCurrent";
            this.progressCurrent.Size = new System.Drawing.Size(128, 128);
            this.progressCurrent.Spinning = false;
            this.progressCurrent.Style = MetroFramework.MetroColorStyle.Blue;
            this.progressCurrent.TabIndex = 2;
            this.progressCurrent.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.progressCurrent.UseSelectable = true;
            this.progressCurrent.Value = -1;
            // 
            // lblVersionInformation
            // 
            this.lblVersionInformation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVersionInformation.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblVersionInformation.Location = new System.Drawing.Point(592, 43);
            this.lblVersionInformation.Name = "lblVersionInformation";
            this.lblVersionInformation.Size = new System.Drawing.Size(167, 14);
            this.lblVersionInformation.TabIndex = 1;
            this.lblVersionInformation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVersionInformation.Theme = MetroFramework.MetroThemeStyle.Dark;
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
            this.ClientSize = new System.Drawing.Size(786, 450);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.lblVersionInformation);
            this.MinimumSize = new System.Drawing.Size(786, 450);
            this.Name = "MainWindow";
            this.Style = MetroFramework.MetroColorStyle.Lime;
            this.Text = "Krypt Keeper";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.mainWindow_Shown);
            this.tabMain.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
            this.panelKeyRads.ResumeLayout(false);
            this.panelKeyRads.PerformLayout();
            this.tabFileProcessing.ResumeLayout(false);
            this.tabFileProcessing.PerformLayout();
            this.panelFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridFileList)).EndInit();
            this.tabStatus.ResumeLayout(false);
            this.tabStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroTabControl tabMain;
        private MetroFramework.Controls.MetroTabPage tabOptions;
        private MetroFramework.Controls.MetroTabPage tabFileProcessing;
        private MetroFramework.Controls.MetroTabPage tabStatus;
        private MetroFramework.Controls.MetroCheckBox chkRemoveAfterEncryption;
        private MetroFramework.Controls.MetroCheckBox chkMaskFileInformation;
        private MetroFramework.Controls.MetroTextBox txtCipherKey;
        private MetroFramework.Controls.MetroCheckBox chkRememberSettings;
        private MetroFramework.Controls.MetroRadioButton radKeyFile;
        private MetroFramework.Controls.MetroCheckBox chkMaskFileDate;
        private MetroFramework.Controls.MetroCheckBox chkMaskFileName;
        private MetroFramework.Controls.MetroRadioButton radPlaintextKey;
        private MetroFramework.Controls.MetroPanel panelKeyRads;
        private MetroFramework.Controls.MetroCheckBox chkConfirmExit;
        private MetroFramework.Controls.MetroCheckBox chkRemoveAfterDecryption;
        private MetroFramework.Controls.MetroButton btnBrowseKeyFile;
        private MetroFramework.Controls.MetroLabel lblVersionInformation;
        private MetroFramework.Controls.MetroPanel panelFiles;
        private MetroFramework.Controls.MetroButton btnRemoveSelectedFiles;
        private MetroFramework.Controls.MetroButton btnDecrypt;
        private MetroFramework.Controls.MetroButton btnEncrypt;
        private MetroFramework.Controls.MetroButton btnAddFiles;
        private MetroFramework.Controls.MetroGrid datagridFileList;
        private MetroFramework.Controls.MetroLabel lblPayload;
        private MetroFramework.Controls.MetroLabel lblFileCount;
        private MetroFramework.Controls.MetroProgressSpinner progressCurrent;
        private MetroFramework.Controls.MetroLabel lblProcessingFile;
        private MetroFramework.Controls.MetroProgressSpinner progressTotal;
        private MetroFramework.Controls.MetroLabel lblOperationStatus;
        private MetroFramework.Controls.MetroLabel lblCurrentPercentage;
        private MetroFramework.Controls.MetroButton btnExport;
        private MetroFramework.Controls.MetroButton btnClear;
        private MetroFramework.Controls.MetroButton btnCancelOperation;
        private MetroFramework.Controls.MetroTextBox txtStatus;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private MetroFramework.Controls.MetroButton btnSelectFiles;
        private MetroFramework.Controls.MetroLabel lblTotalPercentage;
        private MetroFramework.Controls.MetroLabel lblTimeElapsed;
        private MetroFramework.Controls.MetroLabel lblFilesToBeProcessed;
        private MetroFramework.Controls.MetroButton btnExit;
    }
}