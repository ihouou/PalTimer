using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }
        private object GameObj;
        public void ShowData(object GameObj)
        {
            this.GameObj = GameObj;
            if (!timer1.Enabled)
            {
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtText.Text = GameObj.ToString();
        }

        private void DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
            }
            timer1.Dispose();
        }
    }
}
