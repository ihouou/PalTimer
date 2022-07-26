using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyChanger
{
    public partial class KeyAddForm : Form
    {
        private SettingForm p;
        private int ori = -1;
        private int n = -1;
        public KeyAddForm(SettingForm p)
        {
            this.p = p;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ori < 0 || n < 0)
            {
                MessageBox.Show(this, "键位设置不完整", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ori == n)
            {
                MessageBox.Show(this, "键位设置相同", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            p.IsSet = true;
            p.ori = ori;
            p.n = n;
            this.Close();
        }

        private void txtOri_KeyDown(object sender, KeyEventArgs e)
        {
            ori = p.CurrentKeyCode;
            txtOri.Text = KC.KeyCode2Name(ori);
        }

        private void txtOri_KeyUp(object sender, KeyEventArgs e)
        {
            ori = p.CurrentKeyCode;
            txtOri.Text = KC.KeyCode2Name(ori);
        }

        private void txtN_KeyDown(object sender, KeyEventArgs e)
        {
            n = p.CurrentKeyCode;
            txtN.Text = KC.KeyCode2Name(n);
        }

        private void txtN_KeyUp(object sender, KeyEventArgs e)
        {
            n = p.CurrentKeyCode;
            txtN.Text = KC.KeyCode2Name(n);
        }
    }
}
