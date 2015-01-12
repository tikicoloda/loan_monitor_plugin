using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Automation;
using System.Xml;
using System.IO;

namespace LoanMonitorPlugin.Forms
{
    public partial class ExportForm : Form
    {
        public ExportForm()
        {
            InitializeComponent();
            LoadList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadList()
        {
            Type EnumType = typeof(LoanExportFormat);
            Array Values = System.Enum.GetValues(EnumType);

            foreach (int Value in Values)
            {
                string Display = Enum.GetName(EnumType, Value);
                ListHelper Item = new ListHelper(Display, Value);

                cmbExportTypes.Items.Add(Item);
                cmbExportTypes.DisplayMember = "LongName";
                cmbExportTypes.ValueMember = "ShortName";
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                ListHelper interfaceFlagType = (ListHelper)cmbExportTypes.SelectedItem;
                int selectedExportType = interfaceFlagType.ShortName;

                LoanExportFormat selected = (LoanExportFormat)System.Enum.Parse(typeof(LoanExportFormat), selectedExportType.ToString());
                
                try
                {
                    string exp = EncompassApplication.CurrentLoan.ExportAsText("AAJTDQXWVQ", selected, false);

                    txtContent.Text = FormatXMLString(exp);
                    txtContent.SelectAll();
                    txtContent.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error was returned by the Export Engine:" + Environment.NewLine + ex.Message);
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            if (cmbExportTypes.SelectedIndex < 0)
            {
                isValid = false;
                MessageBox.Show("Please select an Export Type.");
            }
            return isValid;
        }
        private string FormatXMLString(string sUnformattedXML)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sUnformattedXML);
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                XmlTextWriter xtw = null;
                try
                {
                    xtw = new XmlTextWriter(sw);
                    xtw.Formatting = Formatting.Indented;
                    xd.WriteTo(xtw);
                }
                finally
                {
                    if (xtw != null)
                        xtw.Close();
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return sUnformattedXML;
            }
        }

    }
}
