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
    public delegate void OnBtnClick();
    public partial class InfoShow : FormEx
    {
        private FormEx p;
        private OnBtnClick cb=delegate(){};
        public InfoShow(FormEx p,OnBtnClick cb)
        {
            this.p = p;
            this.cb = cb;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            cb();
        }

        private void InfoShow_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
    }
}
