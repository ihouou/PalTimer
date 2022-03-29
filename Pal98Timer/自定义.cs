using System;
using System.Collections.Generic;
using System.Text;

namespace Pal98Timer
{
    public class 自定义 : TimerCore
    {
        public 自定义(GForm form) : base(form)
        {
            CoreName = "CUSTOM";
        }
        public override string GetAAction()
        {
            return "";
        }

        public override string GetCriticalError()
        {
            return "";
        }

        public override string GetGameVersion()
        {
            return "自定义";
        }

        public override string GetMoreInfo()
        {
            return "F9启停 F12跳节点";
        }

        public override string GetPointEnd()
        {
            return "预计结束 " + base.GetPointEnd();
        }

        public override string GetSecondWatch()
        {
            return "";
        }

        public override string GetSmallWatch()
        {
            return "";
        }

        private GRender.GBtn btnLiteCtrl;
        public override void InitUI()
        {
            form.rr.VisibleBtn(0, false);
            btnLiteCtrl = form.NewMenuButton(0);
            btnLiteCtrl.Text = "开始";
            btnLiteCtrl.OnClicked = delegate (int x, int y, GRender.GBtn ctl)
            {
                OnBtnActive();
            };
        }
        public override void UnloadUI()
        {
            base.UnloadUI();
            form.rr.VisibleBtn(0, true);
        }

        public override bool IsMainWatchStar()
        {
            return false;
        }

        public override bool IsShowC()
        {
            return false;
        }

        public override bool NeedBlockCtrlEnter()
        {
            return false;
        }

        private bool IsCallNext = false;
        public override void OnFunctionKey(int FunNo)
        {
            switch (FunNo)
            {
                case 9:
                    OnBtnActive();
                    break;
                case 12:
                    IsCallNext = true;
                    break;
            }
        }

        protected override void InitCheckPoints()
        {
            LoadBest();
            CheckPoints = new List<CheckPoint>();
            foreach (var kv in Best)
            {
                CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest(kv.Key, kv.Value.BestTS))
                {
                    Check = PointCheck
                });
            }
        }

        protected bool PointCheck()
        {
            if (IsCallNext)
            {
                IsCallNext = false;
                return true;
            }
            return false;
        }

        protected override void OnTick()
        {
            Checking();
        }
        protected override void OnCheckPointEnd()
        {
            MT.Stop();
            IsBegin = false;
            btnLiteCtrl.Text = "开始";
        }

        private bool IsBegin = false;
        private void OnBtnActive()
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
        public override void Reset()
        {
            base.Reset();
            MT.Stop();
            IsCallNext = false;
            IsBegin = false;
            btnLiteCtrl.Text = "开始";

        }
    }
}
