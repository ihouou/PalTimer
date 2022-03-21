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
    public delegate void DownloadCallBackDel(string c, Download d);
    public partial class Download : FormEx
    {
        private DownloadCallBackDel cb = null;
        public Download(DownloadCallBackDel c)
        {
            this.cb = c;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string c=txtCode.Text;
            if (c == "")
            {
                Error("请输入存档编码");
                return;
            }
            txtCode.Enabled = false;
            btnOK.Enabled = false;
            /*if (p != null)
            {
                p.LoadCloudSRPG(c, this);
            }*/
            if (cb != null)
            {
                cb(c, this);
            }
        }

        private void Download_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
