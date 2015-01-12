namespace LoanMonitorPlugin.Forms
{
    partial class CommissionsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDitechStatements = new System.Windows.Forms.ComboBox();
            this.cmbDitechDelegate = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Pay Period";
            // 
            // cmbDitechStatements
            // 
            this.cmbDitechStatements.FormattingEnabled = true;
            this.cmbDitechStatements.Location = new System.Drawing.Point(120, 83);
            this.cmbDitechStatements.Name = "cmbDitechStatements";
            this.cmbDitechStatements.Size = new System.Drawing.Size(200, 21);
            this.cmbDitechStatements.TabIndex = 3;
            this.cmbDitechStatements.SelectedIndexChanged += new System.EventHandler(this.cmbDitechStatements_SelectedIndexChanged);
            // 
            // cmbDitechDelegate
            // 
            this.cmbDitechDelegate.FormattingEnabled = true;
            this.cmbDitechDelegate.Location = new System.Drawing.Point(120, 44);
            this.cmbDitechDelegate.Name = "cmbDitechDelegate";
            this.cmbDitechDelegate.Size = new System.Drawing.Size(200, 21);
            this.cmbDitechDelegate.TabIndex = 4;
            // 
            // CommissionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 356);
            this.Controls.Add(this.cmbDitechDelegate);
            this.Controls.Add(this.cmbDitechStatements);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CommissionsForm";
            this.Text = "Commissions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDitechStatements;
        private System.Windows.Forms.ComboBox cmbDitechDelegate;
    }
}