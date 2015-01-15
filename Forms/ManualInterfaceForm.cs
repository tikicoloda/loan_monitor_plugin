using System;
using System.Windows.Forms;
using System.IO;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.BusinessObjects.Loans;
using LoanMonitorPlugin.Enums;
using LoanMonitorPlugin.Helpers;

namespace LoanMonitorPlugin.Forms
{
    public partial class ManualInterfaceForm : Form
    {
        public ManualInterfaceForm()
        {
            InitializeComponent();

            txtLoans.Text = "Enter Loan GUID(s)...";
            LoadList();
        }

        private void LoadList()
        {
            Type EnumType = typeof(InterfaceFlagType);
            Array Values = System.Enum.GetValues(EnumType);

            foreach (int Value in Values)
            {
                string Display = Enum.GetName(EnumType, Value);
                ListHelper Item = new ListHelper(Display, Value);

                if (Display != "Other")
                {
                    cmbInterfaces.Items.Add(Item);
                    cmbInterfaces.DisplayMember = "LongName";
                    cmbInterfaces.ValueMember = "ShortName";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    //iterate through the loans in txtLoans and update 
                    //the interface flag for the selected cmbInterfaces.
                    ListHelper interfaceFlagType = (ListHelper)cmbInterfaces.SelectedItem;
                    int interfaceFlag = interfaceFlagType.ShortName;

                    string loans = txtLoans.Text;
                    StringReader strReader = new StringReader(loans);
                    string line;
                    while ((line = strReader.ReadLine()) != null)
                    {
                        try
                        {
                            //make the call to update CX.INTERFACE_FLAG
                            StringList fields = new StringList();
                            fields.Add("CX.INTERFACE_FLAG");
                            //StringList result = EncompassApplication.Session.Loans.SelectFields(line, fields);
                            string loanGuid = LoanHelper.GetLoanGuid(line);
                            //MessageBox.Show(loanGuid);
                            StringList result = EncompassApplication.Session.Loans.SelectFields(loanGuid, fields);

                            int? currentFlag;
                            try
                            {
                                currentFlag = (int?)Convert.ToInt32(result[0]) ?? 0;
                            }
                            catch (Exception)
                            {
                                currentFlag = 0;
                            }
                            bool isAlreadyIncluded = (currentFlag & interfaceFlag) != 0;
                            if (!isAlreadyIncluded)
                            {
                                int newValue = (currentFlag.GetValueOrDefault() | interfaceFlag);

                                StringList loanList = new StringList();
                                loanList.Add(line);
                                BatchUpdate batch = new BatchUpdate(loanList);
                                batch.Fields.Add("CX.INTERFACE_FLAG", newValue);
                                EncompassApplication.Session.Loans.SubmitBatchUpdate(batch);
                            }
                        }
                        catch (Exception ex1)
                        {
                            MessageBox.Show(line + " failed with message::" + ex1.Message);
                        }
                    }
                    MessageBox.Show("Completed Processing.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            if (cmbInterfaces.SelectedIndex < 0)
            {
                isValid = false;
                MessageBox.Show("Please select an Interface Type.");
            }
            if (txtLoans.Text == "Enter Loan GUID(s)..." || txtLoans.Text == "")
            {
                isValid = false;
                MessageBox.Show("Please enter loan guid(s).");
            }
            return isValid;
        }
    }


    public class ListHelper
    {
        private int myShortName;
        private string myLongName;

        public ListHelper(string strLongName, int strShortName)
        {
            this.myShortName = strShortName;
            this.myLongName = strLongName;
        }

        public int ShortName
        {
            get
            {
                return myShortName;
            }
        }

        public string LongName
        {
            get
            {
                return myLongName;
            }
        }

    }
}
