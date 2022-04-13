using HFrame.ENT;
using HFrame.EX;
using HFrame.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pal98Timer
{
    public class 仙剑5前传 : TimerCore
    {
        private GameObject5Q GameObj = new GameObject5Q();
        private string GMD5 = "none";
        public IntPtr PalHandle;
        public IntPtr GameWindowHandle = IntPtr.Zero;
        private int PID = -1;
        private Process PalProcess;
        private bool _HasGameStart = false;
        private bool _IsFirstStarted = false;
        
        private PTimer ST = new PTimer();
        private PTimer LT = new PTimer();
        private DateTime InBattleTime;
        private DateTime OutBattleTime;
        public TimeSpan BattleLong = new TimeSpan(0);
        private bool IsPause = false;

        private bool IsInBattle = false;
        private bool IsDoMoreEndBattle = true;

        private string SelectSword = "";
        private string EndName = "";

        private string cryerror = "";
        private bool IsShowSpeed = false;
        
        //private long PALBaseAddr;
        public 仙剑5前传(GForm form) : base(form)
        {
            CoreName = "PAL5QSTM";
        }
        private string WillAppendNamedBattle = "";
        public override string GetAAction()
        {
            if (WillAppendNamedBattle == "")
            {
                return "";
            }
            else
            {
                string res = WillAppendNamedBattle;
                WillAppendNamedBattle = "";
                return res;
            }
        }
        public override bool IsShowC()
        {
            return false;
        }

        public override string GetCriticalError()
        {
            if (cryerror == "")
            {
                return "";
            }
            else
            {
                string tmp = cryerror;
                cryerror = "";
                return tmp;
            }
        }

        public override string GetGameVersion()
        {
            if (PID != -1)
            {
                return "仙剑5前传Steam";
            }
            else
            {
                return "等待游戏运行";
            }
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
            string deff = "";
            switch (GameObj.Deff)
            {
                case 1:
                    deff = "简单";
                    break;
                case 2:
                    deff = "普通";
                    break;
                case 3:
                    deff = "困难";
                    break;
            }
            if (IsShowSpeed)
            {
                return GameObj.MoveSpeed.ToString("F2") + "  " + deff + "  元神：" + GameObj.OriGod + "  玄武甲：" + MaxXWJ;
            }
            else
            {
                return deff + "  元神：" + GameObj.OriGod + "  玄武甲：" + MaxXWJ;
            }
        }

        public override string GetPointEnd()
        {
            return "预计通关  " + TimeSpanToStringLite(WillClear);
        }

        public override string GetSecondWatch()
        {
            if (ST.CurrentTSOnly.Ticks == 0)
            {
                return "";
            }
            return ST.ToString();
        }

        public override string GetSmallWatch()
        {
            return BattleLong.TotalSeconds.ToString("F2") + "s";
        }

        public void InitCheckPoints_OLD()
        {
            LoadBest();
            _CurrentStep = -1;
            Data = new HObj();
            CheckPoints = new List<CheckPoint>();
            Data["huayao"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("花妖", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("huayao"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6001) > 0)
                        {
                            Data["huayao"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["huayao"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["liyan"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("厉岩", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("liyan"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6016) > 0)
                        {
                            Data["liyan"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["liyan"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["xuenv"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("雪女", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("xuenv"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6004) > 0)
                        {
                            Data["xuenv"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["xuenv"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["loulanwang"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("楼兰王", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("loulanwang"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6031) > 0)
                        {
                            Data["loulanwang"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["loulanwang"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["qiongwu"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("穹武", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("qiongwu"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6040) > 0)
                        {
                            Data["qiongwu"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.BattleResult==6)
                        {
                            Data["qiongwu"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["jieluo"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("结萝", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("jieluo"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6034) > 0)
                        {
                            Data["jieluo"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["jieluo"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["syly"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("神鹰岚翼", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("syly"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6019) > 0)
                        {
                            Data["syly"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["syly"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["shyt"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("三皇一体", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("shyt"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6011) > 0)
                        {
                            Data["shyt"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["shyt"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["gushe"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("骨蛇", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("gushe"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6044) > 0)
                        {
                            Data["gushe"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["gushe"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["yanwu"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("炎舞", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("yanwu"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6014) > 0)
                        {
                            Data["yanwu"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["yanwu"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["xiaohei"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("小黑", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("xiaohei"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6015) > 0)
                        {
                            Data["xiaohei"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["xiaohei"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["dazhaxie"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("噬珊鬼螯", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("dazhaxie"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6021) > 0)
                        {
                            Data["dazhaxie"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["dazhaxie"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["jiangshili"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("姜世离", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("jiangshili"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6045) > 0)
                        {
                            Data["jiangshili"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.BattleResult==6)
                        {
                            Data["jiangshili"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["wood"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("枯木", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("wood"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6026) > 0)
                        {
                            Data["wood"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["wood"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            Data["t"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关节点", new TimeSpan(2, 0, 38)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("t"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6050) > 0)
                        {
                            Data["t"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["t"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
        }
        protected override void InitCheckPoints()
        {
            LoadBest();
            _CurrentStep = -1;
            Data = new HObj();
            CheckPoints = new List<CheckPoint>();
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("花妖", new TimeSpan(0, 16, 51)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp >= 800) return true;
                    return false;
                }
            });
            Data["liyan"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("厉岩", new TimeSpan(0, 23, 10)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("liyan"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6016) > 0)
                        {
                            Data["liyan"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.Enemys.Count <= 0)
                        {
                            Data["liyan"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("雪女", new TimeSpan(0, 33, 27)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp >= 4700) return true;
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("楼兰王", new TimeSpan(0, 55, 53)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp == 10400) return true;
                    return false;
                }
            });
            Data["qiongwu"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("穹武", new TimeSpan(1, 10, 12)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("qiongwu"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6040) > 0)
                        {
                            Data["qiongwu"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.BattleResult == 6)
                        {
                            Data["qiongwu"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("结萝", new TimeSpan(1, 19, 16)))
            {
                Check = delegate ()
                {
                    if (GameObj.Deff == 3)
                    {
                        if (GameObj.IsInBattle && GameObj.BattleEndExp >= 3600) return true;
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.BattleEndExp >= 2000 && GameObj.BattleEndMoney==2400) return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("神鹰岚翼", new TimeSpan(1, 40, 21)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && (GameObj.BattleEndExp == 16000 || GameObj.BattleEndExp == 22800)) return true;
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("三皇一体", new TimeSpan(1, 59, 9)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp >= 32000) return true;
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("骨蛇", new TimeSpan(2, 13, 00)))
            {
                Check = delegate ()
                {
                    if (GameObj.Deff == 3)
                    {
                        if (GameObj.IsInBattle && GameObj.BattleEndMoney == 160 && GameObj.BattleEndExp == 8000) return true;
                    }
                    else
                    {
                        if (GameObj.IsInBattle && GameObj.BattleEndMoney == 240 && GameObj.BattleEndExp == 8000) return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("炎舞", new TimeSpan(2, 40, 11)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp == 63000) return true;
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("小黑", new TimeSpan(2, 51, 45)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp == 90800) return true;
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("噬珊鬼螯", new TimeSpan(3, 9, 45)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp >= 126000) return true;
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("姜世离", new TimeSpan(3, 23, 10)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp == 60000) return true;
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("枯木", new TimeSpan(3, 32, 2)))
            {
                Check = delegate ()
                {
                    if (GameObj.IsInBattle && GameObj.BattleEndExp >= 120000) return true;
                    return false;
                }
            });
            Data["t"] = false;
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关", new TimeSpan(3, 39, 4)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("t"))
                    {
                        if (GameObj.IsInBattle && GameObj.GetEnemyCountByID(6050) > 0)
                        {
                            Data["t"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.BattleResult==6)
                        {
                            Data["t"] = false;
                            return true;
                        }
                    }
                    return false;
                }
            });
        }

        protected override void FillMoreTimerData(HObj exdata)
        {
            exdata["Idle"] = ST.ToString();
            exdata["Lite"] = LT.ToString();
            exdata["GMD5"] = GMD5;
            exdata["MaxXWJ"] = MaxXWJ;
        }
        private ToolStripMenuItem btnGameSpeedShow;
        public override void InitUI()
        {
            var btnExportCurrent = form.NewMenuItem();
            btnExportCurrent.Text = "导出本次成绩";
            btnExportCurrent.Click += delegate (object sender, EventArgs e) {
                ExportCurrent(GetRStr());
            };

            var btnSetCurrentToBest = form.NewMenuItem();
            btnSetCurrentToBest.Text = "设置本次成绩为最佳";
            btnSetCurrentToBest.Click += delegate (object sender, EventArgs e) {
                SaveBest(GetRStr());
            };

            btnGameSpeedShow = form.NewMenuItem();
            btnGameSpeedShow.Text = "显示移动速度";
            btnGameSpeedShow.Checked = false;
            btnGameSpeedShow.Click += delegate (object sender, EventArgs e) {
                btnGameSpeedShow.Checked = !btnGameSpeedShow.Checked;
            };
            btnGameSpeedShow.CheckedChanged += delegate (object sender, EventArgs e) {
                IsShowSpeed = btnGameSpeedShow.Checked;
            };
        }

        public override void OnFunctionKey(int FunNo)
        {
            switch (FunNo)
            {
                case 12:
                    DebugForm df = new DebugForm();
                    df.ShowData(GameObj);
                    df.Show();
                    break;
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaxXWJ = 0;
            HasAlertMutiPal = false;
            ST.Stop();
            _IsFirstStarted = false;
            BattleLong = new TimeSpan(0);
            //InitCheckPoints();
            ST.Reset();
        }

        private string GetGameFilePath(string fn)
        {
            string palpath = PalProcess.MainModule.FileName;
            string[] spli = palpath.Split('\\');
            spli[spli.Length - 1] = fn;
            palpath = "";
            foreach (string s in spli)
            {
                palpath += s + "\\";
            }
            if (palpath != "")
            {
                palpath = palpath.Substring(0, palpath.Length - 1);
            }
            return palpath;
        }
        private void CalcPalMD5()
        {
            try
            {
                string dllmd5 = GetFileMD5(GetGameFilePath("Pal5Q.exe"));
                GMD5 = dllmd5;
            }
            catch
            {
                GMD5 = "none";
            }
        }
        private bool HasAlertMutiPal = false;
        private void JudgePause()
        {
            if (!GameObj.CanControl)
            {
                IsPause = true;
                return;
            }
            if (IsUIPause)
            {
                IsPause = true;
                return;
            }
            IsPause = !this.IsGameWindowFocus();
        }
        private bool IsGameWindowFocus()
        {
            IntPtr hWnd = User32.GetForegroundWindow();    //获取活动窗口句柄  
            int calcID = 0;    //进程ID  
            int calcTD = 0;    //线程ID  
            calcTD = User32.GetWindowThreadProcessId(hWnd, out calcID);
            if (calcID == PID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool GetPalHandle()
        {
            Process[] res = Process.GetProcessesByName("Pal5Q");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个Pal5Q.exe进程，请关闭其他的，只保留一个！";
                    HasAlertMutiPal = true;
                }
                return false;
            }

            HasAlertMutiPal = false;
            if (res.Length > 0)
            {
                PalProcess = res[0];
                GameWindowHandle = res[0].MainWindowHandle;
                PID = PalProcess.Id;
                PalHandle = new IntPtr(Kernel32.OpenProcess(0x1F0FFF, false, PID));
                //PALBaseAddr = PalProcess.MainModule.BaseAddress.ToInt64();
                CalcPalMD5();

                return true;
            }
            else
            {
                PalHandle = IntPtr.Zero;
                GameWindowHandle = IntPtr.Zero;
                PalProcess = null;
                PID = -1;
                GMD5 = "none";
                //PALBaseAddr = -1;
                return false;
            }
        }
        private bool GetPalHandle_old()
        {
            Process[] res = Process.GetProcessesByName("Pal5Q");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个Pal5Q.exe进程，请关闭其他的，只保留一个！";
                    HasAlertMutiPal = true;
                }
                return false;
            }

            HasAlertMutiPal = false;
            if (res.Length > 0)
            {
                if (PID == -1)
                {
                    PalProcess = res[0];
                    GameWindowHandle = res[0].MainWindowHandle;
                    PID = PalProcess.Id;
                    PalHandle = new IntPtr(Kernel32.OpenProcess(0x1F0FFF, false, PID));
                    //PALBaseAddr = PalProcess.MainModule.BaseAddress.ToInt64();
                    CalcPalMD5();

                    return true;
                }
                else
                {
                    if (PID == res[0].Id)
                    {
                        if (GMD5 == "none")
                        {
                            CalcPalMD5();
                        }
                        return true;
                    }
                    else
                    {
                        PalHandle = IntPtr.Zero;
                        GameWindowHandle = IntPtr.Zero;
                        PalProcess = null;
                        PID = -1;
                        GMD5 = "none";
                        //PALBaseAddr = -1;
                        return false;
                    }
                }
            }
            else
            {
                PalHandle = IntPtr.Zero;
                GameWindowHandle = IntPtr.Zero;
                PalProcess = null;
                PID = -1;
                GMD5 = "none";
                //PALBaseAddr = -1;
                return false;
            }
        }
        private bool HasStartGame()
        {
            if (!_HasGameStart)
            {
                if (GameObj.MapID != 0)
                {
                    _HasGameStart = true;
                    if (IsPause)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (IsPause)
                {
                    return false;
                }
                return true;
            }
        }
        protected override void OnTick()
        {
            if (GetPalHandle())
            {

                JudgePause();
                try
                {
                    FlushGameObject();
                }
                catch (Exception ex)
                {
                }


                try
                {
                    if (GameObj.IsInBattle)
                    {
                        if (!IsInBattle)
                        {
                            BattleBegin();
                        }
                        IsInBattle = true;
                        IsDoMoreEndBattle = true;
                        Battling();
                    }
                    else
                    {
                        if (!IsDoMoreEndBattle)
                        {
                            BattleEndMore();
                            IsDoMoreEndBattle = true;
                        }
                        if (IsInBattle)
                        {
                            BattleEnd();
                            IsDoMoreEndBattle = false;
                        }
                        IsInBattle = false;
                    }
                }
                catch { }

                if (HasStartGame())
                {
                    ST.Stop();
                    if (!_IsFirstStarted)
                    {
                        _IsFirstStarted = true;
                    }
                    MT.Start();
                    Checking();
                }
                else
                {
                    MT.Stop();
                }
            }
            else
            {
                _HasGameStart = false;
                MT.Stop();

                if (_IsFirstStarted)
                {
                    ST.Start();
                }
            }
        }
        private int beforeMoney = 0;
        private void BattleBegin()
        {
            beforeMoney = GameObj.Money;
            BattleLong = new TimeSpan(0);
            InBattleTime = DateTime.Now;
        }
        private void Battling()
        {
            BattleLong = DateTime.Now - InBattleTime;
        }
        private int MaxXWJ = 0;
        private void BattleEnd()
        {
            OutBattleTime = DateTime.Now;
            BattleLong = OutBattleTime - InBattleTime;
            //统计一下玄武甲获得量

            if (GameObj.Money != beforeMoney)
            {
                if (GameObj.BattleEndItemGet.ContainsKey(77))
                {
                    MaxXWJ += GameObj.BattleEndItemGet[77];
                }
            }
        }
        private void BattleEndMore()
        {
        }
        private void FlushGameObject()
        {
            GameObj.Flush(PalHandle, PID, 0, 0);
            FlushPlugins(PalHandle, PID, 0, 0);
        }
        public override bool NeedBlockCtrlEnter()
        {
            return false;
        }
    }
    public class GameObject5Q:MemoryReadBase
    {
        //6001  花妖      E=800
        //6016  厉岩      E=0
        //6004  雪女      E>=4700
        //6031  楼兰王    E=10400
        //6040  穹武      E=0
        //6034  结萝（难）E>=3600
        //6034  结萝（简）E>=2000 G=2400
        //6019  神鹰岚翼  E=22800|16000
        //6011  三皇一体  E=32000
        //6044  骨蛇（难）E=8000  G=160
        //6044  骨蛇（简）E=8000  G=240
        //6014  炎舞      E=63000
        //6015  小黑      E=90800
        //6021  噬珊鬼螯  E>=126000
        //6045  姜世离     E=60000
        //6026  枯木      E>=120000
        //6041  魔化瑕     
        //6050  通关      E=0
        public const int BaseAddrPTR = 0x400000;
        public const int MapIDOffset = 0x6C0178;
        public const int XOffset = 0x668500;
        public const int YOffset = 0x668504;
        public const int ZOffset = 0x668508;
        public const int BattleResultOffset = 0x6683F4;
        public const int InBattleOffset = 0x668538;
        public const int FirstEnemyPTROffset = 0x21AB8D8;
        public const int OurCountOffset = 0x668524;
        public const int FirstOurPTROffset = 0x668518;
        public const int OriGodOffSet = 0x6684D0;
        public const int BattleEndGetItemHeadPTROffset = 0x668410;
        public const int BattleEndGetItemTypeCountOffset = 0x668414;
        public const int MoneyOffset = 0x668564;
        public const int BattleEndMoneyOffset= 0x668408;
        public const int BattleEndExpOffset = 0x66840C;
        public const int DeffOffset0 = 0x65DDC0;
        public const int DeffOffset1 = 0x1F0;
        public const int MoveSpeedOffset0 = 0x668538;
        public const int MoveSpeedOffset1 = 0x444;
        public const int MoveSpeedOffset2 = 0x3C;

        //玄武甲ID：77

        private IntPtr handle;
        private int PID;
        public int BaseAddr = 0x400000;

        public bool CanControl = true;
        public bool IsInBattle = false;
        public int MapID = 0;
        public float X = 0.0F;
        public float Y = 0.0F;
        public float Z = 0.0F;
        public int BattleResult = 0;
        private int InBattle = 1;
        public List<EnemyObj> Enemys = new List<EnemyObj>();
        public int EnemyTotalHP = 0;
        private int OurCount = 0;
        private int OurTotalHP = 0;
        public int OriGod = 0;
        public int Money = 0;
        public int BattleEndMoney = 0;
        public int BattleEndExp = 0;
        public int Deff = 1;//1简单 2普通 3困难
        public float MoveSpeed = 0.0F;

        public override string ToString()
        {
            string tmp = "难度"+Deff+"\r\n";
            tmp += "ys:" + OriGod + "\r\n";
            tmp += "map:" + MapID + "\r\n";
            tmp += "x:" + X + "\r\n";
            tmp += "y:" + Y + "\r\n";
            tmp += "z:" + Z + "\r\n";
            tmp += "移动速度：" + MoveSpeed.ToString("F2") + "\r\n";
            tmp += "\r\n";
            tmp += "战斗中:" + IsInBattle + " (" + BattleResult + ")\r\n";
            tmp += "our:" + OurTotalHP + "\r\n";
            tmp += "emeny("+ Enemys.Count + "):"+ EnemyTotalHP + "\r\n\r\n";
            foreach (var eo in Enemys)
            {
                tmp += eo.ID + ":\t" + eo.HP + "\r\n";
            }

            return tmp;
        }

        public int GetEnemyCountByID(int ID)
        {
            if (IsInBattle)
            {
                if (Enemys.Count > 0)
                {
                    int ret = 0;
                    foreach (EnemyObj eo in Enemys)
                    {
                        if (eo.ID == ID) ret++;
                    }
                    return ret;
                }
            }
            return 0;
        }

        public override void Flush(IntPtr handle, int PID,int b32,long b64)
        {
            if (PID != this.PID)
            {
                this.PID = PID;
                this.handle = handle;
            }
            //BaseAddr = Readm<int>(this.handle, BaseAddrPTR);
            int map = Readm<int>(this.handle, BaseAddr + MapIDOffset);
            if (map >= 0 && map < 5000)
            {
                MapID = map;
            }
            else
            {
                MapID = 0;
            }
            int tmpm = Readm<int>(this.handle, BaseAddr + DeffOffset0);
            Deff = Readm<int>(this.handle, tmpm + DeffOffset1);
            tmpm = Readm<int>(this.handle, BaseAddr + MoveSpeedOffset0);
            tmpm = Readm<int>(this.handle, tmpm + MoveSpeedOffset1);
            MoveSpeed = Readm<float>(this.handle, tmpm + MoveSpeedOffset2);

            Money = Readm<int>(this.handle, BaseAddr + MoneyOffset);
            BattleEndExp = Readm<int>(this.handle, BaseAddr + BattleEndExpOffset);
            BattleEndMoney = Readm<int>(this.handle, BaseAddr + BattleEndMoneyOffset);
            FlushBattleEndItem(handle);
            OriGod = Readm<int>(this.handle, BaseAddr + OriGodOffSet);
            X = Readm<float>(this.handle, BaseAddr + XOffset);
            Y = Readm<float>(this.handle, BaseAddr + YOffset);
            Z = Readm<float>(this.handle, BaseAddr + ZOffset);
            OurCount = Readm<int>(this.handle, BaseAddr + OurCountOffset);
            BattleResult= Readm<int>(this.handle, BaseAddr + BattleResultOffset);
            InBattle = Readm<int>(this.handle, BaseAddr + InBattleOffset);
            IsInBattle = (MapID != 0 && InBattle == 0);
            List<EnemyObj> tmp = new List<EnemyObj>();
            int totalhp = 0;
            int ohp = 0;
            if (IsInBattle)
            {
                for (int i = 0; i < 20; ++i)
                {
                    EnemyObj eo = new EnemyObj(this.handle,BaseAddr + FirstEnemyPTROffset + EnemyObj.PTROffLength * i);
                    if (eo.ID >= 5000 && eo.ID<9999 && eo.HP > 0)
                    {
                        tmp.Add(eo);
                        totalhp += eo.HP;
                    }
                }
                int ourheadaddr = Readm<int>(this.handle, BaseAddr + FirstOurPTROffset);
                for (int i = 0; i < OurCount; ++i)
                {
                    int ad1 = Readm<int>(this.handle, ourheadaddr + i * 4);
                    int hp = Readm<int>(this.handle, ad1 + 0x52c);
                    if (hp >= 0) ohp += hp;
                }
            }
            OurTotalHP = ohp;
            EnemyTotalHP = totalhp;
            Enemys = tmp;
        }

        public Dictionary<int, int> BattleEndItemGet = new Dictionary<int, int>();
        private void FlushBattleEndItem(IntPtr handle)
        {
            Dictionary<int, int> tmp = new Dictionary<int, int>();
            int GetItemTypeCount = Readm<int>(handle, BaseAddr + BattleEndGetItemTypeCountOffset);
            int headattr = Readm<int>(handle, BaseAddr + BattleEndGetItemHeadPTROffset);
            for (int i = 0; i < GetItemTypeCount; ++i)
            {
                int curaddr = Readm<int>(handle, headattr + i * 4);
                int id = Readm<int>(handle, curaddr + 16);
                int count = Readm<int>(handle, curaddr + 20);
                if (tmp.ContainsKey(id))
                {
                    tmp[id] = count;
                }
                else
                {
                    tmp.Add(id, count);
                }
            }
            BattleEndItemGet = tmp;
        }

        public class EnemyObj
        {
            public const int PTROffLength = 4;
            public const int IDOffset = 0x53c;
            public const int HPOffset = 0x558;

            public int ID;
            public int HP;
            public EnemyObj(IntPtr handle,int addr)
            {
                this.Flush(handle,addr);
            }
            public EnemyObj()
            {
            }
            public void Flush(IntPtr handle,int addr)
            {
                int a = Readm<int>(handle, addr);
                ID = Readm<int>(handle, a + IDOffset);
                HP = Readm<int>(handle, a + HPOffset);
            }
        }
    }
}
