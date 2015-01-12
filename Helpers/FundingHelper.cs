using System;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Configuration;
using LoanMonitorPlugin.Enums;
using EllieMae.Encompass.BusinessEnums;

namespace LoanMonitorPlugin.Helpers
{
    public static class FundingHelper
    {
        public static void doFunding()
        {
            try
            {
                if (FundingHelper.isFundingAllowed())
                {
                    DialogResult proceed = MessageBox.Show("Wire transfer request will be sent to Treasury on the Funds Ordered Date." + Environment.NewLine +
                                "Funds will be disbursed on Wire Release Date. " + Environment.NewLine +
                                "Click \"OK\" to proceed." + Environment.NewLine +
                                "Please save the loan. ", "confirm", MessageBoxButtons.OKCancel);

                    if (proceed == DialogResult.OK)
                    {
                        LoanHelper.setFieldNoRules(FieldHelper.BankReject, "");
                        LoanHelper.setInterfaceFlag(InterfaceFlagType.Treasury);

                        DateTime fundsOrdered = (DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.FundsOrderedDate].Value;
                        DateTime wireRelease = (DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].Value;
                        LoanHelper.setNotification("Ordered", "Funds Order is scheduled to be processed on " + fundsOrdered.ToString("MM/dd/yyyy") + " with Wire Release Date of " + wireRelease.ToString("MM/dd/yyyy") + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: handle exception
            }
        }

        public static void doCancelFunding()
        {
            LoanHelper.setNotification("Cancel", "Order Funds Request cancelled.");
            LoanHelper.resetInterfaceFlag(InterfaceFlagType.Treasury);

            EncompassApplication.CurrentLoan.Fields[FieldHelper.FundsOrderedDate].Value = "";
            EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].Value = "";
        }

        private static bool isFundingAllowed()
        {
            bool isFundingAllowed = true;
            string errorMessage = "";

            //Test Loans
            if (!LoanHelper.isLoanInterfacesEligible())
                errorMessage += "  * Test Loans are not eligible for funding" + Environment.NewLine;

            //Tasks required to be cleared
            StringList tasksToBecleared = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/FundingConfig/TasksToBeCleared/Task", "Name");
            foreach (string taskToBeCleared in tasksToBecleared)
            {
                //LogEntryList tasks = EncompassApplication.CurrentLoan.Log.MilestoneTasks.GetTasksByName(taskToBeCleared);
                MilestoneEvent postClosingMilestoneEvent = EncompassApplication.CurrentLoan.Log.MilestoneEvents.GetEventForMilestone("Post Closing");
                LogEntryList tasks = EncompassApplication.CurrentLoan.Log.MilestoneTasks.GetTasksForMilestone(postClosingMilestoneEvent);
                foreach (MilestoneTask task in tasks)
                {
                    if (!task.Completed)
                    {
                        errorMessage += "  * Task '" + taskToBeCleared + "' must be completed prior to Ordering Funds" + Environment.NewLine;
                    }
                }
            }

            //Company Name	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.TitleCompanyName].IsEmpty())
                errorMessage += "  * Title company name is required" + Environment.NewLine;

            //ABA Number 	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.ABANumber].IsEmpty())
                errorMessage += "  * ABA Number for Title company is required" + Environment.NewLine;

            //Account Number 	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.AcctNumber].IsEmpty())
                errorMessage += "  * Account Number for Title company is required" + Environment.NewLine;

            //Beneficiary Bank Name 	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentBeneficiaryAccountHolderName].IsEmpty())
                errorMessage += "  * Beneficiary Acct Holder Name is required" + Environment.NewLine;

            //Beneficiary Bank Name 	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentBeneficiaryBankName].IsEmpty())
                errorMessage += "  * Beneficiary Bank Name for Title company is required" + Environment.NewLine;

            //Wire Transfer Amount	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.WireTransferAmount].IsEmpty())
                errorMessage += "  * Wire Transfer Amount is required" + Environment.NewLine;

            //Closing Date	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingDate].IsEmpty())
                errorMessage += "  * Closing date is required" + Environment.NewLine;

            //Funding Date	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.FundingDate].IsEmpty())
                errorMessage += "  * Funding date is required" + Environment.NewLine;

            //Funder Name	• Not null
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.FunderName].IsEmpty())
                errorMessage += "  * Funder Name is required" + Environment.NewLine;

            //Funds Ordered Date	• Not null and must be greater than or equal to current date
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.FundsOrderedDate].IsEmpty() ||
                (DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.FundsOrderedDate].Value < DateTime.Today)
                errorMessage += "  * Funds Ordered Date must be greater than or equal to current date" + Environment.NewLine;

            //Closing Agent Status
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentStatus].IsEmpty() ||
                (String)EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentStatus].Value != "Approved")
                errorMessage += "  * Closing Agent must be in 'Approved' status to Order Funds" + Environment.NewLine;
            
            //Refresh Button
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.CAVRefreshDate].IsEmpty() ||
                (DateTime.Parse( (String) EncompassApplication.CurrentLoan.Fields[FieldHelper.CAVRefreshDate].Value) < DateTime.Today))
                errorMessage += "  * Please click the 'Refresh' button to validate the Closing Agent’s Status" + Environment.NewLine;

            //Wire Release Date
            if (!EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].IsEmpty() &&
                ((DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].Value >
                (DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentStatusExpirationDate].Value))
                errorMessage += "  * Wire Release Date cannot be greater than Approval Expiration Date" + Environment.NewLine;

            BusinessCalendar busCalendar = EncompassApplication.Session.SystemSettings.GetBusinessCalendar(EllieMae.Encompass.Configuration.BusinessCalendarType.Company);
            if (EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].IsEmpty() ||
                (DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].Value < DateTime.Today ||
                !busCalendar.IsBusinessDay((DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].Value))
                errorMessage += "  * Wire Release Date must be greater than or equal to current date and it cannot be on weekends, company holidays or Federal holidays" + Environment.NewLine;

            //For Funders Only----------------------
            StringList cutoffTimes = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/FundingConfig/CutoffTime", "value");
            string baseCutoffTime = (string)cutoffTimes.GetItemAt(0);
            DateTime cutoffTime = Convert.ToDateTime(baseCutoffTime);

            if (UserHelper.hasPermission(new string[] { "Funder" }))
            {
                if (!EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].IsEmpty())
                {
                    DateTime wireReleaseDate = (DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.WireReleaseDate].Value;

                    if (wireReleaseDate.Date == DateTime.Now.Date &&
                        DateTime.Now > cutoffTime)
                    {
                        errorMessage += "  * Funds cannot be ordered after " + baseCutoffTime + " PM EST on current date" + Environment.NewLine +
                                        "    Please update Wire Release Date or contact your Funding Manger or Funding Operations group" + Environment.NewLine;
                    }
                }
            }
            //End Wire Release Date

            if (errorMessage != "")
            {
                isFundingAllowed = false;
                MessageBox.Show("Please correct the following error(s):" + Environment.NewLine + errorMessage);
            }

            return isFundingAllowed;
        }
    }
}
