using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using EllieMae.Encompass.BusinessObjects.Loans;

namespace LoanMonitorPlugin.Forms
{
	/// <summary>
	/// Summary description for QuoteDialog.
	/// </summary>
	public class QuoteDialog : System.Windows.Forms.Form
    {
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnCancel;
        private WebBrowser webBrowser1;
		private Loan currentLoan = null;
        private Label label1;
        private String action = "";

		public QuoteDialog(Loan loan, String action)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Init the form
			this.currentLoan = loan;
            this.action = action;

            //TODO: perform lookup of action to derive:
            //        * url
            //        * inbound mappings
            //        * outbound mappings
            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            webBrowser1.Navigate("http://ftw1aml0492g0r:8001/PluginTest.htm");
            this.label1.Text = action;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Icon = Resources.TrayIcon;
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(314, 319);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 18;
            this.btnApply.Text = "&Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(234, 319);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "&Cancel";
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Location = new System.Drawing.Point(12, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(607, 301);
            this.webBrowser1.TabIndex = 20;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "label1";
            // 
            // QuoteDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(634, 345);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "QuoteDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "External Content";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		// Applies the insurance to the account
		private void btnApply_Click(object sender, System.EventArgs e)
		{
			//TODO: replace with logic to iterate through array of outbound mappings
            try
            {
                object[] parameters = new object[] { "#status" };
                string retValue = webBrowser1.Document.InvokeScript("getValue", parameters).ToString();
                //set loan fields
                this.currentLoan.Fields["CX.LOAN.DECISION"].Value = retValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error saving to loan::"+ex.Message);
            }
		}

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //TODO: replace with logic to iterate through array of inbound mappings
            if (e.Url.ToString().Equals("http://ftw1aml0492g0r:8001/PluginTest.htm"))
            {
                object[] parameters = new object[] { "#username", "InjectedUsername" };
                string retValue = (string)webBrowser1.Document.InvokeScript("setValue", parameters);
                parameters = new object[] { "#password", this.currentLoan.Fields["CX.INSURANCE"].Value };
                retValue = (string)webBrowser1.Document.InvokeScript("setValue", parameters);
            }
        }

	}
}
