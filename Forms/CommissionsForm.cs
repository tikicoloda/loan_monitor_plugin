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
using CompanyName.BusinessEntities;

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
                cmbCompanyNameStatements.DataSource = emptyStatementList().DefaultView;

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

                cmbCompanyNameStatements.ValueMember = "payPeriod";

                cmbCompanyNameStatements.DisplayMember = "payPeriodName";

                cmbCompanyNameStatements.DataSource = dt.DefaultView;

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



                cmbCompanyNameDelegate.ValueMember = "Id";

                cmbCompanyNameDelegate.DisplayMember = "FullName";
                cmbCompanyNameDelegate.DataSource = delegateDt.DefaultView;
                if (cmbCompanyNameDelegate.Items.Count > 0)
                {
                    cmbCompanyNameDelegate.SelectedItem = cmbCompanyNameDelegate.Items[cmbCompanyNameDelegate.Items.Count - 1];
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

        private void cmbCompanyNameDelegates_SelectedIndexChanged(object sender, EventArgs e)
        {


            //populateStatements();
        }

        private void cmbCompanyNameStatements_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (cmbCompanyNameStatements.SelectedValue.ToString() != "")
                {
                    {
                        // MessageBox.Show(cmbCompanyNameDelegate.SelectedValue.ToString());

                        AdminClient adminClient = UserConfigHelper.getAdminServiceClient();

                        string cipherStatementId = HttpUtility.UrlEncode(adminClient.EncryptData(Convert.ToString(cmbCompanyNameStatements.SelectedValue)));
                        string cipherDelegateId = HttpUtility.UrlEncode(adminClient.EncryptData(Convert.ToString(cmbCompanyNameDelegate.SelectedValue)));
                        string isAdmin = HttpUtility.UrlEncode(adminClient.EncryptData(isCurrentUserAdminForCommisions));
                        
                        string url = string.Empty;
                        url = String.Format("{0}?periodid={1}&employeeid={2}&isadmin={3}", UserConfigHelper.getEncompassCoronaUrl(),
                            cipherStatementId, cipherDelegateId, isAdmin);

                        //url = String.Format("{0}?periodid={1}&employeeid={2}&isadmin={3}", UserConfigHelper.getEncompassCoronaUrl(),
                        //    cmbCompanyNameStatements.SelectedValue.ToString(), cmbCompanyNameDelegate.SelectedValue.ToString(), isCurrentUserAdminForCommisions);

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
