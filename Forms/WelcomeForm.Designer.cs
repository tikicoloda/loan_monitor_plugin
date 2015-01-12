namespace LoanMonitorPlugin.Forms
{
    partial class WelcomeForm
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
            this.chkDisplayWelcome = new System.Windows.Forms.CheckBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // chkDisplayWelcome
            // 
            this.Icon = Resources.TrayIcon;
            this.chkDisplayWelcome.AutoSize = true;
            this.chkDisplayWelcome.Location = new System.Drawing.Point(13, 358);
            this.chkDisplayWelcome.Name = "chkDisplayWelcome";
            this.chkDisplayWelcome.Size = new System.Drawing.Size(110, 17);
            this.chkDisplayWelcome.TabIndex = 0;
            this.chkDisplayWelcome.Text = "Display on startup";
            this.chkDisplayWelcome.UseVisualStyleBackColor = true;
            this.chkDisplayWelcome.CheckedChanged += new System.EventHandler(this.chkDisplayWelcome_CheckedChanged);
            // 
            // webBrowser
            // 
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(12, 12);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(787, 340);
            this.webBrowser.TabIndex = 1;
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 387);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.chkDisplayWelcome);
            this.Name = "WelcomeForm";
            this.Text = "Welcome";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDisplayWelcome;
        private System.Windows.Forms.WebBrowser webBrowser;
    }
}