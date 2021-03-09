using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HFrame.EX;
using System.IO;
using HFrame.ENT;

namespace Pal98Timer
{
    public partial class NewPointItem : UserControl
    {
        private TimerCore core;
        private FormEx p;
        public NewPointItem()
        {
            InitializeComponent();
            InitColor();
            InitFontEvent();
        }
        public NewPointItem(FormEx p,TimerCore core)
        {
            this.p = p;
            this.core = core;
            InitializeComponent();
            InitColor();
            InitFontEvent();
        }
        private void InitColor()
        {
            NMColor = Color.FromArgb(ColorBoard.ins.OPQ, 37, 37, 37);
            FlushTitleForeColor();
            FlushBestForeColor();
            FlushCurrentFont();
            FlushCHAFont();
        }
        private void InitFontEvent()
        {
            SetCustomFont(lblPTitle, delegate () {
                ((NewForm)p).OnPointTitleForeColorChanged();
            });
            SetCustomFont(lblPCurrent, delegate () {
                ((NewForm)p).OnPointCurrentFontChanged();
            });
            SetCustomFont(lblPBest, delegate () {
                ((NewForm)p).OnPointBestForeColorChanged();
            });
            SetCustomFont(lblPCHA, delegate () {
                ((NewForm)p).OnPointCHAFontChanged();
            });
        }
        private CheckPoint _cp;
        private Color NMColor;
        public void InitShow(CheckPoint cp)
        {
            _cp = cp;
            lblPTitle.Text = cp.GetNickName();
            lblPBest.Text = TimerCore.TimeSpanToStringLite(cp.Best);
            lblPCHA.Text = "";
            if (cp.Index % 2 == 0)
            {
                NMColor = Color.FromArgb(ColorBoard.ins.OPQ, 51, 51, 51);
            }
            this.BackColor = NMColor;
        }
        public void Flush()
        {
            if (_cp.IsBegin && !_cp.IsEnd)
            {
                ShowCHA();
                this.BackColor = Color.FromArgb(ColorBoard.ins.OPQ, 0, 122, 204);
                lblPCurrent.Text = TimerCore.TimeSpanToString(_cp.Current);
            }
            else
            {
                this.BackColor = NMColor;
                if (_cp.IsEnd)
                {
                    ShowCHA();
                    lblPCurrent.Text = TimerCore.TimeSpanToString(_cp.Current);
                }
                else
                {
                    lblPCHA.Text = "";
                    lblPCurrent.Text = "";
                }
            }
        }

        public void FlushOPQ()
        {
            Color ori = this.BackColor;
            this.BackColor = Color.FromArgb(ColorBoard.ins.OPQ, ori.R, ori.G, ori.B);
        }

        private long cha = 0;
        private void ShowCHA()
        {
            cha = _cp.GetCHA();
            SetCHAColor();
            lblPCHA.Text = TimerCore.GetChaStr(cha);
        }

        private void SetCHAColor()
        {
            if (cha < 0)
            {
                lblPCHA.ForeColor = ColorBoard.ins.PointFasterForeColor;
                lblPCurrent.ForeColor = ColorBoard.ins.PointFasterForeColor;
            }
            else if (cha > 0)
            {
                lblPCHA.ForeColor = ColorBoard.ins.PointSlowerForeColor;
                lblPCurrent.ForeColor = ColorBoard.ins.PointSlowerForeColor;
            }
            else
            {
                lblPCHA.ForeColor = ColorBoard.ins.PointEqualForeColor;
                lblPCurrent.ForeColor = ColorBoard.ins.PointEqualForeColor;
            }
        }

        private void jump()
        {
            if (p.Confirm("确定跳转到时间点【" + _cp.GetNickName() + "】么？"))
            {
                core.Jump(_cp.Index);
            }
        }

        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            jump();
        }

        private void lblCHA_DoubleClick(object sender, EventArgs e)
        {
            jump();
        }

        private void lblBest_DoubleClick(object sender, EventArgs e)
        {
            jump();
        }

        private void lblCurrent_DoubleClick(object sender, EventArgs e)
        {
            jump();
        }

        public void FlushTitleForeColor()
        {
            //lblPTitle.ForeColor = ColorBoard.ins.PointTitleForeColor;
            ColorBoard.ins.ShowWD(lblPTitle);
        }

        public void FlushBestForeColor()
        {
            //lblPBest.ForeColor = ColorBoard.ins.PointBestForeColor;
            ColorBoard.ins.ShowWD(lblPBest);
        }
        public void FlushCurrentFont()
        {
            HObj wd = ColorBoard.ins.GetWD(lblPCurrent.Name);
            lblPCurrent.Font = new Font(wd.GetValue<string>("FontName"), wd.GetValue<float>("Size"), (FontStyle)(wd.GetValue<int>("FontStyle")));
        }
        public void FlushCHAFont()
        {
            HObj wd = ColorBoard.ins.GetWD(lblPCHA.Name);
            lblPCHA.Font = new Font(wd.GetValue<string>("FontName"), wd.GetValue<float>("Size"), (FontStyle)(wd.GetValue<int>("FontStyle")));
        }

        public void FlushCHAForeColor()
        {
            SetCHAColor();
        }

        public delegate void OnFontSet();
        public void SetCustomFont(Control ctl, OnFontSet cb)
        {
            if (p is NewForm)
            {
                ctl.MouseWheel += delegate (object sender, MouseEventArgs e)
                {
                    if (((NewForm)p).OnCtrlDown)
                    {
                        int d = 1;
                        if (e.Delta < 0)
                        {
                            d = -1;
                        }
                        Font last = ctl.Font;
                        float newsize = last.Size + d;
                        ctl.Font = new Font(last.FontFamily, last.Size + d, last.Style);
                        ColorBoard.ins.GetWD(ctl.Name)["Size"] = newsize;
                        //((NewForm)p).OnPointTitleForeColorChanged();
                        cb?.Invoke();
                    }
                };

                ctl.MouseClick += delegate (object sender, MouseEventArgs e) {
                    if (e.Button == MouseButtons.Middle)
                    {
                        Font last = ctl.Font;
                        FontDialogEx fd = new FontDialogEx();
                        fd.SetLocation(p.Location.X, p.Location.Y);
                        fd.Font = ctl.Font;
                        fd.ShowColor = false;
                        if (fd.ShowDialog(this) == DialogResult.OK)
                        {
                            ColorBoard.ins.GetWD(ctl.Name)["FontStyle"] = (int)(fd.Font.Style);
                            ColorBoard.ins.GetWD(ctl.Name)["FontName"] = fd.Font.FontFamily.Name;
                            ColorBoard.ins.GetWD(ctl.Name)["Size"] = fd.Font.Size;
                        }

                        /*if (last.Style == FontStyle.Bold)
                        {
                            ctl.Font = new Font(last, FontStyle.Regular);
                            ColorBoard.ins.GetWD(ctl.Name)["FontStyle"] = (int)FontStyle.Regular;
                        }
                        else
                        {
                            ctl.Font = new Font(last, FontStyle.Bold);
                            ColorBoard.ins.GetWD(ctl.Name)["FontStyle"] = (int)FontStyle.Bold;
                        }*/
                        cb?.Invoke();
                    }
                };
            }
        }
        public delegate void OnColorBoardSet(Color color);
        public void SetCustomForeColor(Color ori, OnColorBoardSet callback)
        {
            if (p is NewForm)
            {
                ((NewForm)p).cdCommon.Color = ori;
                ((NewForm)p).cdCommon.SetLocation(p.Location.X, p.Location.Y);
                ((NewForm)p).cdCommon.ShowDialog(this);
                callback?.Invoke(((NewForm)p).cdCommon.Color);
            }
        }

        private void btnSetTitleColor_Click(object sender, EventArgs e)
        {
            SetCustomForeColor(lblPTitle.ForeColor, delegate (Color c) {
                //ColorBoard.ins.PointTitleForeColor = c;
                ColorBoard.ins.GetWD("lblPTitle")["ForeColor"] = c.ToArgb();
                if (p is NewForm)
                {
                    ((NewForm)p).OnPointTitleForeColorChanged();
                }
            });
        }

        private void btnSetBestColor_Click(object sender, EventArgs e)
        {
            SetCustomForeColor(lblPBest.ForeColor, delegate (Color c) {
                //ColorBoard.ins.PointBestForeColor = c;
                ColorBoard.ins.GetWD("lblPBest")["ForeColor"] = c.ToArgb();
                if (p is NewForm)
                {
                    ((NewForm)p).OnPointBestForeColorChanged();
                }
            });
        }

        private void btnSetFasterColor_Click(object sender, EventArgs e)
        {
            SetCustomForeColor(ColorBoard.ins.PointFasterForeColor, delegate (Color c) {
                ColorBoard.ins.PointFasterForeColor = c;
                if (p is NewForm)
                {
                    ((NewForm)p).OnPointCHAForeColorChanged();
                }
            });
        }

        private void btnSetSlowerColor_Click(object sender, EventArgs e)
        {
            SetCustomForeColor(ColorBoard.ins.PointSlowerForeColor, delegate (Color c) {
                ColorBoard.ins.PointSlowerForeColor = c;
                if (p is NewForm)
                {
                    ((NewForm)p).OnPointCHAForeColorChanged();
                }
            });
        }

        private void btnSetEqualColor_Click(object sender, EventArgs e)
        {
            SetCustomForeColor(ColorBoard.ins.PointEqualForeColor, delegate (Color c) {
                ColorBoard.ins.PointEqualForeColor = c;
                if (p is NewForm)
                {
                    ((NewForm)p).OnPointCHAForeColorChanged();
                }
            });
        }
    }
}
