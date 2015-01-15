using System;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Configuration;
using LoanMonitorPlugin.Forms;
using LoanMonitorPlugin.Helpers;
using LoanMonitorPlugin.Enums;
using System.Threading;
using System.IO;
using System.Diagnostics;
using EllieMae.Encompass.BusinessObjects;
using System.Text;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;

namespace LoanMonitorPlugin
{
	[Plugin]
	public class LoanMonitorPlugin
	{
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private StringList cliMonitoredFields;
        private StringList cliMonitoredMilestones;
        private StringList rescissionDateMonitoredFields;
        private StringList imagingMonitoredFields;
        private StringList imagingMilestones;
        private StringList commissionMonitoredFields;

        private string eFolderContents;
       // private DateTime lastAttachmentDate;
        private DateTime savedLoanLastAttachmentDate;
        private int attachmentCount;
        private int savedLoanAttachmentCount;

        #region constructor
        // The public constructor for the plugin. All plugins must have a public, parameterless
		// constructor. In the constructor you should subscribe to the events you wish to
		// handle within Encompass.
		public LoanMonitorPlugin()
		{
            this.cliMonitoredFields = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/MonitoredFields/CLI/field", "id");
            this.cliMonitoredMilestones = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/MonitoredMilestones/CLI/milestone", "id");
            this.rescissionDateMonitoredFields = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/MonitoredFields/RescissionDate/field", "id");
            this.imagingMonitoredFields = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/MonitoredFields/IMAGING/field", "id");
           // this.imagingMilestones = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/MonitoredMilestones/IMAGING/milestone", "id");
            this.commissionMonitoredFields = ConfigHelper.loadAttributeListFromConfig("e360TrayConfig/MonitoredFields/Commission/field", "id");

            try
            {
                EncompassApplication.Login += new EventHandler(Application_Login);
                EncompassApplication.Logout += new EventHandler(Application_Logout);
                EncompassApplication.LoanOpened += new EventHandler(Application_LoanOpened);
                EncompassApplication.LoanClosing += new EventHandler(Application_LoanClosing);
                EncompassApplication.Session.DataExchange.DataReceived += new DataExchangeEventHandler(Application_DataObjectArrived);
                EllieMae.Encompass.Automation.LoansScreen ls = (LoansScreen) EncompassApplication.Screens[EncompassScreen.Loans];
                ls.FormLoaded += new FormChangeEventHandler(Application_FormChange);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}
        #endregion constructor

        #region application events

        // Event handler for when user logs in
        private void Application_Login(object sender, EventArgs e)
        {
            //Tray icon setup
            try
            {
                trayIcon = new NotifyIcon();
                trayIcon.Text = "e360 Taskbar Tool";
                trayIcon.Icon = Resources.TrayIcon;
                trayIcon.Visible = true;


                BuildMenu(false);

                bool isFirstTime = UserConfigHelper.shouldDisplayWelcome();
                if (isFirstTime)
                    RunFirstTime();
            }
            catch (Exception ex)
            {
               // File.AppendAllText(@"C:\Logs\WriteText" + DateTime.Now.ToShortDateString().Replace("/", "") + ".txt", ex.Message);

            }
        }

        private void RunFirstTime()
        {
            trayIcon.ShowBalloonTip(3000, "E360 Taskbar Tool", "Welcome to the e360 Taskbar Tool! Right-click for options.", ToolTipIcon.Info);
            WelcomeForm frm = new WelcomeForm();
            frm.ShowDialog();
        }

        private void Application_LoanOpened(object sender, EventArgs e)
        {
            //rebuild the menu to add loan level items.
            BuildMenu(true);
            eFolderContents = String.Empty;
            //lastAttachmentDate = DateTime.MinValue;
            savedLoanLastAttachmentDate = LatestLoanAttachmentsDate();
            savedLoanAttachmentCount = GetLoanAttachmentsCount();
            //add the loan level listeners
            //EncompassApplication.CurrentLoan.Committed += new EllieMae.Encompass.BusinessObjects.PersistentObjectEventHandler(Application_Commit);
            EncompassApplication.CurrentLoan.FieldChange += new FieldChangeEventHandler(Application_FieldChange);
            EncompassApplication.CurrentLoan.MilestoneCompleted += new MilestoneEventHandler(Application_MilestoneChange);
            EncompassApplication.CurrentLoan.LogEntryAdded += new  LogEntryEventHandler(CurrentLoan_LogEntryAdded);
            EncompassApplication.CurrentLoan.BeforeCommit += new CancelableEventHandler(CurrentLoan_BeforeCommit);
            EncompassApplication.CurrentLoan.Committed +=new PersistentObjectEventHandler(CurrentLoan_Committed);
        }

        //private string GetEFolderContents(){

        //    string contents = String.Empty;
            
        //    EllieMae.Encompass.BusinessObjects.Loans.Loan loan = EncompassApplication.CurrentLoan;
        //    LoanAttachments la = loan.Attachments;
        //    StringBuilder builder = new StringBuilder();
            
        //    foreach (Attachment a in la)
        //    {
        //        //builder.Append(a.Date.ToString());
        //        //builder.Append("|");
        //        //builder.Append(a.IsActive.ToString());
        //        //builder.Append("|");
        //        //builder.Append(a.IsImageAttachment.ToString());
        //        //builder.Append("|");
        //        //builder.Append(a.Name);
        //        //builder.Append("|");
        //        //builder.Append(a.PageImages.Count.ToString());
        //        //builder.Append("|");
        //        //builder.Append(a.Size.ToString());
        //        //builder.Append("|");
        //        //builder.Append(a.Title.ToString());
        //        //builder.Append("~");
        //        if (a.Date > lastAttachmentDate)
        //        {
        //            lastAttachmentDate = a.Date;

        //        }

        //    }

        //    return contents;
        //}

        private int GetLoanAttachmentsCount()
        {
            EllieMae.Encompass.BusinessObjects.Loans.Loan loan = EncompassApplication.CurrentLoan;
            LoanAttachments la = loan.Attachments;

            //LogTrackedDocuments tdoc = EncompassApplication.CurrentLoan.Log.TrackedDocuments;
            //foreach (TrackedDocument doc in tdoc)
            //{
            //    string dateAdded = doc.DateAdded.ToString();

            //    string name = doc.Title;

            //}

            return la.Count;


        }

        private DateTime LatestLoanAttachmentsDate()
        {

            DateTime lastAttachedDate = DateTime.MinValue;

            EllieMae.Encompass.BusinessObjects.Loans.Loan loan = EncompassApplication.CurrentLoan;
            LoanAttachments la = loan.Attachments;


            foreach (Attachment a in la)
            {

                if (a.Date > lastAttachedDate)
                {
                    lastAttachedDate = a.Date;

                }

            }

            return lastAttachedDate;
        }

        

        private void CurrentLoan_BeforeCommit(object sender, CancelableEventArgs e)
        {


        }

        private void CurrentLoan_Committed(object sender, PersistentObjectEventArgs e)
        {

            int attachmentCount = GetLoanAttachmentsCount();
            DateTime lastAttachmentDate = LatestLoanAttachmentsDate();

            if (lastAttachmentDate > savedLoanLastAttachmentDate || attachmentCount!=savedLoanAttachmentCount)
            {
                //Flag for imaging
                ImagingHelper.doImaging();
                savedLoanLastAttachmentDate = lastAttachmentDate;
                savedLoanAttachmentCount = attachmentCount;
            }

           

         

        }

        private void CurrentLoan_LogEntryAdded(object sender, LogEntryEventArgs e)
        {
            //if (e.LogEntry.EntryType.ToString().Equals("TrackedDocument"))  //LogEntry TrackedDocument has a vlaue 4
            //{
            //    ImagingHelper.doImaging();
            //}

        }

        private void Application_LoanClosing(object sender, EventArgs e)
        {
            //rebuild the menu to remove loan level items.
            BuildMenu(false);
            //remove the loan level listeners
            //EncompassApplication.CurrentLoan.Committed -= new EllieMae.Encompass.BusinessObjects.PersistentObjectEventHandler(Application_Commit);
            EncompassApplication.CurrentLoan.FieldChange -= new FieldChangeEventHandler(Application_FieldChange);
            EncompassApplication.CurrentLoan.MilestoneCompleted -= new MilestoneEventHandler(Application_MilestoneChange);
            EncompassApplication.CurrentLoan.LogEntryAdded -= new LogEntryEventHandler(CurrentLoan_LogEntryAdded);
            EncompassApplication.CurrentLoan.BeforeCommit -= new CancelableEventHandler(CurrentLoan_BeforeCommit);
            EncompassApplication.CurrentLoan.Committed -= new PersistentObjectEventHandler(CurrentLoan_Committed);
        }

        private void Application_Logout(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Form f in Application.OpenForms)
            {
                f.Close();
            }
            trayIcon.Dispose();
        }

        private void Application_Commit(object sender, EllieMae.Encompass.BusinessObjects.PersistentObjectEventArgs e)
        {
            //TODO: not sure if this is needed.
        }

        private void Application_DataObjectArrived(object sender, DataExchangeEventArgs e)
        {
            //Push this onto the UI thread since the DataExchange will be called asynchronously from the server.
            if (EncompassApplication.Screens.InvokeRequired)
                EncompassApplication.Screens.Invoke(new DataExchangeEventHandler(Application_DataObjectArrived), new object[] { sender, e });
            else
                DataExchangeHelper.executeDataExchange(e.Data);
        }

        private void Application_MilestoneChange(object sender, MilestoneEventArgs e)
        {
            ReportingHelper.doReporting();
            CliHelper.doCLI();
            //Should we check whether the milestone event is in the monitored milestone list?
            ImagingHelper.doImaging();
           
        }

        private void 
            Application_FieldChange(object sender, FieldChangeEventArgs e)
        {
            if (!(e.PriorValue.Equals(e.NewValue)))
            {
                ReportingHelper.doReporting();

                //Rescission Date
                if (isMonitoredField(e.FieldID, InterfaceFlagType.Other, "RescissionDate"))
                {
                    LoanHelper.calculateRescissionDate();
                }

                //CLI
                if (isMonitoredField(e.FieldID, InterfaceFlagType.CLI))
                {
                    CliHelper.doCLI();
                }

                //Imaging
               
                if (isMonitoredField(e.FieldID, InterfaceFlagType.Imaging))
                {
                    ImagingHelper.doImaging();
                }

                //Commission
                if (isMonitoredField(e.FieldID, InterfaceFlagType.Commission))
                {
                    CommissionHelper.doCommission();
                }

            }
        }

        private void Application_FormChange(object sender, FormChangeEventArgs e)
        {
            //attach a listener for any btnCompanyNameAction buttons.
            try
            {
                EllieMae.Encompass.Forms.Control[] controls = e.Form.GetAllControls();
                foreach (EllieMae.Encompass.Forms.Control control in controls)
                {
                    if (typeof(EllieMae.Encompass.Forms.Button).Equals(control.GetType()))
                    {
                        EllieMae.Encompass.Forms.Button btnControl = (EllieMae.Encompass.Forms.Button)control;
                        if (btnControl.ControlID.StartsWith("btnCompanyNameAction"))
                        {
                            btnControl.Click += new EventHandler(btnClick);
                        }
                    }
                   
                }
            }
            catch (Exception)
            {
                //do nothing
            }
        }

       

        private void selectedIndexChanged(object sender, EventArgs e)
        {
            EllieMae.Encompass.Forms.DropdownBox cmb = (EllieMae.Encompass.Forms.DropdownBox)sender;
            string controlId = cmb.ControlID;//.Field.ToString();//.Action;
        }

        private string GetFieldValueFromForm(string fieldName)
        {
            EllieMae.Encompass.Automation.LoansScreen ls = (LoansScreen)EncompassApplication.Screens[EncompassScreen.Loans];
            EllieMae.Encompass.Forms.TextBox tb = (EllieMae.Encompass.Forms.TextBox)ls.CurrentForm.Controls.Find(fieldName);
            return tb.Value;
        }
        private void btnClick(object sender, EventArgs e)
        {
            EllieMae.Encompass.Forms.Button buttonClick = (EllieMae.Encompass.Forms.Button)sender;
            string action = buttonClick.Action;

            if (action == "CAV")
            {
                string abaNumber = GetFieldValueFromForm("txtCAVABANumber");
                string accountNumber = GetFieldValueFromForm("txtCAVAccountNumber");

                using (BusinessContactLookup frm = new BusinessContactLookup(abaNumber, accountNumber))
                    frm.ShowDialog(System.Windows.Forms.Form.ActiveForm);
            }
            else if (action == "Funding")
            {
                FundingHelper.doFunding();
            }
            else if (action == "CancelFunding")
            {
                FundingHelper.doCancelFunding();
            }
            else if (action == "RefreshCAV")
            {
                LoanHelper.doRefreshCAV();
            }
            else
            {
                //Default to an external website configuration
                using (QuoteDialog frm = new QuoteDialog(EncompassApplication.CurrentLoan, action))
                    frm.ShowDialog(System.Windows.Forms.Form.ActiveForm);
            }
        }

        #endregion application events

        #region menu events


        private void OnUnimplemented(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not implemented yet! Please check back later.");
        }

        private void OnManualTriggerInterface(object sender, EventArgs e)
        {
            ManualInterfaceForm frm = new ManualInterfaceForm();
            frm.ShowDialog();
        }

        private void OnTrayConfig(object sender, EventArgs e)
        {
            TrayConfigForm frm = new TrayConfigForm();
            frm.ShowDialog();
        }

       
        private void OnComissionWebInterface(object sender, EventArgs e)
        {
            CommissionsForm frm = new CommissionsForm();
            //frm.EmployeeId = "NHAGER";// EncompassApplication.CurrentUser.ID;
            frm.ShowDialog();
        }

        private void OnCLIErrorsClicked(object sender, EventArgs e)
        {
            CLIErrors frm = new CLIErrors();
            //frm.EmployeeId = "NHAGER";// EncompassApplication.CurrentUser.ID;
            frm.ShowDialog();
        }

        private void OnUserInfo(object sender, EventArgs e)
        {
            TestForm frm = new TestForm();
            frm.ShowDialog();
        }

        private void OnAbout(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void OnWelcome(object sender, EventArgs e)
        {
            WelcomeForm frm = new WelcomeForm();
            frm.ShowDialog();
        }

        private void OnExport(object sender, EventArgs e)
        {
            ExportForm frm = new ExportForm();
            frm.ShowDialog();
        }

        private void OnServicing(object sender, EventArgs e)
        {
            ServicingForm frm = new ServicingForm();
            frm.ShowDialog();
        }

        private void OnScriptTest(object sender, EventArgs e)
        {
            ScriptTestForm frm = new ScriptTestForm();
            frm.ShowDialog();
        }

        #endregion menu events

        #region common

        

        private void BuildMenu(bool includeLoanMenu)
        {
            //Tray menu items
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("User Info", OnUserInfo);
            trayMenu.MenuItems.Add("-");

            try
            {
                string isCommissionsAccessible = string.Empty;
                CommissionsClient client = UserConfigHelper.getCommissionServiceClient();

                if (!string.IsNullOrEmpty(EncompassApplication.CurrentUser.EmployeeID))
                {
                    isCommissionsAccessible = client.IsCommissionsEligible(EncompassApplication.CurrentUser.EmployeeID);//EncompassApplication.CurrentUser.ID);

                    if (UserHelper.hasPermission(new string[] { "Administrator", "Super Administrator" }) || isCommissionsAccessible == "yes")
                    {
                        trayMenu.MenuItems.Add("Commission Web", OnComissionWebInterface);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: handle exception
                
               // File.AppendAllText(@"C:\Logs\WriteText" + DateTime.Now.ToShortDateString().Replace("/", "") + ".txt", ex.Message);
            }

            //try
            //{

               // throw new Exception("Testing Exception Log from buid menu");
            //}
            //catch (Exception ex)
            //{
               // Console.Write("test Log");
               // Debug.WriteLine("test debug.writeline");
                //Trace.Write("test Tracing output for exception");
                
            //}

            //Sample permission based menu - all personas will have the menu available
            if (UserHelper.hasPermission(new string[] { "Administrator", "Super Administrator" }))
            {
                MenuItem adminMI = new MenuItem("Admin");
                adminMI.MenuItems.Add("Edit Config File", OnTrayConfig);
                adminMI.MenuItems.Add("User Setup", OnUnimplemented);
                trayMenu.MenuItems.Add(adminMI);
                trayMenu.MenuItems.Add("-");
            }

           //CLI Errors
            trayMenu.MenuItems.Add("CLI Errors", OnCLIErrorsClicked);

            //support
            trayMenu.MenuItems.Add("Manually Trigger Interfaces", OnManualTriggerInterface);
            //all
            trayMenu.MenuItems.Add("Servicing Info (ROSS)", OnServicing);
            //funding
            trayMenu.MenuItems.Add("Bulk Funding", OnUnimplemented);
            trayMenu.MenuItems.Add("-");
            if (includeLoanMenu)
            {
                MenuItem loanMI = new MenuItem("Loan");
                loanMI.MenuItems.Add("History", OnUnimplemented);
                loanMI.MenuItems.Add("Script Tester", OnScriptTest);
                loanMI.MenuItems.Add("Export", OnExport);
                trayMenu.MenuItems.Add(loanMI);
                trayMenu.MenuItems.Add("-");
            }
            trayMenu.MenuItems.Add("About", OnAbout);
            trayMenu.MenuItems.Add("Welcome Screen", OnWelcome);

            trayIcon.ContextMenu = trayMenu;
        }

        private bool isMonitoredMilestone(string milestoneEvent, InterfaceFlagType interfaceType, string optionalOtherDesc = "none")
        {
            bool contains = false;
            if (interfaceType == InterfaceFlagType.CLI)
            {
                contains = this.cliMonitoredMilestones.Contains(milestoneEvent);
            }

            //if (interfaceType == InterfaceFlagType.Imaging)
            //{
            //    contains = this.imagingMilestones.Contains(milestoneEvent);
            //}
            return contains;
        }

        private bool isMonitoredField(string fieldId, InterfaceFlagType interfaceType, string optionalOtherDesc = "none")
        {
            bool contains = false;
            if (interfaceType == InterfaceFlagType.CLI)
            {
                contains = this.cliMonitoredFields.Contains(fieldId);
            }
            if (interfaceType == InterfaceFlagType.Other)
            {
                if (optionalOtherDesc == "RescissionDate")
                {
                    contains = this.rescissionDateMonitoredFields.Contains(fieldId);
                }
            }
            if (interfaceType == InterfaceFlagType.Imaging)
            {
                contains = this.imagingMonitoredFields.Contains(fieldId);
            }
            if (interfaceType == InterfaceFlagType.Commission)
            {
                contains = this.commissionMonitoredFields.Contains(fieldId);
            }

            return contains;
        }

        #endregion common

       
    }
}
