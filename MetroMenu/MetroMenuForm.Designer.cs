namespace LoanMonitorPlugin.MetroMenu
{
    partial class MetroMenuForm
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
            this.metroFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // metroFlowLayout
            // 
            this.metroFlowLayout.AutoScroll = true;
            this.metroFlowLayout.Location = new System.Drawing.Point(12, 81);
            this.metroFlowLayout.Name = "metroFlowLayout";
            this.metroFlowLayout.Size = new System.Drawing.Size(838, 322);
            this.metroFlowLayout.TabIndex = 0;
            // 
            // MetroMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(862, 430);
            this.Controls.Add(this.metroFlowLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MetroMenuForm";
            this.Text = "MetroMenuForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel metroFlowLayout;
    }
}