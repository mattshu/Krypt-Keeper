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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.chkProcessOrderDesc = new MetroFramework.Controls.MetroCheckBox();
            this.cbxProcessOrderBy = new MetroFramework.Controls.MetroComboBox();
            this.chkProcessInOrder = new MetroFramework.Controls.MetroCheckBox();
            this.lblJobInformation = new MetroFramework.Controls.MetroLabel();
            this.btnRemoveSelectedFiles = new MetroFramework.Controls.MetroButton();
            this.btnDecrypt = new MetroFramework.Controls.MetroButton();
            this.btnEncrypt = new MetroFramework.Controls.MetroButton();
            this.btnAddFiles = new MetroFramework.Controls.MetroButton();
            this.panelFiles = new MetroFramework.Controls.MetroPanel();
            this.datagridFileList = new MetroFramework.Controls.MetroGrid();
            this.tabStatus = new MetroFramework.Controls.MetroTabPage();
            this.progressTotalFiles = new MetroFramework.Controls.MetroProgressSpinner();
            this.chkOnCompletion = new MetroFramework.Controls.MetroCheckBox();
            this.panelOnCompletion = new MetroFramework.Controls.MetroPanel();
            this.panelIconClose = new MetroFramework.Controls.MetroPanel();
            this.panelIconSleep = new MetroFramework.Controls.MetroPanel();
            this.panelIconRestart = new MetroFramework.Controls.MetroPanel();
            this.panelIconShutdown = new MetroFramework.Controls.MetroPanel();
            this.txtStatus = new MetroFramework.Controls.MetroTextBox();
            this.lblProcessingRates = new MetroFramework.Controls.MetroLabel();
            this.lblCurrentPercentage = new MetroFramework.Controls.MetroLabel();
            this.progressCurrent = new MetroFramework.Controls.MetroProgressSpinner();
            this.lblTotalBytesPercentage = new MetroFramework.Controls.MetroLabel();
            this.progressTotalBytes = new MetroFramework.Controls.MetroProgressSpinner();
            this.btnExit = new MetroFramework.Controls.MetroButton();
            this.btnCancelOperation = new MetroFramework.Controls.MetroButton();
            this.btnSelectFilesFromStatusTab = new MetroFramework.Controls.MetroButton();
            this.btnExport = new MetroFramework.Controls.MetroButton();
            this.btnClear = new MetroFramework.Controls.MetroButton();
            this.lblTotalFiles = new MetroFramework.Controls.MetroLabel();
            this.lblTimeElapsed = new MetroFramework.Controls.MetroLabel();
            this.lblOperationStatus = new MetroFramework.Controls.MetroLabel();
            this.lblFileBeingProcessed = new MetroFramework.Controls.MetroLabel();
            this.lblVersionInformation = new MetroFramework.Controls.MetroLabel();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.metroToolTip = new MetroFramework.Components.MetroToolTip();
            this.lblTimeRemaining = new MetroFramework.Controls.MetroLabel();
            this.tabMain.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.panelKeyRads.SuspendLayout();
            this.tabFileProcessing.SuspendLayout();
            this.panelFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridFileList)).BeginInit();
            this.tabStatus.SuspendLayout();
            this.panelOnCompletion.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabMain.Controls.Add(this.tabOptions);
            this.tabMain.Controls.Add(this.tabFileProcessing);
            this.tabMain.Controls.Add(this.tabStatus);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.FontSize = MetroFramework.MetroTabControlSize.Tall;
            this.tabMain.ItemSize = new System.Drawing.Size(300, 50);
            this.tabMain.Location = new System.Drawing.Point(20, 60);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 2;
            this.tabMain.Size = new System.Drawing.Size(746, 391);
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
            this.tabOptions.Size = new System.Drawing.Size(738, 333);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.tabOptions.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabOptions.VerticalScrollbarBarColor = true;
            this.tabOptions.VerticalScrollbarHighlightOnWheel = false;
            this.tabOptions.VerticalScrollbarSize = 10;
            // 
            // btnBrowseKeyFile
            // 
            this.btnBrowseKeyFile.Location = new System.Drawing.Point(491, 130);
            this.btnBrowseKeyFile.Name = "btnBrowseKeyFile";
            this.btnBrowseKeyFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseKeyFile.TabIndex = 5;
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
            this.btnSelectFiles.TabIndex = 9;
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
            this.panelKeyRads.Location = new System.Drawing.Point(74, 131);
            this.panelKeyRads.Name = "panelKeyRads";
            this.panelKeyRads.Size = new System.Drawing.Size(199, 22);
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
            this.radKeyFile.Location = new System.Drawing.Point(3, 0);
            this.radKeyFile.Name = "radKeyFile";
            this.radKeyFile.Size = new System.Drawing.Size(68, 19);
            this.radKeyFile.TabIndex = 0;
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
            this.radPlaintextKey.Location = new System.Drawing.Point(93, 0);
            this.radPlaintextKey.Name = "radPlaintextKey";
            this.radPlaintextKey.Size = new System.Drawing.Size(103, 19);
            this.radPlaintextKey.TabIndex = 1;
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
            this.txtCipherKey.Lines = new string[] {
        "D:\\shu\\Pictures\\New Bitmap Image.bmp"};
            this.txtCipherKey.Location = new System.Drawing.Point(293, 130);
            this.txtCipherKey.MaxLength = 32767;
            this.txtCipherKey.Name = "txtCipherKey";
            this.txtCipherKey.PasswordChar = '\0';
            this.txtCipherKey.PromptText = "You must browse for a key file...";
            this.txtCipherKey.ReadOnly = true;
            this.txtCipherKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCipherKey.SelectedText = "";
            this.txtCipherKey.SelectionLength = 0;
            this.txtCipherKey.SelectionStart = 0;
            this.txtCipherKey.ShortcutsEnabled = true;
            this.txtCipherKey.Size = new System.Drawing.Size(192, 23);
            this.txtCipherKey.TabIndex = 4;
            this.txtCipherKey.Text = "D:\\shu\\Pictures\\New Bitmap Image.bmp";
            this.txtCipherKey.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtCipherKey.UseSelectable = true;
            this.txtCipherKey.WaterMark = "You must browse for a key file...";
            this.txtCipherKey.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCipherKey.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtCipherKey.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.txtCipherKey_ButtonClick);
            this.txtCipherKey.Click += new System.EventHandler(this.txtCipherKey_Click);
            // 
            // chkConfirmExit
            // 
            this.chkConfirmExit.AutoSize = true;
            this.chkConfirmExit.Checked = true;
            this.chkConfirmExit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConfirmExit.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkConfirmExit.Location = new System.Drawing.Point(74, 278);
            this.chkConfirmExit.Name = "chkConfirmExit";
            this.chkConfirmExit.Size = new System.Drawing.Size(119, 19);
            this.chkConfirmExit.TabIndex = 8;
            this.chkConfirmExit.Text = "Confirm on exit";
            this.chkConfirmExit.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkConfirmExit.UseSelectable = true;
            // 
            // chkRememberSettings
            // 
            this.chkRememberSettings.AutoSize = true;
            this.chkRememberSettings.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkRememberSettings.Location = new System.Drawing.Point(74, 230);
            this.chkRememberSettings.Name = "chkRememberSettings";
            this.chkRememberSettings.Size = new System.Drawing.Size(242, 19);
            this.chkRememberSettings.TabIndex = 7;
            this.chkRememberSettings.Text = "Remember all settings (except keys)";
            this.chkRememberSettings.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkRememberSettings.UseSelectable = true;
            // 
            // chkRemoveAfterDecryption
            // 
            this.chkRemoveAfterDecryption.AutoSize = true;
            this.chkRemoveAfterDecryption.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkRemoveAfterDecryption.Location = new System.Drawing.Point(74, 182);
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
            this.chkRemoveAfterEncryption.Location = new System.Drawing.Point(74, 83);
            this.chkRemoveAfterEncryption.Name = "chkRemoveAfterEncryption";
            this.chkRemoveAfterEncryption.Size = new System.Drawing.Size(245, 19);
            this.chkRemoveAfterEncryption.TabIndex = 3;
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
            this.chkMaskFileDate.TabIndex = 2;
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
            this.chkMaskFileName.TabIndex = 1;
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
            this.chkMaskFileInformation.TabIndex = 0;
            this.chkMaskFileInformation.Text = "Mask identifyable file details:";
            this.chkMaskFileInformation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkMaskFileInformation.UseSelectable = true;
            this.chkMaskFileInformation.CheckedChanged += new System.EventHandler(this.chkMaskFileInformation_CheckedChanged);
            // 
            // tabFileProcessing
            // 
            this.tabFileProcessing.Controls.Add(this.chkProcessOrderDesc);
            this.tabFileProcessing.Controls.Add(this.cbxProcessOrderBy);
            this.tabFileProcessing.Controls.Add(this.chkProcessInOrder);
            this.tabFileProcessing.Controls.Add(this.lblJobInformation);
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
            this.tabFileProcessing.Size = new System.Drawing.Size(738, 333);
            this.tabFileProcessing.TabIndex = 1;
            this.tabFileProcessing.Text = "File Processing";
            this.tabFileProcessing.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabFileProcessing.VerticalScrollbarBarColor = true;
            this.tabFileProcessing.VerticalScrollbarHighlightOnWheel = false;
            this.tabFileProcessing.VerticalScrollbarSize = 10;
            // 
            // chkProcessOrderDesc
            // 
            this.chkProcessOrderDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkProcessOrderDesc.AutoSize = true;
            this.chkProcessOrderDesc.Enabled = false;
            this.chkProcessOrderDesc.Location = new System.Drawing.Point(365, 279);
            this.chkProcessOrderDesc.Name = "chkProcessOrderDesc";
            this.chkProcessOrderDesc.Size = new System.Drawing.Size(85, 15);
            this.chkProcessOrderDesc.TabIndex = 4;
            this.chkProcessOrderDesc.Text = "Descending";
            this.chkProcessOrderDesc.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkProcessOrderDesc.UseSelectable = true;
            this.chkProcessOrderDesc.CheckedChanged += new System.EventHandler(this.chkProcessOrderDesc_CheckedChanged);
            // 
            // cbxProcessOrderBy
            // 
            this.cbxProcessOrderBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxProcessOrderBy.Enabled = false;
            this.cbxProcessOrderBy.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cbxProcessOrderBy.FontWeight = MetroFramework.MetroComboBoxWeight.Light;
            this.cbxProcessOrderBy.FormattingEnabled = true;
            this.cbxProcessOrderBy.ItemHeight = 19;
            this.cbxProcessOrderBy.Items.AddRange(new object[] {
            "File name",
            "File size"});
            this.cbxProcessOrderBy.Location = new System.Drawing.Point(365, 248);
            this.cbxProcessOrderBy.Name = "cbxProcessOrderBy";
            this.cbxProcessOrderBy.Size = new System.Drawing.Size(105, 25);
            this.cbxProcessOrderBy.TabIndex = 3;
            this.cbxProcessOrderBy.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.cbxProcessOrderBy.UseSelectable = true;
            // 
            // chkProcessInOrder
            // 
            this.chkProcessInOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkProcessInOrder.AutoSize = true;
            this.chkProcessInOrder.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkProcessInOrder.Location = new System.Drawing.Point(233, 249);
            this.chkProcessInOrder.Name = "chkProcessInOrder";
            this.chkProcessInOrder.Size = new System.Drawing.Size(126, 19);
            this.chkProcessInOrder.TabIndex = 2;
            this.chkProcessInOrder.Text = "Process in order:";
            this.chkProcessInOrder.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkProcessInOrder.UseSelectable = true;
            // 
            // lblJobInformation
            // 
            this.lblJobInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblJobInformation.Location = new System.Drawing.Point(233, 312);
            this.lblJobInformation.Name = "lblJobInformation";
            this.lblJobInformation.Size = new System.Drawing.Size(237, 18);
            this.lblJobInformation.TabIndex = 4;
            this.lblJobInformation.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // btnRemoveSelectedFiles
            // 
            this.btnRemoveSelectedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveSelectedFiles.Enabled = false;
            this.btnRemoveSelectedFiles.Location = new System.Drawing.Point(3, 294);
            this.btnRemoveSelectedFiles.Name = "btnRemoveSelectedFiles";
            this.btnRemoveSelectedFiles.Size = new System.Drawing.Size(224, 36);
            this.btnRemoveSelectedFiles.TabIndex = 1;
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
            this.btnDecrypt.Location = new System.Drawing.Point(610, 249);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(128, 81);
            this.btnDecrypt.TabIndex = 6;
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
            this.btnEncrypt.Location = new System.Drawing.Point(476, 249);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(128, 81);
            this.btnEncrypt.TabIndex = 5;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnEncrypt.UseSelectable = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddFiles.Location = new System.Drawing.Point(3, 249);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(224, 39);
            this.btnAddFiles.TabIndex = 0;
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
            this.panelFiles.Size = new System.Drawing.Size(735, 240);
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
            this.datagridFileList.AllowUserToResizeRows = false;
            this.datagridFileList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.datagridFileList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.datagridFileList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.datagridFileList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(208)))), ((int)(((byte)(104)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridFileList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(208)))), ((int)(((byte)(104)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datagridFileList.DefaultCellStyle = dataGridViewCellStyle2;
            this.datagridFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridFileList.EnableHeadersVisualStyles = false;
            this.datagridFileList.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.datagridFileList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.datagridFileList.Location = new System.Drawing.Point(0, 0);
            this.datagridFileList.Name = "datagridFileList";
            this.datagridFileList.ReadOnly = true;
            this.datagridFileList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(208)))), ((int)(((byte)(104)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridFileList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.datagridFileList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.datagridFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagridFileList.Size = new System.Drawing.Size(733, 238);
            this.datagridFileList.Style = MetroFramework.MetroColorStyle.Green;
            this.datagridFileList.TabIndex = 0;
            this.datagridFileList.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.datagridFileList.SelectionChanged += new System.EventHandler(this.datagridFileList_SelectionChanged);
            // 
            // tabStatus
            // 
            this.tabStatus.Controls.Add(this.progressTotalFiles);
            this.tabStatus.Controls.Add(this.chkOnCompletion);
            this.tabStatus.Controls.Add(this.panelOnCompletion);
            this.tabStatus.Controls.Add(this.txtStatus);
            this.tabStatus.Controls.Add(this.lblTimeRemaining);
            this.tabStatus.Controls.Add(this.lblProcessingRates);
            this.tabStatus.Controls.Add(this.lblCurrentPercentage);
            this.tabStatus.Controls.Add(this.progressCurrent);
            this.tabStatus.Controls.Add(this.lblTotalBytesPercentage);
            this.tabStatus.Controls.Add(this.progressTotalBytes);
            this.tabStatus.Controls.Add(this.btnExit);
            this.tabStatus.Controls.Add(this.btnCancelOperation);
            this.tabStatus.Controls.Add(this.btnSelectFilesFromStatusTab);
            this.tabStatus.Controls.Add(this.btnExport);
            this.tabStatus.Controls.Add(this.btnClear);
            this.tabStatus.Controls.Add(this.lblTotalFiles);
            this.tabStatus.Controls.Add(this.lblTimeElapsed);
            this.tabStatus.Controls.Add(this.lblOperationStatus);
            this.tabStatus.Controls.Add(this.lblFileBeingProcessed);
            this.tabStatus.HorizontalScrollbarBarColor = true;
            this.tabStatus.HorizontalScrollbarHighlightOnWheel = false;
            this.tabStatus.HorizontalScrollbarSize = 10;
            this.tabStatus.Location = new System.Drawing.Point(4, 54);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.Size = new System.Drawing.Size(738, 333);
            this.tabStatus.TabIndex = 2;
            this.tabStatus.Text = "Operational Status";
            this.tabStatus.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabStatus.VerticalScrollbarBarColor = true;
            this.tabStatus.VerticalScrollbarHighlightOnWheel = false;
            this.tabStatus.VerticalScrollbarSize = 10;
            // 
            // progressTotalFiles
            // 
            this.progressTotalFiles.Backwards = true;
            this.progressTotalFiles.EnsureVisible = false;
            this.progressTotalFiles.Location = new System.Drawing.Point(341, 4);
            this.progressTotalFiles.Maximum = 100;
            this.progressTotalFiles.Name = "progressTotalFiles";
            this.progressTotalFiles.Size = new System.Drawing.Size(64, 64);
            this.progressTotalFiles.Spinning = false;
            this.progressTotalFiles.Style = MetroFramework.MetroColorStyle.Teal;
            this.progressTotalFiles.TabIndex = 11;
            this.progressTotalFiles.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroToolTip.SetToolTip(this.progressTotalFiles, "Total file progress");
            this.progressTotalFiles.UseSelectable = true;
            this.progressTotalFiles.Value = 100;
            // 
            // chkOnCompletion
            // 
            this.chkOnCompletion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkOnCompletion.AutoSize = true;
            this.chkOnCompletion.FontWeight = MetroFramework.MetroCheckBoxWeight.Light;
            this.chkOnCompletion.Location = new System.Drawing.Point(0, 204);
            this.chkOnCompletion.Name = "chkOnCompletion";
            this.chkOnCompletion.Size = new System.Drawing.Size(100, 15);
            this.chkOnCompletion.TabIndex = 17;
            this.chkOnCompletion.Text = "On completion:";
            this.chkOnCompletion.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkOnCompletion.UseSelectable = true;
            this.chkOnCompletion.CheckedChanged += new System.EventHandler(this.chkOnCompletion_CheckedChanged);
            // 
            // panelOnCompletion
            // 
            this.panelOnCompletion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelOnCompletion.Controls.Add(this.panelIconClose);
            this.panelOnCompletion.Controls.Add(this.panelIconSleep);
            this.panelOnCompletion.Controls.Add(this.panelIconRestart);
            this.panelOnCompletion.Controls.Add(this.panelIconShutdown);
            this.panelOnCompletion.HorizontalScrollbarBarColor = true;
            this.panelOnCompletion.HorizontalScrollbarHighlightOnWheel = false;
            this.panelOnCompletion.HorizontalScrollbarSize = 10;
            this.panelOnCompletion.Location = new System.Drawing.Point(106, 204);
            this.panelOnCompletion.Name = "panelOnCompletion";
            this.panelOnCompletion.Size = new System.Drawing.Size(91, 18);
            this.panelOnCompletion.TabIndex = 16;
            this.panelOnCompletion.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.panelOnCompletion.VerticalScrollbarBarColor = true;
            this.panelOnCompletion.VerticalScrollbarHighlightOnWheel = false;
            this.panelOnCompletion.VerticalScrollbarSize = 10;
            // 
            // panelIconClose
            // 
            this.panelIconClose.BackColor = System.Drawing.Color.Transparent;
            this.panelIconClose.BackgroundImage = global::KryptKeeper.Properties.Resources.close_disabled;
            this.panelIconClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelIconClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelIconClose.Enabled = false;
            this.panelIconClose.HorizontalScrollbarBarColor = true;
            this.panelIconClose.HorizontalScrollbarHighlightOnWheel = false;
            this.panelIconClose.HorizontalScrollbarSize = 10;
            this.panelIconClose.Location = new System.Drawing.Point(69, 0);
            this.panelIconClose.Name = "panelIconClose";
            this.panelIconClose.Size = new System.Drawing.Size(16, 16);
            this.panelIconClose.TabIndex = 2;
            this.panelIconClose.Tag = false;
            this.panelIconClose.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroToolTip.SetToolTip(this.panelIconClose, "Exit KryptKeeper");
            this.panelIconClose.VerticalScrollbarBarColor = true;
            this.panelIconClose.VerticalScrollbarHighlightOnWheel = false;
            this.panelIconClose.VerticalScrollbarSize = 10;
            this.panelIconClose.Click += new System.EventHandler(this.panelIconClose_Click);
            // 
            // panelIconSleep
            // 
            this.panelIconSleep.BackColor = System.Drawing.Color.Transparent;
            this.panelIconSleep.BackgroundImage = global::KryptKeeper.Properties.Resources.sleep_disabled;
            this.panelIconSleep.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelIconSleep.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelIconSleep.Enabled = false;
            this.panelIconSleep.HorizontalScrollbarBarColor = true;
            this.panelIconSleep.HorizontalScrollbarHighlightOnWheel = false;
            this.panelIconSleep.HorizontalScrollbarSize = 10;
            this.panelIconSleep.Location = new System.Drawing.Point(47, 0);
            this.panelIconSleep.Name = "panelIconSleep";
            this.panelIconSleep.Size = new System.Drawing.Size(16, 16);
            this.panelIconSleep.TabIndex = 2;
            this.panelIconSleep.Tag = false;
            this.panelIconSleep.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroToolTip.SetToolTip(this.panelIconSleep, "Sleep");
            this.panelIconSleep.VerticalScrollbarBarColor = true;
            this.panelIconSleep.VerticalScrollbarHighlightOnWheel = false;
            this.panelIconSleep.VerticalScrollbarSize = 10;
            this.panelIconSleep.Click += new System.EventHandler(this.panelIconSleep_Click);
            // 
            // panelIconRestart
            // 
            this.panelIconRestart.BackColor = System.Drawing.Color.Transparent;
            this.panelIconRestart.BackgroundImage = global::KryptKeeper.Properties.Resources.restart_disabled;
            this.panelIconRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelIconRestart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelIconRestart.Enabled = false;
            this.panelIconRestart.HorizontalScrollbarBarColor = true;
            this.panelIconRestart.HorizontalScrollbarHighlightOnWheel = false;
            this.panelIconRestart.HorizontalScrollbarSize = 10;
            this.panelIconRestart.Location = new System.Drawing.Point(25, 0);
            this.panelIconRestart.Name = "panelIconRestart";
            this.panelIconRestart.Size = new System.Drawing.Size(16, 16);
            this.panelIconRestart.TabIndex = 2;
            this.panelIconRestart.Tag = false;
            this.panelIconRestart.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroToolTip.SetToolTip(this.panelIconRestart, "Restart");
            this.panelIconRestart.VerticalScrollbarBarColor = true;
            this.panelIconRestart.VerticalScrollbarHighlightOnWheel = false;
            this.panelIconRestart.VerticalScrollbarSize = 10;
            this.panelIconRestart.Click += new System.EventHandler(this.panelIconRestart_Click);
            // 
            // panelIconShutdown
            // 
            this.panelIconShutdown.BackColor = System.Drawing.Color.Transparent;
            this.panelIconShutdown.BackgroundImage = global::KryptKeeper.Properties.Resources.shutdown_disabled;
            this.panelIconShutdown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelIconShutdown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelIconShutdown.Enabled = false;
            this.panelIconShutdown.HorizontalScrollbarBarColor = true;
            this.panelIconShutdown.HorizontalScrollbarHighlightOnWheel = false;
            this.panelIconShutdown.HorizontalScrollbarSize = 10;
            this.panelIconShutdown.Location = new System.Drawing.Point(3, 0);
            this.panelIconShutdown.Name = "panelIconShutdown";
            this.panelIconShutdown.Size = new System.Drawing.Size(16, 16);
            this.panelIconShutdown.TabIndex = 2;
            this.panelIconShutdown.Tag = false;
            this.panelIconShutdown.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroToolTip.SetToolTip(this.panelIconShutdown, "Shutdown");
            this.panelIconShutdown.VerticalScrollbarBarColor = true;
            this.panelIconShutdown.VerticalScrollbarHighlightOnWheel = false;
            this.panelIconShutdown.VerticalScrollbarSize = 10;
            this.panelIconShutdown.Click += new System.EventHandler(this.panelIconShutdown_Click);
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
            this.txtStatus.CustomButton.Location = new System.Drawing.Point(404, 1);
            this.txtStatus.CustomButton.Name = "";
            this.txtStatus.CustomButton.Size = new System.Drawing.Size(103, 103);
            this.txtStatus.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtStatus.CustomButton.TabIndex = 1;
            this.txtStatus.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtStatus.CustomButton.UseSelectable = true;
            this.txtStatus.CustomButton.Visible = false;
            this.txtStatus.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            this.txtStatus.Lines = new string[0];
            this.txtStatus.Location = new System.Drawing.Point(230, 225);
            this.txtStatus.MaxLength = 32767;
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.PasswordChar = '\0';
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.SelectedText = "";
            this.txtStatus.SelectionLength = 0;
            this.txtStatus.SelectionStart = 0;
            this.txtStatus.ShortcutsEnabled = true;
            this.txtStatus.ShowClearButton = true;
            this.txtStatus.Size = new System.Drawing.Size(508, 105);
            this.txtStatus.TabIndex = 3;
            this.txtStatus.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtStatus.UseSelectable = true;
            this.txtStatus.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtStatus.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtStatus.TextChanged += new System.EventHandler(this.txtStatus_TextChanged);
            // 
            // lblProcessingRates
            // 
            this.lblProcessingRates.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblProcessingRates.Location = new System.Drawing.Point(230, 204);
            this.lblProcessingRates.Name = "lblProcessingRates";
            this.lblProcessingRates.Size = new System.Drawing.Size(158, 18);
            this.lblProcessingRates.TabIndex = 13;
            this.lblProcessingRates.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblCurrentPercentage
            // 
            this.lblCurrentPercentage.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lblCurrentPercentage.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblCurrentPercentage.Location = new System.Drawing.Point(71, 88);
            this.lblCurrentPercentage.Name = "lblCurrentPercentage";
            this.lblCurrentPercentage.Size = new System.Drawing.Size(59, 27);
            this.lblCurrentPercentage.TabIndex = 12;
            this.lblCurrentPercentage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCurrentPercentage.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // progressCurrent
            // 
            this.progressCurrent.Backwards = true;
            this.progressCurrent.EnsureVisible = false;
            this.progressCurrent.Location = new System.Drawing.Point(3, 4);
            this.progressCurrent.Maximum = 100;
            this.progressCurrent.Name = "progressCurrent";
            this.progressCurrent.Size = new System.Drawing.Size(194, 194);
            this.progressCurrent.Spinning = false;
            this.progressCurrent.Style = MetroFramework.MetroColorStyle.Blue;
            this.progressCurrent.TabIndex = 11;
            this.progressCurrent.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroToolTip.SetToolTip(this.progressCurrent, "File data progress");
            this.progressCurrent.UseSelectable = true;
            this.progressCurrent.Value = -1;
            // 
            // lblTotalBytesPercentage
            // 
            this.lblTotalBytesPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalBytesPercentage.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lblTotalBytesPercentage.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblTotalBytesPercentage.Location = new System.Drawing.Point(612, 88);
            this.lblTotalBytesPercentage.Name = "lblTotalBytesPercentage";
            this.lblTotalBytesPercentage.Size = new System.Drawing.Size(59, 27);
            this.lblTotalBytesPercentage.TabIndex = 10;
            this.lblTotalBytesPercentage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTotalBytesPercentage.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // progressTotalBytes
            // 
            this.progressTotalBytes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTotalBytes.Backwards = true;
            this.progressTotalBytes.EnsureVisible = false;
            this.progressTotalBytes.Location = new System.Drawing.Point(544, 4);
            this.progressTotalBytes.Maximum = 100;
            this.progressTotalBytes.Name = "progressTotalBytes";
            this.progressTotalBytes.Size = new System.Drawing.Size(194, 194);
            this.progressTotalBytes.Spinning = false;
            this.progressTotalBytes.Style = MetroFramework.MetroColorStyle.Green;
            this.progressTotalBytes.TabIndex = 9;
            this.progressTotalBytes.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroToolTip.SetToolTip(this.progressTotalBytes, "Total data progress");
            this.progressTotalBytes.UseSelectable = true;
            this.progressTotalBytes.Value = -1;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExit.Location = new System.Drawing.Point(0, 294);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(224, 36);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnExit.UseSelectable = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCancelOperation
            // 
            this.btnCancelOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelOperation.Enabled = false;
            this.btnCancelOperation.Highlight = true;
            this.btnCancelOperation.Location = new System.Drawing.Point(0, 249);
            this.btnCancelOperation.Name = "btnCancelOperation";
            this.btnCancelOperation.Size = new System.Drawing.Size(224, 39);
            this.btnCancelOperation.Style = MetroFramework.MetroColorStyle.Red;
            this.btnCancelOperation.TabIndex = 1;
            this.btnCancelOperation.Text = "CANCEL OPERATIONS";
            this.btnCancelOperation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnCancelOperation.UseSelectable = true;
            this.btnCancelOperation.Click += new System.EventHandler(this.btnCancelOperation_Click);
            // 
            // btnSelectFilesFromStatusTab
            // 
            this.btnSelectFilesFromStatusTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectFilesFromStatusTab.Enabled = false;
            this.btnSelectFilesFromStatusTab.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSelectFilesFromStatusTab.Location = new System.Drawing.Point(0, 225);
            this.btnSelectFilesFromStatusTab.Name = "btnSelectFilesFromStatusTab";
            this.btnSelectFilesFromStatusTab.Size = new System.Drawing.Size(224, 18);
            this.btnSelectFilesFromStatusTab.TabIndex = 0;
            this.btnSelectFilesFromStatusTab.Text = "Select More Files...";
            this.btnSelectFilesFromStatusTab.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnSelectFilesFromStatusTab.UseSelectable = true;
            this.btnSelectFilesFromStatusTab.Click += new System.EventHandler(this.btnSelectFilesFromStatusTab_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnExport.Location = new System.Drawing.Point(582, 204);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 18);
            this.btnExport.TabIndex = 4;
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
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnClear.UseSelectable = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblTotalFiles
            // 
            this.lblTotalFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalFiles.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblTotalFiles.Location = new System.Drawing.Point(236, 88);
            this.lblTotalFiles.Name = "lblTotalFiles";
            this.lblTotalFiles.Size = new System.Drawing.Size(272, 26);
            this.lblTotalFiles.Style = MetroFramework.MetroColorStyle.Blue;
            this.lblTotalFiles.TabIndex = 5;
            this.lblTotalFiles.Text = "Don\'t forget your key!";
            this.lblTotalFiles.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTotalFiles.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeElapsed.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblTimeElapsed.Location = new System.Drawing.Point(298, 123);
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
            this.lblOperationStatus.Location = new System.Drawing.Point(236, 149);
            this.lblOperationStatus.Name = "lblOperationStatus";
            this.lblOperationStatus.Size = new System.Drawing.Size(272, 29);
            this.lblOperationStatus.TabIndex = 5;
            this.lblOperationStatus.Text = "Awaiting instruction";
            this.lblOperationStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblOperationStatus.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblFileBeingProcessed
            // 
            this.lblFileBeingProcessed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileBeingProcessed.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblFileBeingProcessed.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblFileBeingProcessed.Location = new System.Drawing.Point(196, 178);
            this.lblFileBeingProcessed.Name = "lblFileBeingProcessed";
            this.lblFileBeingProcessed.Size = new System.Drawing.Size(342, 23);
            this.lblFileBeingProcessed.TabIndex = 4;
            this.lblFileBeingProcessed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblFileBeingProcessed.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblVersionInformation
            // 
            this.lblVersionInformation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVersionInformation.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblVersionInformation.Location = new System.Drawing.Point(592, 53);
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
            // metroToolTip
            // 
            this.metroToolTip.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip.StyleManager = null;
            this.metroToolTip.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblTimeRemaining.Location = new System.Drawing.Point(394, 204);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(182, 18);
            this.lblTimeRemaining.TabIndex = 13;
            this.lblTimeRemaining.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 471);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.lblVersionInformation);
            this.MinimumSize = new System.Drawing.Size(786, 450);
            this.Name = "MainWindow";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
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
            this.panelOnCompletion.ResumeLayout(false);
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
        private MetroFramework.Controls.MetroLabel lblJobInformation;
        private MetroFramework.Controls.MetroLabel lblFileBeingProcessed;
        private MetroFramework.Controls.MetroLabel lblOperationStatus;
        private MetroFramework.Controls.MetroButton btnExport;
        private MetroFramework.Controls.MetroButton btnClear;
        private MetroFramework.Controls.MetroButton btnCancelOperation;
        private MetroFramework.Controls.MetroTextBox txtStatus;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private MetroFramework.Controls.MetroButton btnSelectFiles;
        private MetroFramework.Controls.MetroLabel lblTimeElapsed;
        private MetroFramework.Controls.MetroLabel lblTotalFiles;
        private MetroFramework.Controls.MetroButton btnExit;
        private MetroFramework.Controls.MetroLabel lblTotalBytesPercentage;
        private MetroFramework.Controls.MetroProgressSpinner progressTotalBytes;
        private MetroFramework.Controls.MetroLabel lblCurrentPercentage;
        private MetroFramework.Controls.MetroProgressSpinner progressCurrent;
        private MetroFramework.Controls.MetroButton btnSelectFilesFromStatusTab;
        private MetroFramework.Controls.MetroLabel lblProcessingRates;
        private MetroFramework.Controls.MetroCheckBox chkProcessInOrder;
        private MetroFramework.Controls.MetroComboBox cbxProcessOrderBy;
        private MetroFramework.Controls.MetroCheckBox chkProcessOrderDesc;
        private MetroFramework.Controls.MetroPanel panelOnCompletion;
        private MetroFramework.Controls.MetroCheckBox chkOnCompletion;
        private MetroFramework.Controls.MetroPanel panelIconSleep;
        private MetroFramework.Controls.MetroPanel panelIconRestart;
        private MetroFramework.Controls.MetroPanel panelIconShutdown;
        private MetroFramework.Components.MetroToolTip metroToolTip;
        private MetroFramework.Controls.MetroProgressSpinner progressTotalFiles;
        private MetroFramework.Controls.MetroPanel panelIconClose;
        private MetroFramework.Controls.MetroLabel lblTimeRemaining;
    }
}