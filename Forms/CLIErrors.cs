using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LoanMonitorPlugin.Forms
{
    public partial class CLIErrors : Form
    {
        public CLIErrors()
        {
            InitializeComponent();
            
            dgvCLIErrors.DataSource = GetErrorDescription();
            dgvCLIErrors.Columns[0].Width = 60;
            dgvCLIErrors.Columns[1].Width = 120;
            dgvCLIErrors.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void buildDataGrid()
        {           
            dgvCLIErrors.Columns.Add("id", "ID");
            dgvCLIErrors.Columns.Add("loanNumber", "Loan Number");
            dgvCLIErrors.Columns.Add("e360Status", "E360 Status");
            dgvCLIErrors.Columns.Add("e360StatusChangeDate", "E360 Status Change Date");
            dgvCLIErrors.Columns.Add("status", "Status");
            dgvCLIErrors.Columns.Add("editID", "Edit ID");
            dgvCLIErrors.Columns.Add("editMsg", "Edit Message");//error description
            dgvCLIErrors.Columns.Add("editType", "Edit Type");
            dgvCLIErrors.Columns.Add("e360CLIfieldNames", "E360 CLI Field Names");
            dgvCLIErrors.Columns.Add("editDate", "Edit Date");

        }

        private DataTable GetErrorDescription()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("Loan Number");
            //dt.Columns.Add("E360 Status");
            //dt.Columns.Add("E360 Status Change Date");
            //dt.Columns.Add("Status");
            //dt.Columns.Add("Edit ID");
            dt.Columns.Add("Edit Message");//error description
            //dt.Columns.Add("Edit Type");
            //dt.Columns.Add("E360 CLI Field Names");
            //dt.Columns.Add("Edit Date");

            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = i.ToString();
                dr["Loan Number"] = "Loan Number " + i.ToString();
                dr["Edit Message"] = "This is temp error message description Number " + i.ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {   
           
        }
    }
}
