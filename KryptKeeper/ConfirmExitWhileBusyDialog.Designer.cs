namespace KryptKeeper
{
    partial class ConfirmExitWhileBusyDialog
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
            this.btnAbortAndExit = new System.Windows.Forms.Button();
            this.btnFinishInBackground = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAbortAndExit
            // 
            this.btnAbortAndExit.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAbortAndExit.Location = new System.Drawing.Point(15, 77);
            this.btnAbortAndExit.Name = "btnAbortAndExit";
            this.btnAbortAndExit.Size = new System.Drawing.Size(128, 23);
            this.btnAbortAndExit.TabIndex = 0;
            this.btnAbortAndExit.Text = "Abort and Exit";
            this.btnAbortAndExit.UseVisualStyleBackColor = true;
            this.btnAbortAndExit.Click += new System.EventHandler(this.btnStopExit_Click);
            // 
            // btnFinishInBackground
            // 
            this.btnFinishInBackground.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnFinishInBackground.Location = new System.Drawing.Point(158, 77);
            this.btnFinishInBackground.Name = "btnFinishInBackground";
            this.btnFinishInBackground.Size = new System.Drawing.Size(124, 23);
            this.btnFinishInBackground.TabIndex = 0;
            this.btnFinishInBackground.Text = "Finish In Background";
            this.btnFinishInBackground.UseVisualStyleBackColor = true;
            this.btnFinishInBackground.Click += new System.EventHandler(this.btnFinishExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "The application is in the middle of something.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Stop operations and exit?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Or finish current job then exit?";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(310, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ConfirmExitWhileBusyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(406, 112);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFinishInBackground);
            this.Controls.Add(this.btnAbortAndExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmExitWhileBusyDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Operation In Progress";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAbortAndExit;
        private System.Windows.Forms.Button btnFinishInBackground;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
    }
}