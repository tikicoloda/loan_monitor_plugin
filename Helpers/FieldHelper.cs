
namespace LoanMonitorPlugin.Helpers
{
    public static class FieldHelper
    {
        #region Funding Fields
        
        public const string FundingNotification = "CX.FUNDING.NOTIFICATIONS";
        public const string FundingNotificationStatus = "CX.ORDER.FUNDS.STATUS";
        public const string RescissionDate = "L724";
        public const string DisbursementDate = "2553";
        public const string PropertyState = "14";
        public const string LoanPurpose = "19";
        public const string PropertyUsage = "1811";
        public const string ClosingDate = "748";
        public const string FundsOrderedDate = "1996";
        public const string WireReleaseDate = "CX.WIRE.RELEASE.DATE";
        public const string TitleCompanyName = "411";
        public const string ABANumber = "VEND.X398";
        public const string AcctNumber = "VEND.X399";
        public const string ForCreditTo = "2007";
        public const string WireTransferAmount = "1990";
        public const string FundingDate = "2553";
        public const string FunderName = "1991";
        public const string DitechInterfaceFlag = "CX.DITECH_INTERFACE";
        public const string BankReject = "CX.BANKREJECT";

        #endregion Funding Fields

        #region CAV Fields

        public const string ClosingAgentName = "411";
        public const string ClosingAgentAddress = "412";
        public const string ClosingAgentCity = "413";
        public const string ClosingAgentState = "1174";
        public const string ClosingAgentZip = "414";
        public const string ClosingAgentContactName = "416";
        public const string ClosingAgentPhone = "417";
        public const string ClosingAgentFax = "1243";
        public const string ClosingAgentPrimaryABA = "VEND.X398";
        public const string ClosingAgentPrimaryAcct = "VEND.X399";
        public const string ClosingAgentBeneficiaryAccountHolderName = "2007";
        public const string ClosingAgentSecondBankName = "CX.INTERMEDIATE.BANK.NAME";
        public const string ClosingAgentSecondBankRouting = "CX.INTERMEDIATE.BANK.ROUTING";
        public const string ClosingAgentStatus = "CX.CAV.CLOSING.AGENT.STATUS";
        public const string ClosingAgentStatusDate = "CX.CAV.CASTATUSDATE";
        public const string ClosingAgentStatusExpirationDate = "CX.CAV.CLOSING.AGENT.EXP.DAT";
        public const string ClosingAgentTitleInsurerName = "CX.CAV.TITLEINSURER";
        public const string ClosingAgentBeneficiaryBankName = "CX.CAV.BANK.NAME";
        //public const string ClosingAgentWireDetails = "CX.CAV.WIREDETAILS";
        public const string FundingInstructions = "CX.FUNDING.INSTRUCTIONS";
        public const string ClosingAgentSequenceId = "CX.CAV.CONTACTSEQID";
        public const string CAVRefreshDate = "CX.CAVREFRESHDATE";

        #endregion CAV Fields

        #region Contact Custom Fields

        public const string ContactCABeneficiaryAccountHolderName = "Beneficiary Acct Holder Name";
        public const string ContactCASecondBankName = "Second Bank Name";
        public const string ContactCASecondBankRouting = "Second Bank ABA";
        public const string ContactCASecondBankAccount = "Second Bank Account Number";
        public const string ContactCAStatus = "Closing Agent Status";
        public const string ContactCAStatusDate = "Closing Agent Status Date";
        public const string ContactCAStatusExpirationDate = "Closing Agent Approval Expiration Date";
        public const string ContactCATitleInsurerName = "Title Insurer Name";
        public const string ContactCABeneficiaryBankName = "Beneficiary Bank Name";
        public const string ContactCAPrimaryABA = "Primary ABA";
        public const string ContactCABeneficiaryAccountNumber = "Beneficiary Account Number";

        #endregion Contact Custom Fields

        #region CLI Fields

        public const string CurrentStatus = "1393";
        public const string RateIsLocked = "2400";
        public const string GFEApplicationDate = "3142";
        public const string LoanPlanProductCode = "3041";

        #endregion CLI Fields

        #region Imaging Fields
        public const string ServicerLoanNumber = "SERVICE.X108";//It is mentioned as SERVICE.X108 in BRD
                        
        #endregion Imaging Fields
    }
}
