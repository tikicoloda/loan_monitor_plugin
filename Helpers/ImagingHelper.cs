
using LoanMonitorPlugin.Enums;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
namespace LoanMonitorPlugin.Helpers
{
    public static class ImagingHelper
    {
        public static void doImaging()
        {

            //Check if loan interfaces are allowed
            //Test loans are not allowed
            if (LoanHelper.isLoanInterfacesEligible())
            {

                //Check if Imaging interface is allowed

                if (isImagingAllowed())
                {

                    LoanHelper.setInterfaceFlag(InterfaceFlagType.Imaging);

                }

            }

        }




        private static bool isImagingAllowed()
        {

            //TODO: flesh out with requirements when available

            bool isImagingAllowed = false;
            //Should return true when loan funded milestone is complete. is this the way to check? 
            //have checked the value of Current Status 

            LogMilestoneEvents msEvents =EncompassApplication.CurrentLoan.Log.MilestoneEvents;
            MilestoneEvent ms = msEvents.GetEventForMilestone("Funding");
            if (ms.Completed)
            {
                if ((string)EncompassApplication.CurrentLoan.Fields[FieldHelper.ServicerLoanNumber].Value != "")
                {
                    isImagingAllowed = true;
                }

            }

          
            //string currentStatus = (string)EncompassApplication.CurrentLoan.Fields[FieldHelper.CurrentStatus].Value;

            //if (currentStatus.Equals("Loan Originated"))
            //{
            //    //The condition is that GreenTree Servicer Loan number should be there.
            //    if ((string)EncompassApplication.CurrentLoan.Fields[FieldHelper.ServicerLoanNumber].Value != "")
            //    {
            //        isImagingAllowed = true;
            //    }
            //}
            
          


            return isImagingAllowed;

        }

       

        
    }
}

