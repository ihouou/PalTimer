using System;
using System.Collections.Generic;
using System.IO;
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
            return "F9启停 Ctrl+F12上一点 F12下一点";
        }

        public override string GetPointEnd()
        {
            return "预计结束 " + GetWillClearStr();
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
        private GRender.GBtn btnJumpBtn;
        public override void InitUI()
        {
            form.rr.VisibleBtn(0, false);
            btnLiteCtrl = form.NewMenuButton(0);
            btnLiteCtrl.Text = "开始";
            btnLiteCtrl.OnClicked = delegate (int x, int y, GRender.GBtn ctl)
            {
                OnBtnActive();
            };

            btnJumpBtn = form.NewMenuButton(1);
            btnJumpBtn.Text = "跳点";
            btnJumpBtn.OnClicked = delegate (int x, int y, GRender.GBtn ctl)
              {
                  IsCallNext = true;
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
                    if (form.OnCtrlDown || form.OnCtrlDown2)
                    {
                        if (CurrentStep > 0)
                        {
                            CurrentStep = CurrentStep - 1;
                        }
                    }
                    else
                    {
                        IsCallNext = true;
                    }
                    break;
            }
        }

        protected override void InitCheckPoints()
        {
            string fn = "best" + CoreName + ".txt";
            if (!File.Exists(fn))
            {
                using (FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write("{\"CheckPoints\":[{\"name\":\"测试1\",\"des\":\"\",\"time\":\"00:01:00.00\"},{\"name\":\"测试2\",\"des\":\"\",\"time\":\"00:02:00.00\"},{\"name\":\"测试3\",\"des\":\"\",\"time\":\"00:03:00.00\"},{\"name\":\"测试4\",\"des\":\"\",\"time\":\"00:04:00.00\"},{\"name\":\"测试5\",\"des\":\"\",\"time\":\"00:05:00.00\"}]}");
                    }
                }
            }
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
            base.OnCheckPointEnd();
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
