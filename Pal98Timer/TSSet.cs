using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public delegate void SetTSDel(TimeSpan ts);
    public partial class TSSet : Form
    {
        private MainForm parent;
        public TSSet(MainForm parent)
        {
            InitializeComponent();
            this.cb = null;
            this.parent = parent;
            txtHour.GotFocus += txtHour_GotFocus;
            txtMin.GotFocus += txtMin_GotFocus;
            txtSec.GotFocus += txtSec_GotFocus;
            txtMs.GotFocus += txtMs_GotFocus;
        }
        private SetTSDel cb;
        public TSSet(SetTSDel callback)
        {
            InitializeComponent();
            this.cb = callback;
            this.parent = null;
            txtHour.GotFocus += txtHour_GotFocus;
            txtMin.GotFocus += txtMin_GotFocus;
            txtSec.GotFocus += txtSec_GotFocus;
            txtMs.GotFocus += txtMs_GotFocus;
        }

        void txtMs_GotFocus(object sender, EventArgs e)
        {
            txtMs.SelectAll();
        }

        void txtSec_GotFocus(object sender, EventArgs e)
        {
            txtSec.SelectAll();
        }

        void txtMin_GotFocus(object sender, EventArgs e)
        {
            txtMin.SelectAll();
        }

        void txtHour_GotFocus(object sender, EventArgs e)
        {
            txtHour.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int h = 0;
            int m = 0;
            int s = 0;
            int ms = 0;
            int.TryParse(txtHour.Text, out h);
            int.TryParse(txtMin.Text, out m);
            int.TryParse(txtSec.Text, out s);
            int.TryParse(txtMs.Text, out ms);
            TimeSpan ts = new TimeSpan(0, h, m, s, ms);
            if (parent != null)
            {
                parent.SetMainClock(ts);
            }
            if (cb != null)
            {
                cb(ts);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtHour_Validating(object sender, CancelEventArgs e)
        {
            string c = ((TextBox)sender).Text;
            int t = 0;
            if (!int.TryParse(c, out t))
            {
                ((TextBox)sender).Text = "0";
            }
        }

        private void txtMin_Validating(object sender, CancelEventArgs e)
        {
            string c = ((TextBox)sender).Text;
            int t = 0;
            if (!int.TryParse(c, out t) || t > 60 || t < 0)
            {
                ((TextBox)sender).Text = "0";
            }
        }

        private void txtSec_Validating(object sender, CancelEventArgs e)
        {
            string c = ((TextBox)sender).Text;
            int t = 0;
            if (!int.TryParse(c, out t) || t>60 || t<0)
            {
                ((TextBox)sender).Text = "0";
            }
        }

        private void txtMs_Validating(object sender, CancelEventArgs e)
        {
            string c = ((TextBox)sender).Text;
            int t = 0;
            if (!int.TryParse(c, out t) || t > 999 || t < 0)
            {
                ((TextBox)sender).Text = "0";
            }
        }
    }
}
