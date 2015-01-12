using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Query;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.BusinessObjects.Loans;
using LoanMonitorPlugin.Helpers;

namespace LoanMonitorPlugin.Forms
{
    public partial class BusinessContactLookup : Form
    {
        public BusinessContactLookup(string abaNumber, string accountNumber)
        {
            InitializeComponent();
            buildDataGrid();
            getContacts("Closing Agent", abaNumber, accountNumber);

            this.Shown += new EventHandler(MyForm_CloseOnStart);
        }

        private void MyForm_CloseOnStart(object sender, EventArgs e)
        {
            if (dgvBusinessContacts.Rows.Count <= 0)
            {
                MessageBox.Show("No Closing Agents found.");
                this.Close();
            }
        }

        private void buildDataGrid()
        {
            /*
            dgvBusinessContacts.Columns.Add("id", "ID");
            dgvBusinessContacts.Columns.Add("companyName", "Company Name");
            dgvBusinessContacts.Columns.Add("abaNumber", "ABA Number");
            dgvBusinessContacts.Columns.Add("accountNumber", "Account Number");
            dgvBusinessContacts.Columns.Add("caStatus", "Closing Agent Status");
            dgvBusinessContacts.Columns.Add("titleInsurerName", "Title Insurer");
            dgvBusinessContacts.Columns.Add("titleInsurerStatus", "Title Insurer Status");
            */
            dgvBusinessContacts.Columns.Add("id", "ID");
            dgvBusinessContacts.Columns.Add("closingAgentName", "Closing Agent Name");
            dgvBusinessContacts.Columns.Add("closingAgentAddress", "Closing Agent Address");
            dgvBusinessContacts.Columns.Add("closingAgentStatus", "Closing Agent Status");
            dgvBusinessContacts.Columns.Add("titleInsurerName", "Title Insurer Name");
            dgvBusinessContacts.Columns.Add("closingAgentApprovalExpirationDate", "Closing Agent Approval Expiration Date");
            dgvBusinessContacts.Columns.Add("secondBankName", "Second Bank Name");
            dgvBusinessContacts.Columns.Add("secondBankCity", "Second Bank City");
            dgvBusinessContacts.Columns.Add("secondBankState", "Second Bank State");
            dgvBusinessContacts.Columns.Add("secondBankABA", "Second Bank ABA");
            dgvBusinessContacts.Columns.Add("secondBankAccountNumber", "Second Bank Account Number");
        }

        private void addRow(string[] rowContents)
        {
            dgvBusinessContacts.Rows.Add(rowContents);
            dgvBusinessContacts.Columns["id"].Visible = false;
        }

        private void getContacts(string bizCategory, string abaNumber, string accountNumber)
        {
            NumericFieldCriterion catCri = new NumericFieldCriterion();
            catCri.FieldName = "Contact.CategoryID";
            catCri.Value = EncompassApplication.Session.Contacts.BizCategories.GetItemByName(bizCategory).ID;

            StringFieldCriterion abaCri = new StringFieldCriterion();
            //abaCri.FieldName = "Primary ABA";
            //abaCri.FieldName = "Custom.Primary ABA";
            abaCri.FieldName = "CustomCategory.Closing Agent.Primary ABA";
            abaCri.Value = abaNumber;
            abaCri.Include = true;

            StringFieldCriterion acctCri = new StringFieldCriterion();
            acctCri.FieldName = "CustomCategory.Closing Agent.Beneficiary Account Number";
            acctCri.Value = accountNumber;
            acctCri.Include = true;

            QueryCriterion criterion = catCri.And(abaCri).And(acctCri);

            ContactList contacts = EncompassApplication.Session.Contacts.Query(criterion, ContactLoanMatchType.None, ContactType.Biz);
            //ContactList contacts = EncompassApplication.Session.Contacts.GetAll(ContactType.Biz);

            foreach (BizContact biz in contacts)
            {
                ContactCustomFields custFields = biz.BizCategoryCustomFields["Closing Agent"];
                string aba = "";
                string acct = "";
                string status = "";
                string titleInsurerName = "";
                string titleInsurerStatus = "";
                string expDate = "";
                string secondBankName = "";
                string secondBankCity = "";
                string secondBankState = "";
                string secondBankABA = "";
                string secondBankAcct = "";

                foreach (ContactCustomField custField in custFields)
                {
                    if (custField.Name == "Primary ABA")
                        aba = custField.Value;
                    if (custField.Name == "Beneficiary Account Number")
                        acct = custField.Value;
                    if (custField.Name == "Closing Agent Status")
                        status = custField.Value;
                    if (custField.Name == "Title Insurer Name")
                        titleInsurerName = custField.Value;
                    if (custField.Name == "Title Insurer Status")
                        titleInsurerStatus = custField.Value;
                    if (custField.Name == "Closing Agent Approval Expiration Date")
                        expDate = custField.Value;
                    if (custField.Name == "Second Bank Name")
                        secondBankName = custField.Value;
                    if (custField.Name == "Second Bank City")
                        secondBankCity = custField.Value;
                    if (custField.Name == "Second Bank State")
                        secondBankState = custField.Value;
                    if (custField.Name == "Second Bank ABA")
                        secondBankABA = custField.Value;
                    if (custField.Name == "Second Bank Account Number")
                        secondBankAcct = custField.Value;
                }
                //if (abaNumber == aba && accountNumber == acct)
                //{
                    //addRow(new string[] { biz.ID.ToString(), biz.CompanyName, aba, acct, status, titleInsurer, titleInsurerStatus });
                    addRow(new string[] { biz.ID.ToString(), biz.CompanyName, biz.BizAddress.Street1, status, titleInsurerName, expDate,
                                          secondBankName, secondBankCity, secondBankState, secondBankABA, secondBankAcct });
                //}
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (isSelectionValid())
            {
                try
                {
                    string selectedContactId = getSelectedValue("id");
                    LoanHelper.applySelectedContactId(selectedContactId);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private bool isSelectionValid()
        {
            bool isSelectionValid = false;
            if (dgvBusinessContacts.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please select a valid Closing Agent.");
                return isSelectionValid;
            }

            string titleInsurerStatus = getSelectedValue("closingAgentStatus");
            if (titleInsurerStatus == "Invalid" || titleInsurerStatus == "Rejected")
            {
                MessageBox.Show("The selected Closing Agent is in status '" + titleInsurerStatus + "'. Please select a valid Closing Agent.");
                return isSelectionValid;
            }

            isSelectionValid = true;
            return isSelectionValid;
        }

        private string getSelectedValue(string columnName)
        {
            DataGridViewRow row = dgvBusinessContacts.SelectedRows[0];
            string selectedValue = (string)row.Cells[columnName].Value;
            return selectedValue;
        }


    }
}
