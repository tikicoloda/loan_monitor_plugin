namespace LoanMonitorPlugin.Forms
{
    partial class ScriptTestForm
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
            this.txtScript = new System.Windows.Forms.TextBox();
            this.txtFields = new System.Windows.Forms.TextBox();
            this.btnEvaluate = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSetField = new System.Windows.Forms.Button();
            this.txtSetField = new System.Windows.Forms.TextBox();
            this.txtSetValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtScript
            // 
            this.txtScript.Location = new System.Drawing.Point(35, 36);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtScript.Size = new System.Drawing.Size(632, 59);
            this.txtScript.TabIndex = 0;
            // 
            // txtFields
            // 
            this.txtFields.Enabled = false;
            this.txtFields.Location = new System.Drawing.Point(35, 253);
            this.txtFields.Multiline = true;
            this.txtFields.Name = "txtFields";
            this.txtFields.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtFields.Size = new System.Drawing.Size(632, 154);
            this.txtFields.TabIndex = 1;
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.Location = new System.Drawing.Point(35, 101);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(75, 23);
            this.btnEvaluate.TabIndex = 2;
            this.btnEvaluate.Text = "Evaluate";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            // 
            // txtResult
            // 
            this.txtResult.Enabled = false;
            this.txtResult.Location = new System.Drawing.Point(35, 164);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtResult.Size = new System.Drawing.Size(632, 45);
            this.txtResult.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter Script:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Script Result:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Loan Field Values:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtResult);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtScript);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtFields);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnEvaluate);
            this.panel1.Location = new System.Drawing.Point(41, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 430);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtSetValue);
            this.panel2.Controls.Add(this.txtSetField);
            this.panel2.Controls.Add(this.btnSetField);
            this.panel2.Location = new System.Drawing.Point(41, 449);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(687, 65);
            this.panel2.TabIndex = 8;
            // 
            // btnSetField
            // 
            this.btnSetField.Location = new System.Drawing.Point(536, 17);
            this.btnSetField.Name = "btnSetField";
            this.btnSetField.Size = new System.Drawing.Size(75, 23);
            this.btnSetField.TabIndex = 0;
            this.btnSetField.Text = "Set";
            this.btnSetField.UseVisualStyleBackColor = true;
            this.btnSetField.Click += new System.EventHandler(this.btnSetField_Click);
            // 
            // txtSetField
            // 
            this.txtSetField.Location = new System.Drawing.Point(102, 19);
            this.txtSetField.Name = "txtSetField";
            this.txtSetField.Size = new System.Drawing.Size(169, 20);
            this.txtSetField.TabIndex = 1;
            // 
            // txtSetValue
            // 
            this.txtSetValue.Location = new System.Drawing.Point(334, 19);
            this.txtSetValue.Name = "txtSetValue";
            this.txtSetValue.Size = new System.Drawing.Size(169, 20);
            this.txtSetValue.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Field ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(291, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Value:";
            // 
            // ScriptTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 526);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = global::LoanMonitorPlugin.Resources.TrayIcon;
            this.Name = "ScriptTestForm";
            this.Text = "Script Testing";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.TextBox txtFields;
        private System.Windows.Forms.Button btnEvaluate;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSetValue;
        private System.Windows.Forms.TextBox txtSetField;
        private System.Windows.Forms.Button btnSetField;
    }
}