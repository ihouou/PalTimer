using HFrame.EX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class SetRTMP : FormEx
    {
        private LiveWindow p;
        public SetRTMP(LiveWindow p)
        {
            this.p = p;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            p.RTMPAddress = txtRTMPAddress.Text + "/" + txtRTMPCode.Text;
            this.Close();
        }
    }
}
