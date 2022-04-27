using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class BestEditItem : UserControl
    {
        private iBestEditForm p;
        public BestEditItem(iBestEditForm p, string name, string des, TimeSpan time,int idx)
        {
            InitializeComponent();
            this.p = p;
            if (name.Trim() == "") name = "节点" + idx;
            txtName.Text = name;
            txtDes.Text = des;
            numHour.Value = time.Hours;
            numMin.Value = time.Minutes;
            numSec.Value = time.Seconds;
        }

        public CheckPointNewer GetValue()
        {
            CheckPointNewer ret = new CheckPointNewer();
            ret.Name = txtName.Text.Trim();
            ret.NickName = txtDes.Text;
            ret.BestTS = new TimeSpan(Convert.ToInt32(numHour.Value), Convert.ToInt32(numMin.Value), Convert.ToInt32(numSec.Value));

            if (ret.Name == "") ret.Name = "无名节点";
            return ret;
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            p?.CallRemoveItem(this);
        }
    }
    public interface iBestEditForm
    {
        void CallRemoveItem(BestEditItem c);
    }
}
