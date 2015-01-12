using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using EllieMae.Encompass.Automation;
using System.ServiceModel;
using System.IO;


namespace LoanMonitorPlugin.Helpers
{
    public static class UserConfigHelper
    {
        public static bool shouldDisplayWelcome()
        {
            bool displayWelcome = true;
            try
            {
                XmlDocument xmlDocument = GetUserConfig();

                foreach (XmlElement child in xmlDocument.SelectNodes("e360TrayUserConfig/settings/displayWelcome"))
                {
                    string value = child.GetAttribute("value");
                    if (value == "N")
                        displayWelcome = false;
                }
            }
            catch (Exception)
            {
                //do nothing
            }
            return displayWelcome;
        }

        public static void setDisplayWelcome(string option)
        {
            try
            {
                XmlDocument xmlDocument = GetUserConfig();
                foreach (XmlElement child in xmlDocument.SelectNodes("e360TrayUserConfig/settings/displayWelcome"))
                {
                    child.SetAttribute("value", option);
                }

                EllieMae.Encompass.BusinessObjects.DataObject dataObject = new EllieMae.Encompass.BusinessObjects.DataObject(Encoding.ASCII.GetBytes(xmlDocument.OuterXml));
                EncompassApplication.CurrentUser.SaveCustomDataObject("e360TrayUserConfig.xml", dataObject);
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        private static XmlDocument GetUserConfig()
        {
            XmlDocument xmlDocument = new XmlDocument();
            string xml;
            try
            {
                EllieMae.Encompass.BusinessObjects.DataObject customDataObject = EncompassApplication.CurrentUser.GetCustomDataObject("e360TrayUserConfig.xml");
                xml = Encoding.ASCII.GetString(customDataObject.Data);
                xmlDocument.LoadXml(xml);
            }
            catch (Exception)
            {
                xml = createUserConfigFile();
                xmlDocument.LoadXml(xml);
            }

            return xmlDocument;
        }

        private static string createUserConfigFile()
        {
            string configXML = "<e360TrayUserConfig><settings><displayWelcome value='Y'/></settings></e360TrayUserConfig>";
            EllieMae.Encompass.BusinessObjects.DataObject dataObject = new EllieMae.Encompass.BusinessObjects.DataObject(Encoding.ASCII.GetBytes(configXML));
            EncompassApplication.CurrentUser.SaveCustomDataObject("e360TrayUserConfig.xml", dataObject);
            return configXML;
        }

        public static string getEncompassCoronaUrl()
        {

            string encompassCoronaUrl = string.Empty;
            string encompassConfigText=string.Empty;

            try
            {

                XmlDocument doc = new XmlDocument();
                encompassConfigText = getE360TrayConfigTextFromEncompass().Trim().Replace("???", "");//Need to explore why ??? is coming at the start of the config string
                doc.LoadXml(encompassConfigText);


                XmlElement root = doc.DocumentElement;
                XmlNode node = root.SelectSingleNode("//EncompassCoronaUrl");

                if (node != null)
                {
                    encompassCoronaUrl = node.Attributes["name"].Value.ToString();
                }
                else
                {
                    throw new Exception("EncompassCorona Url not specified");
                }

            }
            catch (Exception ex)
            {
                string exceptionMessage = String.Empty;
                exceptionMessage = ex.Message + Environment.NewLine + encompassConfigText;
                throw new Exception(exceptionMessage);

            }
            

            return encompassCoronaUrl;
        }

        private static string getCommissionsServiceURI()
        {
            string serviceURI = string.Empty;
            serviceURI = getServiceHost() + @"/E360/Commissions.svc/commissions";
            return serviceURI;

        }

        private static string getAdminServiceURI()
        {
            string serviceURI = string.Empty;
            serviceURI = getServiceHost() + @"/E360/Admin.svc/admin";
            return serviceURI;
        }


        private static string getE360TrayConfigTextFromEncompass()
        {
            EllieMae.Encompass.BusinessObjects.DataObject customDataObject = EncompassApplication.Session.DataExchange.GetCustomDataObject("e360TrayConfig.xml");
            string xml = Encoding.ASCII.GetString(customDataObject.Data); //Encoding.UTF8.GetString(customDataObject.Data);
            return xml;
        }

      

        private static string getServiceHost()
        {

            string serviceHost = string.Empty;
            string encompassConfigText = String.Empty;
           

            try
            {
              
                XmlDocument doc = new XmlDocument();
                encompassConfigText = getE360TrayConfigTextFromEncompass().Trim().Replace("???", "");//Need to explore why ??? is coming at the start of the config string
                doc.LoadXml(encompassConfigText);

                XmlElement root = doc.DocumentElement;
                XmlNode node = root.SelectSingleNode("//ServiceHostedServerName");

                if (node != null)
                {
                    serviceHost = node.Attributes["name"].Value.ToString();
                }
                else
                {
                    throw new Exception("Service Host Server not specified");
                }

               
            }
            catch (Exception ex)
            {
                string exceptionMessage = String.Empty;
                exceptionMessage = ex.Message + Environment.NewLine + encompassConfigText;
                throw new Exception(exceptionMessage);

            }

            return serviceHost;
        }

        public static AdminClient getAdminServiceClient()
        {
            string address = getAdminServiceURI();
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpointAddress = new EndpointAddress(address);
            AdminClient client = new AdminClient(binding, endpointAddress);
            return client;
        }

        
        public static CommissionsClient getCommissionServiceClient(){

            string address = getCommissionsServiceURI();// "http://dmdws002:8001/E360/Commissions.svc/commissions";
                System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                EndpointAddress endpointAddress = new EndpointAddress(address);

                CommissionsClient client = new CommissionsClient(binding, endpointAddress);
                return client;
    }
    }
}
