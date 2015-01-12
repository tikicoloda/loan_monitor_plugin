namespace LoanMonitorPlugin.MetroMenu
{
    partial class MetroTile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TileText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TileText
            // 
            this.TileText.AutoSize = true;
            this.TileText.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TileText.ForeColor = System.Drawing.Color.White;
            this.TileText.Location = new System.Drawing.Point(29, 23);
            this.TileText.Name = "TileText";
            this.TileText.Size = new System.Drawing.Size(42, 16);
            this.TileText.TabIndex = 0;
            this.TileText.Text = "label1";
            // 
            // MetroTile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(47)))));
            this.Controls.Add(this.TileText);
            this.Name = "MetroTile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TileText;
    }
}
