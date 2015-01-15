using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;
using LoanMonitorPlugin.Helpers;

namespace LoanMonitorPlugin.Forms
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();

            chkDisplayWelcome.Checked = UserConfigHelper.shouldDisplayWelcome();
            LoadContent();
        }

        private void LoadContent()
        {
            webBrowser.DocumentText = @"<html>
                                        <style>
                                          .blue {color:#0077C8;}
                                          .red {color:#CE0058;}
                                          .content {margin-top:20px;width:700px;}
                                          div [width:700px;]
                                          h2 {margin-left:20px;}
                                          h3 {margin-left:20px;}
                                          p {margin-left:40px;}
                                          li {margin-left:40px;}
                                          body {font-family:'HelveticaNeue-Light', 'Helvetica Neue Light', 'Helvetica Neue', Helvetica, Arial, 'Lucida Grande', sans-serif;width:700px;}
                                        </style>
                                          <body>
                                            <div id='header'><img src='http://intranet.companyname.com/brandcenter/Documents/CompanyName_logo_3c_pms.jpg'/></div>
                                            <div id='content' class='content'>
                                              <h1 class='blue'>Welcome to the e360 Tray Application.</h1>
                                              <p>
	                                        The e360 Tray Application is a companion tool for the Encompass360 software. 
	                                        This is an 'alpha' release and, as such, the features will change over time.
	                                        See below for some high-level information about the initial release.
	                                        If you have any questions/concerns, please contact the CompanyName Development Team through the <a href='mailto:EncompassDevelopmentTeam@companyname.com'>Encompass - Development distribution</a>.
                                              </p>
                                              <h3 class='blue'>Dynamic Event Handling</h3>
                                              <p>
	                                        At the core of the plugin is support for handling events.
	                                        The events being handled provide, nearly, limitless possibilities for extensibility.
	                                        All Design Patterns previously identified in the 'CompanyName Prototype Design Patterns' form have now been replicated externally.
	                                        This means we have a single code-base for deploying between environments.
	                                        <ul><li>Interface Triggers</li><li>Extending button clicks</li></ul>
                                              </p>
                                              <h3 class='blue'>Permission Based Menu</h3>
                                              <p>
	                                        The main UI of the application is built with the idea of creating menus dynamically based on the users persona.
	                                        This allows the user experience to be customized for each role within the application.
	                                        Below are some of the scenarios which are permission based:
	                                        <ul><li>Support Related Items</li><li>Administration Tools</li><li>Role-related Forms</li></ul>
                                              </p>
                                              <h3 class='blue'>Non-Loan Level Forms</h3>
                                              <p>
	                                        One of the biggest limitations within the Encompass360 application has been the lack of support for forms outside of a loan.
	                                        This limitation has now been lifted, as we can provide simple access to the forms.
	                                        Some of the big 
	                                        <ul><li>Servicing Screen (ROSS)</li><li>Bulk Funding</li></ul>
                                              </p>
                                              <h3 class='blue'>Administrative Functions</h3>
                                              <p>
	                                        The tools provided under the Admin menu will allow for a consolidated location for miscellaneous 
	                                        maintenance tools. These tools will provide additional functionality outside the realm of the Enconpass360 configuration.
	                                        <ul><li>CLI Monitored Fields</li><li>User Administration</li><li>Property Configuration Manager</li></ul>
                                              </p>
                                            </div>
                                          </body>
                                        </html>";

        }

        private void chkDisplayWelcome_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkDisplayWelcome.Checked;
            UserConfigHelper.setDisplayWelcome(isChecked?"Y":"N");
        }
    }
}
