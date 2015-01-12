using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;
using System.Text.RegularExpressions;
using EllieMae.Encompass.Collections;
using LoanMonitorPlugin.Helpers;

namespace LoanMonitorPlugin.Forms
{
    public partial class ScriptTestForm : Form
    {
        public ScriptTestForm()
        {
            InitializeComponent();
            btnEvaluate.Click += new EventHandler(btnEvaluate_Click);
        }

        private void btnSetField_Click(object sender, EventArgs e)
        {
            try
            {
                LoanHelper.setFieldNoRules(txtSetField.Text, txtSetValue.Text);
                Macro.Alert("Field has been updated.");
            }
            catch(Exception ex)
            {
                Macro.Alert("Error occurred while attempting to set field::" + ex.Message);
            }
            txtSetField.Text = "";
            txtSetValue.Text = "";
        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            evalScript();
            parseScript();
        }

        private void evalScript()
        {
            try
            {
                bool result = (bool)Macro.Eval(txtScript.Text);
                txtResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        private void parseScript()
        {
            string sInput = txtScript.Text;

            // store unique field IDs that we find within code
            StringList fieldList = new StringList();

            // split field IDs with Regex and put unique fields into dictFields
            string sFieldID = null;

            Regex rx = new Regex(@"\[(.*?)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(sInput);

            foreach (Match match in matches)
            {
                if (!fieldList.Contains(match.Value))
                    fieldList.Add(match.Value);
            }

            // show all field values
            string sAllFields = "";
            foreach (string sField in fieldList)
            {
                sFieldID = sField.Replace("[", "").Replace("]", "");
                string sValue = "";
                try
                {
                    sValue = EncompassApplication.CurrentLoan.Fields[sFieldID].FormattedValue;
                }
                catch (Exception ex)
                {
                    sValue = "ERROR: " + ex.Message;
                    //sValue = sValue.Replace(Constants.vbCr, " ").Replace(Constants.vbLf, " ").Replace(Constants.vbTab, " ");
                    sValue = sValue.Replace("Parameter name: fieldId", "");
                    //sValue = sValue.Replace("  ", " ");
                    sValue = sValue.Replace("  ", " ");
                    sValue = sValue.Trim();
                }
                sAllFields = sAllFields + sFieldID + "\t" + sValue + Environment.NewLine;
            }
            txtFields.Text = sAllFields;
        }

        private void parseScript2()
        {
            //string currentScript = txtScript.Text;


            string sInput = txtScript.Text;

            // store unique field IDs that we find within code
            StringList fieldList = new StringList();

            // split field IDs with Regex and put unique fields into dictFields
            string sFieldID = null;

            Regex rx = new Regex(@"\[(.*?)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(sInput);

            foreach (Match match in matches)
            {
                if (!fieldList.Contains(sFieldID))
                    fieldList.Add(sFieldID);
            }

            // show all field values
            string sAllFields = "";
            foreach (string sField in fieldList)
            {
                sFieldID = sField.Replace("[", "").Replace("]", "");
	            string sValue = "";
	            try {
		            sValue = EncompassApplication.CurrentLoan.Fields[sFieldID].FormattedValue;
	            } catch (Exception ex) {
		            sValue = "ERROR: " + ex.Message;
		            //sValue = sValue.Replace(Constants.vbCr, " ").Replace(Constants.vbLf, " ").Replace(Constants.vbTab, " ");
		            sValue = sValue.Replace("Parameter name: fieldId", "");
		            //sValue = sValue.Replace("  ", " ");
		            sValue = sValue.Replace("  ", " ");
		            sValue = sValue.Trim();
	            }
	            sAllFields = sAllFields + sFieldID + "\t" + sValue + Environment.NewLine;
            }

            txtFields.Text = sAllFields;
        }
    }
}
