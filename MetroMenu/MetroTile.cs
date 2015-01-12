using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoanMonitorPlugin.MetroMenu
{
    public partial class MetroTile : UserControl
    {
        public MetroTile(string tileText)
        {
            InitializeComponent();
            this.TileText.Text = tileText;
        }
    }
}
