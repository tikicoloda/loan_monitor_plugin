using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Collections;

namespace LoanMonitorPlugin.Helpers
{
    public static class ConfigHelper
    {
        //TODO: We may want to pull the config locally upon login to prevent read contention.
        public static StringList loadAttributeListFromConfig(string xpath, string attribute)
        {
            StringList sl = new StringList();
            try
            {
                EllieMae.Encompass.BusinessObjects.DataObject customDataObject = EncompassApplication.Session.DataExchange.GetCustomDataObject("e360TrayConfig.xml");
                string xml = Encoding.ASCII.GetString(customDataObject.Data);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                foreach (XmlElement child in xmlDocument.SelectNodes(xpath))
                {
                    sl.Add(child.GetAttribute(attribute));
                }
            }
            catch (Exception)
            {
                //do nothing
            }
            return sl;
        }
    }
}
