
using LoanMonitorPlugin.Enums;
using System;
namespace LoanMonitorPlugin.Helpers
{
    public static class CommissionHelper
    {
        public static void doCommission()
        {
            try
            {
                if (LoanHelper.isLoanInterfacesEligible())
                {
                    LoanHelper.setInterfaceFlag(InterfaceFlagType.Commission);
                }
            }
            catch (Exception ex)
            {
                //TODO: handle exception
            }
        }
    }
}
