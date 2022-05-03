using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public class 简版 : TimerCore
    {
        public 简版(GForm form) : base(form)
        {
            CoreName = "S";
        }
        public override bool NeedBlockCtrlEnter()
        {
            return false;
        }
        public override string GetAAction()
        {
            return "";
        }

        public override string GetGameVersion()
        {
            return "";
        }

        public override TimeSpan GetMainWatch()
        {
            return MT.CurrentTS;
        }
        public override bool IsMainWatchStar()
        {
            return false;
        }

        public override string GetMoreInfo()
        {
            return "按F9启停";
        }

        public override string GetPointEnd()
        {
            return "";
        }
        public override string GetPointSpan()
        {
            return "";
        }

        public override string GetSecondWatch()
        {
            return "";
        }

        public override string GetSmallWatch()
        {
            return "";
        }

        protected override void InitCheckPoints()
        {
        }
        public override bool IsShowC()
        {
            return false;
        }

        public override void OnFunctionKey(int FunNo)
        {
            switch (FunNo)
            {
                case 9:
                    BtnLiteCtrl_Click(null, null);
                    break;
                /*case 6:
                    if (form.Confirm("更换内核将会重置计时器，确认么？"))
                    {
                        BtnSwitch_Click(null, null);
                    }
                    break;*/
            }
        }

        public override void Reset()
        {
            base.Reset();
            form.rr.IsForceRefreshAll = true;
            if (IsBegin)
            {
                BtnLiteCtrl_Click(null,null);
            }
        }

        protected override void OnTick()
        {
        }
        
        private GRender.GBtn btnLiteCtrl;
        private ToolStripMenuItem btnSwitch;
        private Size orisize;
        public override void InitUI()
        {
            form.SetSCoreBtnVisible(false);
            form.rr.VisibleBtn(0, false);
            btnLiteCtrl = form.NewMenuButton(0);
            btnLiteCtrl.Text = "开始";
            //btnLiteCtrl.Click += BtnLiteCtrl_Click;
            btnLiteCtrl.OnClicked = delegate (int x, int y, GRender.GBtn ctl)
              {
                  BtnLiteCtrl_Click(null, null);
              };

            btnSwitch = form.NewMenuItem();
            btnSwitch.Text = "切换至98速通版";
            btnSwitch.Click += BtnSwitch_Click;

            /*form.pnMid.Visible = false;
            form.pnPointEnd.Visible = false;
            form.lblST.Visible = false;
            form.lblT2.Visible = false;*/
            //form.lblMore.Visible = false;
            orisize = form.Size;
            form.Size = new Size(270, 200);
        }

        public override void UnloadUI()
        {
            base.UnloadUI();
            form.Size = orisize;
            form.rr.VisibleBtn(0, true);
            form.SetSCoreBtnVisible(true);
        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            LoadCore(new 仙剑98柔情(form));
        }

        private bool IsBegin = false;

        private void BtnLiteCtrl_Click(object sender, EventArgs e)
        {
            IsBegin = !IsBegin;
            if (IsBegin)
            {
                MT.Start();
                btnLiteCtrl.Text = "暂停";
            }
            else
            {
                MT.Stop();
                btnLiteCtrl.Text = "开始";
            }
        }

        public override string GetCriticalError()
        {
            return "";
        }
    }
}
