using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HFrame.EX;

namespace Pal98Timer
{
    public partial class Upload : FormEx
    {
        private ToolStripMenuItem btnCS;
        public Upload(ToolStripMenuItem b)
        {
            this.btnCS = b;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (btnCS != null)
            {
                btnCS.Enabled = true;
            }
            this.Dispose();
        }

        private void Upload_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
