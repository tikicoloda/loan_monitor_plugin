using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;

namespace LoanMonitorPlugin.Forms
{
    public partial class TrayConfigForm : Form
    {
        public TrayConfigForm()
        {
            InitializeComponent();

            txtXMLContent.Text = getTextFromEncompass();
        }

        private string getTextFromEncompass()
        {
            EllieMae.Encompass.BusinessObjects.DataObject customDataObject = EncompassApplication.Session.DataExchange.GetCustomDataObject("e360TrayConfig.xml");
            string xml = Encoding.UTF8.GetString(customDataObject.Data);
            return xml;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtXMLContent.Text = getTextFromEncompass();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EllieMae.Encompass.BusinessObjects.DataObject customDataObject = new EllieMae.Encompass.BusinessObjects.DataObject();
            customDataObject.Load(Encoding.ASCII.GetBytes(txtXMLContent.Text));
            EncompassApplication.Session.DataExchange.SaveCustomDataObject("e360TrayConfig.xml", customDataObject);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
