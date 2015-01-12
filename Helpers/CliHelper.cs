
using LoanMonitorPlugin.Enums;
using EllieMae.Encompass.Automation;
using System;
namespace LoanMonitorPlugin.Helpers
{
    public static class CliHelper
    {
        public static void doCLI()
        {
            try
            {
                if (LoanHelper.isLoanInterfacesEligible())
                {
                    //Check if CLI interface is required
                    if (isCLIAllowed())
                    {
                        LoanHelper.setInterfaceFlag(InterfaceFlagType.CLI);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: handle exception
            }
        }

        /*
         * 
        */
        private static bool isCLIAllowed()
        {
            //  Per Requirements:
            //    Start :: Condition for Registered is satisfied
            //    End   :: Condition for either Funded or Cancelled is satisfied
            bool isCLIAllowed = false;
            TransactionType currentTransactionType = getTransactionType();

            if (currentTransactionType > 0)
            {
                if (!(EncompassApplication.CurrentLoan.Fields[FieldHelper.LoanPlanProductCode].IsEmpty()))
                {
                    isCLIAllowed = true;
                }
            }

            return isCLIAllowed;
        }

        private static TransactionType getTransactionType()
        {
            string currentStatus = (string)EncompassApplication.CurrentLoan.Fields[FieldHelper.CurrentStatus].Value;

            if (currentStatus.Equals("Loan Originated"))
            {
                return TransactionType.Funded;
            }
            else
            {
                if ((currentStatus.Equals("Application approved but not accepted") ||
                      currentStatus.Equals("Application Denied") ||
                      currentStatus.Equals("Application withdrawn") ||
                      currentStatus.Equals("File Closed for Incompleteness") ||
                      currentStatus.Equals("Preapproval request denied by financial institution") ||
                      currentStatus.Equals("Preapproval request approved but not accepted")) &&
                    !(EncompassApplication.CurrentLoan.Fields[FieldHelper.GFEApplicationDate].IsEmpty())
                    )
                {
                    return TransactionType.Cancelled;
                }
                else
                {
                    if (!(EncompassApplication.CurrentLoan.Fields[FieldHelper.RateIsLocked].IsEmpty()))
                    {
                        return TransactionType.Locked;
                    }
                    else
                    {
                        if (!(EncompassApplication.CurrentLoan.Fields[FieldHelper.GFEApplicationDate].IsEmpty()))
                        {
                            return TransactionType.Registered;
                        }
                    }
                }
            }

            return TransactionType.Unregistered;
        }
    }
}
