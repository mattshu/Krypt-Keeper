namespace KryptKeeper
{
    partial class ConfirmSettingsDialog
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
            this.BtnYes = new System.Windows.Forms.Button();
            this.BtnNo = new System.Windows.Forms.Button();
            this.ChkDoNotAskAgain = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnYes
            // 
            this.BtnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.BtnYes.Location = new System.Drawing.Point(199, 77);
            this.BtnYes.Name = "BtnYes";
            this.BtnYes.Size = new System.Drawing.Size(75, 23);
            this.BtnYes.TabIndex = 0;
            this.BtnYes.Text = "Yes";
            this.BtnYes.UseVisualStyleBackColor = true;
            this.BtnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // BtnNo
            // 
            this.BtnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.BtnNo.Location = new System.Drawing.Point(280, 77);
            this.BtnNo.Name = "BtnNo";
            this.BtnNo.Size = new System.Drawing.Size(75, 23);
            this.BtnNo.TabIndex = 0;
            this.BtnNo.Text = "No";
            this.BtnNo.UseVisualStyleBackColor = true;
            this.BtnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // ChkDoNotAskAgain
            // 
            this.ChkDoNotAskAgain.AutoSize = true;
            this.ChkDoNotAskAgain.Location = new System.Drawing.Point(234, 54);
            this.ChkDoNotAskAgain.Name = "ChkDoNotAskAgain";
            this.ChkDoNotAskAgain.Size = new System.Drawing.Size(121, 17);
            this.ChkDoNotAskAgain.TabIndex = 1;
            this.ChkDoNotAskAgain.Text = "Never ask me again";
            this.ChkDoNotAskAgain.UseVisualStyleBackColor = true;
            this.ChkDoNotAskAgain.CheckedChanged += new System.EventHandler(this.chkDoNotAskAgain_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Are you sure you want to continue? Select \'No\' to verify settings.";
            // 
            // ConfirmSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 112);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChkDoNotAskAgain);
            this.Controls.Add(this.BtnNo);
            this.Controls.Add(this.BtnYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmSettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm settings?";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnYes;
        private System.Windows.Forms.Button BtnNo;
        private System.Windows.Forms.CheckBox ChkDoNotAskAgain;
        private System.Windows.Forms.Label label1;
    }
}