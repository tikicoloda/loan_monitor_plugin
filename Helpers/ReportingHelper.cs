
using LoanMonitorPlugin.Enums;
using System;
namespace LoanMonitorPlugin.Helpers
{
    public static class ReportingHelper
    {
        public static void doReporting()
        {
            try
            {
                if (LoanHelper.isLoanInterfacesEligible())
                {
                    LoanHelper.setInterfaceFlag(InterfaceFlagType.Reporting);
                }
            }
            catch (Exception ex)
            {
                //TODO: handle exception
            }
        }
    }
}
