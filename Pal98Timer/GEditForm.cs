using HFrame.EX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public partial class GEditForm : FormEx
    {
        private GBoard bb;
        private GRender rr;
        private TimeSpan ts = new TimeSpan(0, 20, 15);
        private TimeSpan cur = new TimeSpan(0, 20, 15);
        private TimeSpan cha = new TimeSpan(0, 0, 3);
        private GForm mf;
        public GEditForm(GForm mf)
        {
            this.mf = mf;
            InitializeComponent();
            this.FormClosed += delegate (object sender, FormClosedEventArgs e) {
                this.Dispose();
            };
            pnMain.MouseWheel += PnMain_MouseWheel;
            pnMain.MouseClick += pnMain_MouseClick;

            bb = new GBoard(mf.rr.GetGBoard());
            rr = new GRender(pnMain, true);
            rr.SetGBoard(bb);
            rr.SetBG(GForm.bgpath);
            rr.OnEditCurrentChanged = delegate (GBoardChanger gbc) {
                if (gbc == null)
                {
                    gpBlock.Visible = false;
                }
                else
                {
                    gpBlock.Text = gbc.Name;
                    string info = gbc.Description + "\r\n";
                    if (gbc.OnLeftClick != null)
                    {
                        info += "\r\n\r\n鼠标左键：修改填充颜色";
                    }
                    if (gbc.OnRightClick != null)
                    {
                        info += "\r\n\r\n鼠标右键：修改描边颜色";
                    }
                    if (gbc.OnMiddleClick != null)
                    {
                        info += "\r\n\r\n鼠标中键：修改字体";
                    }
                    if (gbc.OnWheel != null)
                    {
                        info += "\r\n\r\n鼠标滚轮：修改字体大小";
                    }
                    if (gbc.OnCtrlWheel != null)
                    {
                        info += "\r\n\r\nCtl+滚轮：修改描边宽度";
                    }

                    lblBlock.Text = info;
                    gpBlock.Visible = true;
                }
            };
            InitRender();
        }

        private void InitRender()
        {
            rr.SetTitle("自动计时器");
            rr.SetGameVersion("等待游戏运行");
            rr.SetVersion("v2.34");
            rr.SetBL("好运签");
            rr.SetBR("皮言皮语");
            rr.SetMoreInfo("蜂0 蜜0 火0 血0 夜0 剑0");
            rr.SetSubTimer("0.00s");
            rr.SetOutTimer("+ 0:00:00.00");
            rr.SetWillClear("预计通关  03:20:01");
            rr.SetPointSpan("[xx~xx] 99:00.00");
            rr.AddDot("测试-红");
            rr.AddDot("测试-绿");
            rr.AddDot("测试-蓝");
            rr.AddDot("测试-橙");

            rr.AddBtn("暂停").Red().Text = "暂停 1";
            rr.AddBtn("重置");
            rr.AddBtn("功能").Orange();

            rr.AddItem("相等", new TimeSpan(0, 5, 53), GRender.GItem.EditMetaEqual);
            rr.AddItem("快", new TimeSpan(0, 5, 53), GRender.GItem.EditMetaFast);
            rr.AddItem("慢", new TimeSpan(0, 5, 53), GRender.GItem.EditMetaSlow);
            rr.AddItem("当前", new TimeSpan(ts.Ticks), GRender.GItem.EditMetaActive);

            rr.GetItem(1).Cur = new TimeSpan(0, 4, 10);
            rr.GetItem(2).Cur = new TimeSpan(0, 7, 10);

            rr.ItemIdx = 3;

            rr.IsInCheck = true;
            rr.IsC = true;
        }
        
        private void pnMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (rr.CurrentEdit != null)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        rr.CurrentEdit.OnLeftClick?.Invoke(0, SelectColor, SelectFont);
                        break;
                    case MouseButtons.Right:
                        rr.CurrentEdit.OnRightClick?.Invoke(0, SelectColor, SelectFont);
                        break;
                    case MouseButtons.Middle:
                        rr.CurrentEdit.OnMiddleClick?.Invoke(0, SelectColor, SelectFont);
                        break;
                }
            }
        }

        private void PnMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (mf.OnCtrlDown || mf.OnCtrlDown2)
            {
                if (rr.CurrentEdit != null && rr.CurrentEdit.OnCtrlWheel != null)
                {
                    int d = 0;
                    if (e.Delta > 0)
                    {
                        d = 1;
                    }
                    else if (e.Delta < 0)
                    {
                        d = -1;
                    }
                    rr.CurrentEdit.OnCtrlWheel(d);
                }
            }
            else
            {
                if (rr.CurrentEdit != null && rr.CurrentEdit.OnWheel != null)
                {
                    int d = 0;
                    if (e.Delta > 0)
                    {
                        d = 1;
                    }
                    else if (e.Delta < 0)
                    {
                        d = -1;
                    }
                    rr.CurrentEdit.OnWheel(d);
                }
            }
        }
        
        private Color SelectColor(Color ori)
        {
            dlgColor.Color = ori;
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                return dlgColor.Color;
            }
            else
            {
                return ori;
            }
        }
        private Font SelectFont(Font ori)
        {
            dlgFont.Font = ori;
            if (dlgFont.ShowDialog(this) == DialogResult.OK)
            {
                return dlgFont.Font;
            }
            else
            {
                return ori;
            }
        }

        public string SelectedBGPath = GForm.bgpath;
        private void btnClearBG_Click(object sender, EventArgs e)
        {
            SelectedBGPath = "";
            rr.SetBG("");
            rr.IsForceRefreshAll = true;
        }

        private void btnSetBG_Click(object sender, EventArgs e)
        {
            if (dlgFile.ShowDialog(this) == DialogResult.OK)
            {
                string fn1 = dlgFile.FileName;
                CutBGForm cf = new CutBGForm();
                cf.SetData(mf, fn1);
                if (cf.ShowDialog(this) == DialogResult.OK)
                {
                    Image tmp = cf.GetResult();
                    if (tmp != null)
                    {
                        SelectedBGPath = "tmpbg_" + Guid.NewGuid().ToString() + ".png";
                        //if (File.Exists("tmp.png")) File.Delete("tmp.png");
                        tmp.Save(SelectedBGPath, System.Drawing.Imaging.ImageFormat.Png);
                        tmp.Dispose();
                        rr.SetBG(SelectedBGPath);
                        rr.IsForceRefreshAll = true;
                    }
                }
                cf.x();
                cf.Dispose();
            }
        }

        private bool isSave = false;
        private void btnSave_Click(object sender, EventArgs e)
        {
            bb.Save();
            mf.rr.SetGBoard(bb);
            if (SelectedBGPath != GForm.bgpath)
            {
                if (File.Exists(GForm.bgpath))
                {
                    File.Move(GForm.bgpath, GForm.bgpath.Replace("bg", "bg_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                }
                if (File.Exists(SelectedBGPath))
                {
                    File.Copy(SelectedBGPath, GForm.bgpath);
                    mf.rr.SetBG(GForm.bgpath);
                }
                else
                {
                    mf.rr.SetBG("");
                }
            }
            mf.rr.IsForceRefreshAll = true;
            isSave = true;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cur += new TimeSpan(1000000);
            if (cur - ts > cha)
            {
                cur = ts - cha;
            }
            rr?.SetMainTimer(cur);
            if (rr != null && rr.Draw())
            {
                pnMain.Invalidate();
            }
        }
    }

    public class DFPanel : Panel
    {
        public DFPanel() : base()
        {
            DoubleBuffered = true;
        }
    }
}
