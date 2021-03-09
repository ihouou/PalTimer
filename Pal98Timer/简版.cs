using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pal98Timer
{
    public class 简版 : TimerCore
    {
        private PTimer MT = new PTimer();
        public override string GetAAction()
        {
            return "";
        }

        public override string GetGameVersion()
        {
            return "";
        }

        public override string GetMainWatch()
        {
            return MT.ToString();
        }

        public override string GetMoreInfo()
        {
            return "按F8启停";
        }

        public override string GetPointEnd()
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

        public override void InitCheckPoints()
        {
        }

        public override void OnFunctionKey(int FunNo, NewForm form)
        {
            switch (FunNo)
            {
                case 8:
                    BtnLiteCtrl_Click(null, null);
                    break;
                case 6:
                    if (form.Confirm("更换内核将会重置计时器，确认么？"))
                    {
                        BtnSwitch_Click(null, null);
                    }
                    break;
            }
        }

        public override void Reset()
        {
            MT.Reset();
        }

        public override void SetTS(TimeSpan ts)
        {
            MT.SetTS(ts);
        }

        public override void Start()
        {
        }

        public override void Unload()
        {
        }

        private Button btnLiteCtrl;
        private ToolStripMenuItem btnSwitch;
        private Size orisize;
        public override void InitUI(NewForm form)
        {
            btnLiteCtrl = form.NewMenuButton(0);
            btnLiteCtrl.Text = "开始";
            btnLiteCtrl.Click += BtnLiteCtrl_Click;

            btnSwitch = form.NewMenuItem();
            btnSwitch.Text = "切换至98速通版";
            btnSwitch.Click += BtnSwitch_Click;

            form.pnMid.Visible = false;
            form.pnPointEnd.Visible = false;
            form.lblST.Visible = false;
            form.lblT2.Visible = false;
            //form.lblMore.Visible = false;
            orisize = form.Size;
            form.Size = new Size(270, 165);
        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            LoadCore(new 仙剑98柔情());
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

        public override void UnloadUI(NewForm form)
        {
            base.UnloadUI(form);
            form.pnMid.Visible = true;
            form.pnPointEnd.Visible = true;
            form.lblST.Visible = true;
            form.lblT2.Visible = true;
            form.lblMore.Visible = true;
            form.Size = orisize;
        }

        public override string GetCriticalError()
        {
            return "";
        }
    }
}
