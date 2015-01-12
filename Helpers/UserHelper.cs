using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Users;

namespace LoanMonitorPlugin.Helpers
{
    public static class UserHelper
    {
        public static bool hasPermission(string[] personas)
        {
            bool hasPermission = false;

            foreach (Persona p in EncompassApplication.CurrentUser.Personas)
            {
                foreach (string persona in personas)
                {
                    if (persona == p.Name)
                    {
                        hasPermission = true;
                        break;
                    }
                }
            }

            return hasPermission;
        }
    }
}
