namespace LoanMonitorPlugin.Forms
{
    partial class CLIErrors
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvCLIErrors = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCLIErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCLIErrors
            // 
            this.dgvCLIErrors.AllowUserToAddRows = false;
            this.dgvCLIErrors.AllowUserToDeleteRows = false;
            this.dgvCLIErrors.AllowUserToOrderColumns = true;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.dgvCLIErrors.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCLIErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCLIErrors.Location = new System.Drawing.Point(5, 36);
            this.dgvCLIErrors.MultiSelect = false;
            this.dgvCLIErrors.Name = "dgvCLIErrors";
            this.dgvCLIErrors.ReadOnly = true;
            this.dgvCLIErrors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCLIErrors.Size = new System.Drawing.Size(725, 361);
            this.dgvCLIErrors.TabIndex = 5;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(6, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // CLIErrors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 402);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgvCLIErrors);
            this.Name = "CLIErrors";
            this.Text = "CLIErrors";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCLIErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCLIErrors;
        private System.Windows.Forms.Button btnExport;
    }
}