namespace KryptKeeper
{
    partial class ConfirmExitWhileBusy
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.lblSubLabel = new MetroFramework.Controls.MetroLabel();
            this.btnAbort = new MetroFramework.Controls.MetroButton();
            this.btnStop = new MetroFramework.Controls.MetroButton();
            this.btnFinish = new MetroFramework.Controls.MetroButton();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.Location = new System.Drawing.Point(23, 86);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(358, 25);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "The application is in the middle of something.";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // lblSubLabel
            // 
            this.lblSubLabel.AutoSize = true;
            this.lblSubLabel.Location = new System.Drawing.Point(23, 122);
            this.lblSubLabel.Name = "lblSubLabel";
            this.lblSubLabel.Size = new System.Drawing.Size(186, 19);
            this.lblSubLabel.TabIndex = 1;
            this.lblSubLabel.Text = "How do you want to proceed?";
            this.lblSubLabel.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // btnAbort
            // 
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAbort.Location = new System.Drawing.Point(334, 150);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(182, 23);
            this.btnAbort.TabIndex = 2;
            this.btnAbort.Text = "Abort and Exit";
            this.btnAbort.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnAbort.UseSelectable = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnStop
            // 
            this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnStop.Location = new System.Drawing.Point(334, 179);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(182, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Finish File and Exit";
            this.btnStop.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnStop.UseSelectable = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnFinish.Location = new System.Drawing.Point(334, 208);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(182, 23);
            this.btnFinish.TabIndex = 2;
            this.btnFinish.Text = "Finish All in Background";
            this.btnFinish.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnFinish.UseSelectable = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Highlight = true;
            this.btnCancel.Location = new System.Drawing.Point(334, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(182, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ConfirmExitWhileBusy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 283);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.lblSubLabel);
            this.Controls.Add(this.metroLabel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmExitWhileBusy";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Operation in Progress";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel lblSubLabel;
        private MetroFramework.Controls.MetroButton btnAbort;
        private MetroFramework.Controls.MetroButton btnStop;
        private MetroFramework.Controls.MetroButton btnFinish;
        private MetroFramework.Controls.MetroButton btnCancel;
    }
}