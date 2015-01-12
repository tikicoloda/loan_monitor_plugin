using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;
using System.Xml;
using System.Net;
using System.IO;

namespace LoanMonitorPlugin.Forms
{
    public partial class ServicingForm : Form
    {
        public ServicingForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string callResponse = callSwitch();
            webBrowser.DocumentText = callResponse;
        }

        private string callSwitch()
        {
            string response = "";
            try
            {
                string url = getConfigItem("e360TrayConfig/ServicingConfig", "url");
                string username = getConfigItem("e360TrayConfig/ServicingConfig", "username");
                string password = getConfigItem("e360TrayConfig/ServicingConfig", "password");

                HttpWebRequest switchRequest = (HttpWebRequest)WebRequest.Create(url + txtLoanNumber.Text);
                switchRequest.Method = "GET";
                switchRequest.Credentials = new NetworkCredential(username, password);
                WebResponse switchResponse = switchRequest.GetResponse();
                StreamReader sr = new StreamReader(switchResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                response = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }

        private string getConfigItem(string xpath, string attribute)
        {
            String configValue = "";
            try
            {
                EllieMae.Encompass.BusinessObjects.DataObject customDataObject = EncompassApplication.Session.DataExchange.GetCustomDataObject("e360TrayConfig.xml");
                string xml = Encoding.ASCII.GetString(customDataObject.Data);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                foreach (XmlElement child in xmlDocument.SelectNodes(xpath))
                {
                    configValue = child.GetAttribute(attribute);
                }
            }
            catch (Exception)
            {
                //do nothing
            }
            return configValue;
        }
    }
}
