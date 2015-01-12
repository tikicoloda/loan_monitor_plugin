using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.ServiceModel;
using EllieMae.Encompass.Automation;
using LoanMonitorPlugin.Helpers;
using System.Web;
using Ditech.BusinessEntities;

namespace LoanMonitorPlugin.Forms
{
    public partial class CommissionsForm : Form
    {
        public CommissionsForm()
        {
            InitializeComponent();            
            //CurrentUser = "NHAGER";
            CurrentUser = EncompassApplication.CurrentUser.EmployeeID;
            populateDelegates();
            populateStatements();


            isCurrentUserAdminForCommisions = CanCurrentUserAdminCommissions();

        }

        private string CurrentUser;

        private string isCurrentUserAdminForCommisions;

        public string EmployeeId { get; set; }

        DataTable emptyStatementList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("payPeriod");
            dt.Columns.Add("payPeriodName");
            return dt;

        }

        private void populateStatements()
        {
            try
            {
                cmbDitechStatements.DataSource = emptyStatementList().DefaultView;

                //string address = UserConfigHelper.getCommissionsServiceURI();// "http://dmdws002:8001/E360/Commissions.svc/commissions";
                //System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                //EndpointAddress endpointAddress = new EndpointAddress(address);

                CommissionsClient client = UserConfigHelper.getCommissionServiceClient();// new CommissionsClient(binding, endpointAddress);



                StatementPeriod[] response = client.GetStatementList();

                DataTable dt = new DataTable();
                dt.Columns.Add("payPeriod");
                dt.Columns.Add("payPeriodName");

                DataRow newRow;

                foreach (StatementPeriod period in response)
                {

                    newRow = dt.NewRow();
                    newRow[0] = period.PeriodID;
                    newRow[1] = period.PeriodName;
                    dt.Rows.Add(newRow);
                }



                DataRow dr;
                dr = dt.NewRow();
                dt.Rows.InsertAt(dr, 0);

                cmbDitechStatements.ValueMember = "payPeriod";

                cmbDitechStatements.DisplayMember = "payPeriodName";

                cmbDitechStatements.DataSource = dt.DefaultView;

                //if (client.State == CommunicationState.Opened)
                //{
                //    client.Close();
                //}



            }


            catch (FaultException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {

            }

        }

        private void populateDelegates()
        {

            try
            {
                CommissionsClient client = UserConfigHelper.getCommissionServiceClient();// new CommissionsClient(binding, endpointAddress);
                // MessageBox.Show(CurrentUser);
                DelegateUser[] response = client.GetDelegatesDataContract(CurrentUser);

                DataTable delegateDt = new DataTable();
                delegateDt.Columns.Add("Id");
                delegateDt.Columns.Add("FullName");
                foreach (DelegateUser user in response)
                {
                    DataRow dr;
                    dr = delegateDt.NewRow();
                    dr[0] = user.UserID;
                    //MessageBox.Show(user.UserID);
                    //MessageBox.Show(user.DelegateID);

                    dr[1] = user.FirstName + " " + user.LastName;
                    // MessageBox.Show(user.FirstName + " " + user.LastName);
                    delegateDt.Rows.Add(dr);


                }



                cmbDitechDelegate.ValueMember = "Id";

                cmbDitechDelegate.DisplayMember = "FullName";
                cmbDitechDelegate.DataSource = delegateDt.DefaultView;
                if (cmbDitechDelegate.Items.Count > 0)
                {
                    cmbDitechDelegate.SelectedItem = cmbDitechDelegate.Items[cmbDitechDelegate.Items.Count - 1];
                }

                //if (client.State == CommunicationState.Opened)
                //{
                //    client.Close();
                //}



            }


            catch (FaultException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }




        }
        
        private string CanCurrentUserAdminCommissions()
        {

            string returnValue = String.Empty;
            try
            {
                CommissionsClient client = UserConfigHelper.getCommissionServiceClient();
                returnValue = client.CanCurrentUserAdminCommissions(CurrentUser).ToString().ToLower();//EncompassApplication.CurrentUser.EmployeeID).ToString().ToLower();
                //if (client.State == CommunicationState.Opened)
                //{
                //    client.Close();
                //}

            }
            catch (FaultException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return returnValue;


        }

        private void cmbDitechDelegates_SelectedIndexChanged(object sender, EventArgs e)
        {


            //populateStatements();
        }

        private void cmbDitechStatements_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cmbDitechStatements.SelectedValue.ToString() != "")
                {
                    {
                        // MessageBox.Show(cmbDitechDelegate.SelectedValue.ToString());

                        AdminClient adminClient = UserConfigHelper.getAdminServiceClient();

                        string cipherStatementId = HttpUtility.UrlEncode(adminClient.EncryptData(Convert.ToString(cmbDitechStatements.SelectedValue)));
                        string cipherDelegateId = HttpUtility.UrlEncode(adminClient.EncryptData(Convert.ToString(cmbDitechDelegate.SelectedValue)));
                        string isAdmin = HttpUtility.UrlEncode(adminClient.EncryptData(isCurrentUserAdminForCommisions));
                        
                        string url = string.Empty;
                        url = String.Format("{0}?periodid={1}&employeeid={2}&isadmin={3}", UserConfigHelper.getEncompassCoronaUrl(),
                            cipherStatementId, cipherDelegateId, isAdmin);

                        //url = String.Format("{0}?periodid={1}&employeeid={2}&isadmin={3}", UserConfigHelper.getEncompassCoronaUrl(),
                        //    cmbDitechStatements.SelectedValue.ToString(), cmbDitechDelegate.SelectedValue.ToString(), isCurrentUserAdminForCommisions);

                        ProcessStartInfo sInfo = new ProcessStartInfo(url);
                        Process.Start(sInfo);
                    }

                }
            }
            catch (FaultException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




    }
}
