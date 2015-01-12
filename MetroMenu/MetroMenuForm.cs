using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoanMonitorPlugin.MetroMenu
{
    public partial class MetroMenuForm : Form
    {
        public MetroMenuForm()
        {
            InitializeComponent();
            GenerateTiles();
        }

        private void GenerateTiles()
        {
            for (int i = 0; i < 10; i++)
            {
                MetroTile mt = new MetroTile("Item " + i);
                AddTile(mt);
            }
            metroFlowLayout.PerformLayout();
        }

        private void AddTile(MetroTile tile)
        {
            metroFlowLayout.Controls.Add(tile);
        }
    }
}
