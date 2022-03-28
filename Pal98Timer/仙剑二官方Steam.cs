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
    public class 仙剑二官方Steam : TimerCore
    {
        private Pal2SteamObject GameObj = new Pal2SteamObject();
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
        
        private long PALBaseAddr;
        public 仙剑二官方Steam(GForm form) : base(form)
        {
            CoreName = "PAL2STM";
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
                return "仙剑2Steam";
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
            return "按F9手动暂停";
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

        protected override void InitCheckPoints()
        {
            LoadBest();
            _CurrentStep = -1;
            Data = new HObj();
            CheckPoints = new List<CheckPoint>();
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("协查", new TimeSpan(0, 6, 59)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.xiecha && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("石版四", new TimeSpan(0, 19, 20)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.shiban4 && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("沈齐", new TimeSpan(0, 36, 02)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.shenqi && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("飏枭", new TimeSpan(0, 43, 51)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.yangxiao && (GameObj.EnemyCount == 0 || GameObj.OurCount == 0))
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("江都王", new TimeSpan(0, 49, 09)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.jiangduwang && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("虞蛇", new TimeSpan(0, 59, 06)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.yushe && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("武林人士", new TimeSpan(1, 2, 16)))
            {
                Check = delegate ()
                {
                    if (GameObj.MapID==104 && GameObj.X==1600 && GameObj.Y==186)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("蜈王", new TimeSpan(1, 4, 20)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.wuwang && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("鹿妖", new TimeSpan(1, 7, 30)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.luyao && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("石头人", new TimeSpan(1, 14, 48)))
            {
                Check = delegate ()
                {
                    if ((GameObj.BattleID == Pal2SteamObject.EBattle.shitouren) && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("天鬼兄妹", new TimeSpan(1, 18, 24)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.tianguixiongmei && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("秦儒", new TimeSpan(1, 22, 17)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.qinru && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("火画妖", new TimeSpan(1, 27, 45)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.huohuayao && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("四画妖", new TimeSpan(1, 32, 00)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.sihuayao && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("大画妖", new TimeSpan(1, 35, 24)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.dahuayao && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("孔磷", new TimeSpan(1, 36, 37)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.konglin && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("山猪婆", new TimeSpan(1, 42, 22)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.shanzhupo && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("猫妖", new TimeSpan(1, 48, 36)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.maoyao && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("喻南松", new TimeSpan(1, 50, 04)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.yunansong && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("千叶", new TimeSpan(2, 0, 38)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == Pal2SteamObject.EBattle.qianye && GameObj.EnemyCount == 0)
                    {
                        WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2") + "s";
                        return true;
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
        }
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
        }

        public override void OnFunctionKey(int FunNo)
        {
            switch (FunNo)
            {
                case 12:
                    /*DebugForm df = new DebugForm();
                    df.ShowData(GameObj);
                    df.Show();*/
                    break;
            }
        }

        public override void Reset()
        {
            base.Reset();
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
                string dllmd5 = GetFileMD5(GetGameFilePath("Pal2_x64.exe"));
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
        private bool GetPalHandle() {
            Process[] res = Process.GetProcessesByName("Pal2_x64");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个Pal2_x64.exe进程，请关闭其他的，只保留一个！";
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
                PALBaseAddr = PalProcess.MainModule.BaseAddress.ToInt64();
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
                PALBaseAddr = -1;
                return false;
            }
        }
        private bool GetPalHandle_old()
        {
            Process[] res = Process.GetProcessesByName("Pal2_x64");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个Pal2_x64.exe进程，请关闭其他的，只保留一个！";
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
                    PALBaseAddr = PalProcess.MainModule.BaseAddress.ToInt64();
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
                        PALBaseAddr = -1;
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
                PALBaseAddr = -1;
                return false;
            }
        }
        private bool HasStartGame()
        {
            if (!_HasGameStart)
            {
                if (GameObj.MapID!=0)
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
        private void BattleBegin()
        {
            BattleLong = new TimeSpan(0);
            InBattleTime = DateTime.Now;
        }
        private void Battling()
        {
            BattleLong = DateTime.Now - InBattleTime;
        }
        private void BattleEnd()
        {
            OutBattleTime = DateTime.Now;
            BattleLong = OutBattleTime - InBattleTime;
        }
        private void BattleEndMore()
        {
        }
        private void FlushGameObject()
        {
            GameObj.Flush(PalHandle, PID, PALBaseAddr);
        }
        public override bool NeedBlockCtrlEnter()
        {
            return this.IsGameWindowFocus();
        }
    }

    public class Pal2SteamObject
    {
        //private const int BaseAddr = 0x400000;
        private const int MoneyOffset = 0x482158;
        private const int WXHHPOffset = 0x48176c;
        private const int LYRHPOffset = 0x48176c + 0x28c + 0x28c + 0x28c;
        private const int SMHPOffset = 0x48176c + 0x28c + 0x28c;
        private const int SQSHPOffset = 0x48176c + 0x28c;
        private static readonly int[] BattleObjCountOffset = new int[] { 0x274FB0, -0x80 };
        private static readonly int[] BattleNPCCountOffset = new int[] { 0x274FB0, -0x18 };
        private const int OpenGameTimelenOffset = 0x482178;
        private const int MapIDOffset = 0x493380;
        private const int CamXOffset = 0x493388;
        private const int CamYOffset = 0x49338c;
        private const int PvCXOffset = 0x482a5c;
        private const int PvCYOffset = 0x483ef0;
        private const int BGMOffset = 0x49e8a0;
        private const int MapTimelenOffset = 0x492c48;
        private static readonly int[][] BOSlotsIDOffset = new int[][] {
            new int[]{ 0x274FB0, -0x78, 0x20},
            new int[]{ 0x274FB0, -0x70, 0x20},
            new int[]{ 0x274FB0, -0x68, 0x20},
            new int[]{ 0x274FB0, -0x60, 0x20},
            new int[]{ 0x274FB0, -0x58, 0x20},
            new int[]{ 0x274FB0, -0x50, 0x20},
            new int[]{ 0x274FB0, -0x48, 0x20},
            new int[]{ 0x274FB0, -0x40, 0x20},
            new int[]{ 0x274FB0, -0x38, 0x20},
            new int[]{ 0x274FB0, -0x30, 0x20},
            new int[]{ 0x274FB0, -0x28, 0x20},
        };
        private static readonly int[][] BOSlotsStatusOffset = new int[][] {
            new int[]{ 0x274FB0, -0x78, 0},
            new int[]{ 0x274FB0, -0x70, 0},
            new int[]{ 0x274FB0, -0x68, 0},
            new int[]{ 0x274FB0, -0x60, 0},
            new int[]{ 0x274FB0, -0x58, 0},
            new int[]{ 0x274FB0, -0x50, 0},
            new int[]{ 0x274FB0, -0x48, 0},
            new int[]{ 0x274FB0, -0x40, 0},
            new int[]{ 0x274FB0, -0x38, 0},
            new int[]{ 0x274FB0, -0x30, 0},
            new int[]{ 0x274FB0, -0x28, 0},
        };
        

        private int Money = 0;
        private int BattleObjCount = 0;
        private int BattleNPCCount = 0;
        private int OpenGameTimelen = 0;
        public int MapID = 0;
        private int CamX = 0;
        private int CamY = 0;
        public int X = 0;
        public int Y = 0;
        private int BGM = 0;
        private int MapTimelen = 0;
        private List<BattleObj> OurObjs;
        private List<BattleObj> YLObjs;
        private List<BattleObj> EnemyObjs;
        
        public Pal2SteamObject() {
        }

        private IntPtr handle;
        private int PID;
        private long BaseAddr;
        public void Flush(IntPtr handle, int PID, long PALBaseAddr)
        {
            if (PID != this.PID)
            {
                this.PID = PID;
            }
            this.handle = handle;

            BaseAddr = PALBaseAddr;
            Money = Readm<int>(this.handle, BaseAddr + MoneyOffset);
            OpenGameTimelen= Readm<int>(this.handle, BaseAddr + OpenGameTimelenOffset);
            MapID= Readm<int>(this.handle, BaseAddr + MapIDOffset);
            CamX= Readm<int>(this.handle, BaseAddr + CamXOffset);
            CamY= Readm<int>(this.handle, BaseAddr + CamYOffset);
            X=CamX+ Readm<int>(this.handle, BaseAddr + PvCXOffset);
            Y= CamY+ Readm<int>(this.handle, BaseAddr + PvCYOffset);
            BGM= Readm<int>(this.handle, BaseAddr + BGMOffset);
            MapTimelen= Readm<int>(this.handle, BaseAddr + MapTimelenOffset);
            BattleObjCount = Readm<int>(this.handle, BaseAddr, BattleObjCountOffset);
            BattleNPCCount = Readm<int>(this.handle, BaseAddr, BattleNPCCountOffset);
            this.IsInBattle = this.BattleObjCount > 0;
            if (IsInBattle)
            {
                _flushOur();
                this.OurCount = this.OurObjs.Count;
                _flushYL();
                this.YLCount = this.YLObjs.Count;
                _flushEnemy();
                this.EnemyCount = this.EnemyObjs.Count;
            }
            else
            {
                this.OurCount = 0;
                this.YLCount = 0;
                this.EnemyCount = 0;
                this.BattleName = "";
                this.BattleID = EBattle.none;
            }
            this.SetFastSpeakIfCan();
        }
        private void _flushOur() {
            List<BattleObj> tmp = new List<BattleObj>();

            for (var i = 0; i < 4; ++i)
            {
                BattleObj bo = new BattleObj();
                bo.ID = Readm<int>(this.handle, BaseAddr, BOSlotsIDOffset[i]);
                if (bo.ID >= 2001 && bo.ID <= 2004)
                {
                    bo.Status=Readm<ushort>(this.handle, BaseAddr, BOSlotsStatusOffset[i]);
                    switch (bo.ID)
                    {
                        case WXHid:
                            bo.HP= Readm<int>(this.handle, BaseAddr + WXHHPOffset);
                            break;
                        case LYRid:
                            bo.HP = Readm<int>(this.handle, BaseAddr + LYRHPOffset);
                            break;
                        case SMid:
                            bo.HP = Readm<int>(this.handle, BaseAddr + SMHPOffset);
                            break;
                        case SQSid:
                            bo.HP = Readm<int>(this.handle, BaseAddr + SQSHPOffset);
                            break;
                    }
                    if (bo.HP > 0)
                    {
                        tmp.Add(bo);
                    }
                }
            }

            OurObjs = tmp;
        }
        private void _flushYL() {
            List<BattleObj> tmp = new List<BattleObj>();
            for (var i = 4; i < 6; ++i)
            {
                BattleObj bo = new BattleObj();
                bo.ID = Readm<int>(this.handle, BaseAddr, BOSlotsIDOffset[i]);
                if (bo.ID >= 2005 && bo.ID <= 2010)
                {
                    bo.Status = Readm<ushort>(this.handle, BaseAddr, BOSlotsStatusOffset[i]);
                    if (bo.Status == 51608)
                    {
                        tmp.Add(bo);
                    }
                }
            }
            YLObjs = tmp;
        }
        private void _flushEnemy()
        {
            List<BattleObj> alive = new List<BattleObj>();
            List<BattleObj> all = new List<BattleObj>();
            for (var i = 6; i < 11; i++)
            {
                BattleObj bo = new BattleObj();
                bo.ID = Readm<int>(this.handle, BaseAddr, BOSlotsIDOffset[i]);
                if (bo.ID > 2010 && bo.ID < 2346)
                {
                    bo.Status = Readm<ushort>(this.handle, BaseAddr, BOSlotsStatusOffset[i]);
                    all.Add(bo);
                    if (bo.Status == 51608)
                    {
                        alive.Add(bo);
                    }
                }
            }
            EnemyObjs = alive;
            if (this.BattleID == EBattle.none || this.BattleID == EBattle.konglin)
            {
                if (alive.Count > 0 && all.Count > 0)
                {
                    this._anabattle(all);
                }
            }
        }
        private void _anabattle(List<BattleObj> all)
        {
            if (GetEnemyCount(all, 2301) == 1)
            {
                this.BattleID = EBattle.xiecha;
                this.BattleName = "查协";
            }
            else if (GetEnemyCount(all, 2305) == 1)
            {
                this.BattleID = EBattle.shiban4;
                this.BattleName = "石版四";
            }
            else if (GetEnemyCount(all, 2334) == 1)
            {
                this.BattleID = EBattle.shenqi;
                this.BattleName = "沈齐";
            }
            else if (GetEnemyCount(all, 2307) == 1)
            {
                this.BattleID = EBattle.yangxiao;
                this.BattleName = "杨枭";
            }
            else if (GetEnemyCount(all, 2308) == 1)
            {
                this.BattleID = EBattle.jiangduwang;
                this.BattleName = "江都王";
            }
            else if (GetEnemyCount(all, 2310) == 1 || GetEnemyCount(all, 2340) == 1)
            {
                this.BattleID = EBattle.yushe;
                this.BattleName = "虞蛇";
            }
            else if (GetEnemyCount(all, 2336) == 1)
            {
                this.BattleID = EBattle.wulinrenshi;
                this.BattleName = "武林人士";
            }
            else if (GetEnemyCount(all, 2311) == 1)
            {
                this.BattleID = EBattle.wuwang;
                this.BattleName = "蜈王";
            }
            else if (GetEnemyCount(all, 2312) == 1)
            {
                this.BattleID = EBattle.luyao;
                this.BattleName = "鹿妖";
            }
            else if (GetEnemyCount(all, 2313) == 1)
            {
                this.BattleID = EBattle.shitouren;
                this.BattleName = "石头人";
            }
            else if (GetEnemyCount(all, 2219) == 1)
            {
                this.BattleID = EBattle.tianguixiongmei;
                this.BattleName = "天鬼兄妹";
            }
            else if (GetEnemyCount(all, 2317) == 1)
            {
                this.BattleID = EBattle.qinru;
                this.BattleName = "秦儒";
            }
            else if (GetEnemyCount(all, 2319) == 1)
            {
                this.BattleID = EBattle.huohuayao;
                this.BattleName = "火画妖";
            }
            else if (GetEnemyCount(all, 2319) == 2 && GetEnemyCount(all, 2320) == 2)
            {
                this.BattleID = EBattle.sihuayao;
                this.BattleName = "四画妖";
            }
            else if (GetEnemyCount(all, 2322) == 1)
            {
                this.BattleID = EBattle.dahuayao;
                this.BattleName = "大画妖";
            }
            else if (GetEnemyCount(all, 2323) == 1)
            {
                this.BattleID = EBattle.konglin;
                this.BattleName = "孔磷";
            }
            else if (GetEnemyCount(all, 2324) == 1 || GetEnemyCount(all, 2325) == 1)
            {
                this.BattleID = EBattle.shanzhupo;
                this.BattleName = "山猪婆";
            }
            else if (GetEnemyCount(all, 2326) == 1)
            {
                this.BattleID = EBattle.maoyao;
                this.BattleName = "猫妖";
            }
            else if (GetEnemyCount(all, 2327) == 1)
            {
                this.BattleID = EBattle.yunansong;
                this.BattleName = "喻南松";
            }
            else if (GetEnemyCount(all, 2333) == 1)
            {
                this.BattleID = EBattle.qianye;
                this.BattleName = "千叶";
            }
            else
            {
                this.BattleID = EBattle.normal;
                this.BattleName = "普通";
            }
        }

        private int GetEnemyCount(List<BattleObj> lst,int findID) {
            int ret = 0;
            foreach (var bo in lst)
            {
                if (bo.ID == findID) ret++;
            }
            return ret;
        }

        //private static readonly int[] BOSSid = new int[] { 2301,2305,2334,2307 };
        /*
查协  	2301!
石版四	2302 2303 2304 2305!
沈齐	    2334!
杨枭  	2307* 战斗结束即可
江都王	2308! &2309
虞蛇  	2310!->2340!
武林人士[2338,2345][2337,2335][2336*10回合] a104,x1600,y186
蜈王  	2311!
鹿妖	    2312!
石头人	2313!
天鬼兄妹 2219!
秦儒	    2317!
火画妖	2319!
四画妖	2319! 2319! 2320! 2320!
大画妖	2322!
孔磷  	2323!
山猪婆	2324! 2325!
猫妖	    2326!
喻南松	2327!
千叶	    2333!
             */
        private static readonly int[] YLid = new int[] { 2005, 2006, 2007, 2008, 2009, 2010 };
        private const int WXHid = 2001;
        private const int LYRid = 2002;
        private const int SMid = 2003;
        private const int SQSid = 2004;

        public bool CanControl = true;
        public bool IsInBattle = false;
        public int EnemyCount = 0;
        public int YLCount = 0;
        public int OurCount = 0;
        public string BattleName = "";
        public enum EBattle
        {
            none,
            normal,
            xiecha,
            shiban4,
            shenqi,
            yangxiao,
            jiangduwang,
            yushe,
            wulinrenshi,
            wuwang,
            luyao,
            shitouren,
            tianguixiongmei,
            qinru,
            huohuayao,
            sihuayao,
            dahuayao,
            konglin,
            shanzhupo,
            maoyao,
            yunansong,
            qianye
        }
        public EBattle BattleID = EBattle.none;

        public override string ToString()
        {
            return "A:" + MapID + " X:" + X + " Y:" + Y + "\r\n" +
                "钱:"+Money+"\r\n"+
                "BGM:"+BGM+"\r\n\r\n"+
                "战斗中：" + (this.IsInBattle ? "是" : "否") + "\r\n" +
                "敌人数量：" + this.EnemyCount + " " + this.BattleName + "\r\n" +
                "自己存活：" + this.OurCount + "\r\n" +
                "御灵数量：" + this.YLCount;
        }

        private bool isIn(int id, int[] arr)
        {
            foreach (int i in arr)
            {
                if (i == id) return true;
            }
            return false;
        }

        public static T Readm<T>(IntPtr handle, long baseaddr, int[] offset)
        {
            long addr = baseaddr;
            for (var i = 0; i < offset.Length-1; ++i)
            {
                addr = Readm<long>(handle, addr + offset[i]);
            }
            return Readm<T>(handle, addr + offset[offset.Length - 1]);
        }
        public static T Readm<T>(IntPtr handle, long addr)
        {
            T res = default(T);
            Type t = typeof(T);
            int size = 0;
            if (t.Name == "String")
            {
                size = 1024;
            }
            else
            {
                size = System.Runtime.InteropServices.Marshal.SizeOf(t);
            }
            byte[] buffer = new byte[size];
            int sizeofRead;

            if (Kernel32.ReadProcessMemory(handle, new IntPtr(addr), buffer, size, out sizeofRead))
            {
                if (t == typeof(short))
                {
                    short tmp = BitConverter.ToInt16(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(ushort))
                {
                    ushort tmp = BitConverter.ToUInt16(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(byte))
                {
                    byte tmp = buffer[0];
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(int))
                {
                    int tmp = BitConverter.ToInt32(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(long))
                {
                    long tmp = BitConverter.ToInt64(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(bool))
                {
                    bool tmp = BitConverter.ToBoolean(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(double))
                {
                    double tmp = BitConverter.ToDouble(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(float))
                {
                    float tmp = BitConverter.ToSingle(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sizeofRead; ++i)
                    {
                        //byte b = buffer[i];
                        char c = (char)buffer[i];
                        if (c == '\0')
                        {
                            res = (T)Convert.ChangeType(sb.ToString(), t);
                            break;
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                }
            }

            return res;
        }

        public void SetFastSpeakIfCan()
        {
            byte[] readbuf = new byte[3];
            int sizeofRead;
            try
            {
                if (Kernel32.ReadProcessMemory(handle, new IntPtr(BaseAddr + 0x99AEA), readbuf, 3, out sizeofRead))
                {
                    if (readbuf[0] == 0x8B && readbuf[1] == 0x45 && readbuf[2] == 0xA8)
                    {
                        byte[] writebuf = new byte[3] { 0x33, 0xC0, 0x90 };
                        int sizeofWrite;
                        Kernel32.WriteProcessMemory(handle, new IntPtr(BaseAddr + 0x99AEA), writebuf, 3, out sizeofWrite);
                    }
                }
            }
            catch { }
        }
        public class BattleObj
        {
            public int ID;
            public ushort Status;
            public int HP;
        }
    }
}
