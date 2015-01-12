using System;
using EllieMae.Encompass.Automation;
using LoanMonitorPlugin.Enums;
using EllieMae.Encompass.Configuration;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Query;

namespace LoanMonitorPlugin.Helpers
{
    public static class LoanHelper
    {
        public static void setLockedField(string fieldName, string fieldValue)
        {
            bool isLocked = EncompassApplication.CurrentLoan.Fields[fieldName].Locked;
            if (isLocked)
                EncompassApplication.CurrentLoan.Fields[fieldName].Locked = false;

            EncompassApplication.CurrentLoan.Fields[fieldName].Value = fieldValue;

            if (isLocked)
                EncompassApplication.CurrentLoan.Fields[fieldName].Locked = true;
        }

        public static void setFieldNoRules(string fieldName, string fieldValue)
        {
            Macro.SetFieldNoRules(fieldName, fieldValue);
        }

        public static void setNotification(string status, string message)
        {
            string fullMessage = DateTime.Now.ToString() + "\t" + status + System.Environment.NewLine + message;
            string currentValue = (string)EncompassApplication.CurrentLoan.Fields[FieldHelper.FundingNotification].Value;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.FundingNotification].Value = currentValue + System.Environment.NewLine + fullMessage;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.FundingNotificationStatus].Value = status;
        }

        public static void setInterfaceFlag(InterfaceFlagType interfaceType)
        {
            int? interfaceFlag = (int?)EncompassApplication.CurrentLoan.Fields[FieldHelper.DitechInterfaceFlag].Value ?? 0;
            bool isAlreadyIncluded = (interfaceFlag & (int)interfaceType) != 0;
            if (!isAlreadyIncluded)
            {
                EncompassApplication.CurrentLoan.Fields[FieldHelper.DitechInterfaceFlag].Value = interfaceFlag | (int)interfaceType;
            }
        }

        public static void resetInterfaceFlag(InterfaceFlagType interfaceType)
        {
            int? interfaceFlag = (int?)EncompassApplication.CurrentLoan.Fields[FieldHelper.DitechInterfaceFlag].Value ?? 0;
            bool isAlreadyIncluded = (interfaceFlag & (int)interfaceType) != 0;
            if (isAlreadyIncluded)
            {
                EncompassApplication.CurrentLoan.Fields[FieldHelper.DitechInterfaceFlag].Value = interfaceFlag ^ (int)interfaceType;
            }
        }

        public static bool isLoanInterfacesEligible()
        {
            //Loans in the Testing_Training folder are not eligible for interfaces
            bool isLoanInterfacesEligible = true;
            string currentFolder = EncompassApplication.CurrentLoan.LoanFolder;

            if (currentFolder == "Testing_Training")
            {
                isLoanInterfacesEligible = false;
            }
            return isLoanInterfacesEligible;
        }

        public static void calculateRescissionDate()
        {
            try
            {
                //calculate the rescission date
                string propertyState = (string)EncompassApplication.CurrentLoan.Fields[FieldHelper.PropertyState].Value;
                string loanPurpose = (string)EncompassApplication.CurrentLoan.Fields[FieldHelper.LoanPurpose].Value;
                string propertyUsage = (string)EncompassApplication.CurrentLoan.Fields[FieldHelper.PropertyUsage].Value;
                DateTime closingDate = (DateTime)EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingDate].Value;

                if (!EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingDate].IsEmpty() &&
                    isRefinance(loanPurpose) &&
                    isPrimaryResidence(propertyUsage))
                {
                    BusinessCalendar busCalendar = EncompassApplication.Session.SystemSettings.GetBusinessCalendar(EllieMae.Encompass.Configuration.BusinessCalendarType.Postal);
                    DateTime rescissionDate = busCalendar.AddBusinessDays(closingDate, 3, true);

                    //Removing custom holiday list
                    //if (isCustomHolidayWithinSpan(propertyState, closingDate, rescissionDate))
                    //{
                    //    rescissionDate = rescissionDate.AddDays(1);
                    //}

                    EncompassApplication.CurrentLoan.Fields[FieldHelper.RescissionDate].Value = rescissionDate;
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.DisbursementDate].Value = busCalendar.AddBusinessDays(rescissionDate, 1, true);
                }
                else
                {
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.RescissionDate].Value = "";
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.DisbursementDate].Value = closingDate;
                }
            }
            catch (Exception ex)
            {
                //TODO: handle exception
            }
        }

        private static bool isCustomHolidayWithinSpan(string state, DateTime minDate, DateTime maxDate)
        {
            StringList customHolidays = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/CustomHolidays/" + state, "date");
            bool isDateWithinSpan = false;

            foreach (string holiday in customHolidays)
            {
                DateTime dateToCheck = DateTime.Parse(holiday);

                if (minDate <= dateToCheck && dateToCheck <= maxDate)
                {
                    isDateWithinSpan = true;
                }
            }
            return isDateWithinSpan;
        }

        private static bool isRefinance(string input)
        {
            bool isRefinance = false;
            if (input == "NoCash-Out Refinance" || input == "Cash-Out Refinance")
            {
                isRefinance = true;
            }
            return isRefinance;
        }

        private static bool isPrimaryResidence(string input)
        {
            bool isPrimaryResidence = false;
            if (input == "PrimaryResidence")
            {
                isPrimaryResidence = true;
            }
            return isPrimaryResidence;
        }

        public static void doRefreshCAV()
        {
            //get the contactId of the selected record based on the ClosingAgentSequenceId field
            if (!EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSequenceId].IsEmpty())
            {
                string selectedSeqId = (string)EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSequenceId].Value;
                string selectedContactId = lookupContact("Contact.LastName", selectedSeqId);
                applySelectedContactId(selectedContactId);
            }
        }

        private static string lookupContact(string searchField, string searchValue)
        {
            string retValue = "";

            StringFieldCriterion searchCriteria = new StringFieldCriterion();
            searchCriteria.FieldName = searchField;
            searchCriteria.Value = searchValue;
            searchCriteria.Include = true;

            ContactList contacts = EncompassApplication.Session.Contacts.Query(searchCriteria, ContactLoanMatchType.None, ContactType.Biz);

            foreach (BizContact biz in contacts)
            {
                retValue = biz.ID.ToString();
            }

            return retValue;
        }

        public static void applySelectedContactId(string selectedContactId)
        {
            BizContact contact = (BizContact)EncompassApplication.Session.Contacts.Open(Convert.ToInt32(selectedContactId), ContactType.Biz);
            //LoanContactRelationshipType relType = LoanContactRelationshipType.CustomCategory2;
            //EncompassApplication.CurrentLoan.Contacts.LinkToBizContact(contact, relType);

            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentName].Value = contact.CompanyName;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentContactName].Value = contact.FirstName;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentAddress].Value = contact.BizAddress.Street1 + " " + contact.BizAddress.Street2;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentCity].Value = contact.BizAddress.City;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentState].Value = contact.BizAddress.State;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentZip].Value = contact.BizAddress.Zip;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentPhone].Value = contact.WorkPhone;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentFax].Value = contact.FaxNumber;
            EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSequenceId].Value = contact.LastName;
            
            ContactCustomFields custFields = contact.BizCategoryCustomFields["Closing Agent"];

            string secondBankName = "";
            string secondBankAccount = "";
            string secondBankABA = "";

            foreach (ContactCustomField custField in custFields)
            {
                if (custField.Name == FieldHelper.ContactCABeneficiaryAccountHolderName)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentBeneficiaryAccountHolderName].Value = custField.Value;
                if (custField.Name == FieldHelper.ContactCAStatus)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentStatus].Value = custField.Value;
                if (custField.Name == FieldHelper.ContactCAStatusDate)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentStatusDate].Value = custField.Value;
                if (custField.Name == FieldHelper.ContactCAStatusExpirationDate)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentStatusExpirationDate].Value = custField.Value;
                if (custField.Name == FieldHelper.ContactCATitleInsurerName)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentTitleInsurerName].Value = custField.Value;
                if (custField.Name == FieldHelper.ContactCABeneficiaryBankName)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentBeneficiaryBankName].Value = custField.Value;

                if (custField.Name == FieldHelper.ContactCAPrimaryABA)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentPrimaryABA].Value = custField.Value;
                if (custField.Name == FieldHelper.ContactCABeneficiaryAccountNumber)
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentPrimaryAcct].Value = custField.Value;

                if (custField.Name == FieldHelper.ContactCASecondBankName)
                    //EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankName].Value = custField.Value;
                    secondBankName = custField.Value;
                if (custField.Name == FieldHelper.ContactCASecondBankRouting)
                    //EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankRouting].Value = custField.Value;
                    secondBankABA = custField.Value;
                if (custField.Name == FieldHelper.ContactCASecondBankAccount)
                    //EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankAccount].Value = custField.Value;
                    secondBankAccount = custField.Value;
            }

            setSecondBankInfo(secondBankName, secondBankABA, secondBankAccount);
        }

        private static void setSecondBankInfo(string secondBankName, string secondBankABA, string secondBankAccount)
        {
            if (secondBankName != "NULL")
            {
                if (secondBankABA != "NULL")
                {
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankName].Value = secondBankName;
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankRouting].Value = secondBankABA;
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.FundingInstructions].Value = "";
                }

                if (secondBankAccount != "NULL")
                {
                    string wireMessage = "FFC:" + secondBankName + secondBankAccount;
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankName].Value = "";
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankRouting].Value = "";
                    EncompassApplication.CurrentLoan.Fields[FieldHelper.FundingInstructions].Value = wireMessage;
                }
            }
            else
            {
                EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankName].Value = "";
                EncompassApplication.CurrentLoan.Fields[FieldHelper.ClosingAgentSecondBankRouting].Value = "";
                EncompassApplication.CurrentLoan.Fields[FieldHelper.FundingInstructions].Value = "";
            }
        }

        public static string GetLoanGuid(string loanNumber){

            string loanGuid=String.Empty;

            StringFieldCriterion cri = new StringFieldCriterion();
            cri.FieldName = "Loan.LoanNumber";
            cri.Value = loanNumber;
            LoanIdentityList list = EncompassApplication.Session.Loans.Query(cri);
            if (list != null && list.Count > 0)
            {
                loanGuid = list[0].Guid;
            }

            return loanGuid;


        }
    }
}
