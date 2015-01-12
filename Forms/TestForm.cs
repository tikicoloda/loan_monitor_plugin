using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Users;

namespace LoanMonitorPlugin.Forms
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            String sid = EncompassApplication.Session.ID;
            String userName = EncompassApplication.CurrentUser.ID;
            String personas = "";

            foreach (Persona p in EncompassApplication.CurrentUser.Personas)
            {
                personas += p.Name + ";";
            }

            txtSessionId.Text = sid;
            txtUserName.Text = userName;
            txtPersonas.Text = personas;
        }

    }
}
