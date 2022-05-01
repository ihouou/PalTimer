using HFrame.ENT;
using HFrame.EX;
using HFrame.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Pal98Timer
{
    public class 古剑2官方 : TimerCore
    {

        private GameObjectGuJian2 GameObj = new GameObjectGuJian2();
        private string GMD5 = "none";
        public IntPtr PalHandle;
        public IntPtr GameWindowHandle = IntPtr.Zero;
        private int PID = -1;
        private Process PalProcess;
        private bool _HasGameStart = false;
        private bool _IsFirstStarted = false;
        private int ExeBaseAddr = -1;
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
        
        //private long PALBaseAddr;
        public 古剑2官方(GForm form) : base(form)
        {
            CoreName = "GUJIAN2";
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
                if (WillAppendNamedBattle.IndexOf(":0.00") > 0) return "";
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
                return "古剑奇谭2官方版";
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
            int m = GameObj.Money;
            int tong = m % 100;
            m = m / 100;
            int yin = m % 100;
            m = m / 100;

            return m + "金  乐Lv." + GameObj.YWYLevel + (CurrentStep <= 9 ? (" 返:" + GameObj.QFLSCount) : "");
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
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("太乙神兵", new TimeSpan(0, 4, 12)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.太乙神兵 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("慧明", new TimeSpan(0, 17, 7)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.慧明 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("金砖", new TimeSpan(0, 21, 31)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.金砖 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("灵虚", new TimeSpan(0, 27, 52)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.灵虚 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("偃甲将军", new TimeSpan(0, 38, 40)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.偃甲将军 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("雩风", new TimeSpan(0, 45, 10)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.雩风 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("尸兽厌火", new TimeSpan(1, 10, 1)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.尸兽厌火 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("安尼瓦尔", new TimeSpan(1, 16, 18)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.安尼瓦尔 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("流月守卫头领", new TimeSpan(1, 30, 3)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.流月城守卫头领 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("远古石兽", new TimeSpan(1, 46, 11)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.远古石兽 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("火龙伏英", new TimeSpan(1, 58, 38)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.火龙伏英 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("怒霜天君", new TimeSpan(2, 5, 50)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.怒霜天君 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("温留", new TimeSpan(2, 10, 27)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.温留 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("蜃精玉怜", new TimeSpan(2, 23, 57)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.蜃精玉怜 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("初七", new TimeSpan(2, 45, 37)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.初七 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("程廷钧", new TimeSpan(2, 48, 44)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.程廷钧 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("魔化华月", new TimeSpan(3, 3, 39)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.魔化华月 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("沈夜变身", new TimeSpan(3, 10, 6)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.沈夜变身 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关", new TimeSpan(3, 14, 33)))
            {
                Check = delegate ()
                {
                    if (GameObj.BattleID == GameObjectGuJian2.EBattle.砺罂 && GameObj.IsWin())
                    {
                        WillAppendNamedBattle = GameObj.BattleID.ToString() + ":" + BattleLong.TotalSeconds.ToString("F2");
                        GameObj.BattleID = GameObjectGuJian2.EBattle.none;
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
                    DebugForm df = new DebugForm();
                    df.ShowData(GameObj);
                    df.Show();
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
                string dllmd5 = GetFileMD5(GetGameFilePath("GuJian2.exe"));
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
            if (GameObj.IsLoading)
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
            Process[] res = Process.GetProcessesByName("GuJian2");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个GuJian2.exe进程，请关闭其他的，只保留一个！";
                    HasAlertMutiPal = true;
                }
                return false;
            }

            HasAlertMutiPal = false;
            if (res.Length > 0)
            {
                PalProcess = res[0];
                GameWindowHandle = res[0].MainWindowHandle;
                if (PID != PalProcess.Id || ExeBaseAddr<=0)
                {
                    ExeBaseAddr = GetX86ModuleBaseAddr(PalProcess.Id, "TRGame.vPlugin");
                    /*foreach (ProcessModule pm in PalProcess.Modules)
                    {
                        if (pm.ModuleName == "TRGame.vPlugin")
                        {
                            ExeBaseAddr = pm.BaseAddress.ToInt32();
                            break;
                        }
                    }*/
                }
                PID = PalProcess.Id;
                PalHandle = new IntPtr(Kernel32.OpenProcess(0x1F0FFF, false, PID));
                
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
        private bool HasStartGame()
        {
            if (!_HasGameStart)
            {
                if (GameObj.CanControl)
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

            if (GameObj.IsWin() && GameObj.BattleName != "")
            {
                WillAppendNamedBattle = GameObj.BattleName + ":" + BattleLong.TotalSeconds.ToString("F2");
            }
        }
        private void BattleEndMore()
        {
        }
        private void FlushGameObject()
        {
            GameObj.Flush(PalHandle, PID, ExeBaseAddr, 0);
            FlushPlugins(PalHandle, PID, ExeBaseAddr, 0);
        }
        public override bool NeedBlockCtrlEnter()
        {
            return false;
        }
    }

    public class GameObjectGuJian2:MemoryReadBase
    {
        private static readonly int[] MoneyOffset = new int[] { 0x7080A8, 0x20, 0x4 };
        private const int CanControlOffset = 0x7080A8 + 0x544;
        private static readonly int[] GameTimeOffset = new int[] { 0x7080A8, 0x20, 0xE8 };
        private static readonly int[] InBattleOffset = new int[] { 0x700870, 0xAE8 };
        private static readonly int[] BattleResultOffset = new int[] {0x700870, 0xB18 };
        private static readonly int[] EnemyCountBeginOffset = new int[] { 0x700870, 0xC90 };
        private static readonly int[] EnemyCountCurrentOffset = new int[] { 0x700870, 0x16C };
        private static readonly int[] YWYLevelOffset = new int[] { 0x7080A8, 0x28, 0x8, 0x560, 0x8EC };
        private static readonly int[] UITypeOffset = new int[] { 0x707C5C, 0x10 };
        private const int EnemyCountCurrentOffset2 = 0x910;
        private static readonly int[][] EnemyIDOffset = new int[][] {
            new int[]{ 0x700870, 0x118, 0x0, 0x560},
            new int[]{ 0x700870, 0x118, 0x4, 0x560},
            new int[]{ 0x700870, 0x118, 0x8, 0x560},
            new int[]{ 0x700870, 0x118, 0xC, 0x560},
            new int[]{ 0x700870, 0x118, 0x10, 0x560},
        };
        private static readonly int[][] EnemyHPOffset = new int[][] {
            new int[]{ 0x700870, 0x118, 0x0, 0x6C4},
            new int[]{ 0x700870, 0x118, 0x4, 0x6C4},
            new int[]{ 0x700870, 0x118, 0x8, 0x6C4},
            new int[]{ 0x700870, 0x118, 0xC, 0x6C4},
            new int[]{ 0x700870, 0x118, 0x10, 0x6C4},
        };
        private static readonly int[][] EnemyMaxHPOffset = new int[][] {
            new int[]{ 0x700870, 0x118, 0x0, 0x6C4+40},
            new int[]{ 0x700870, 0x118, 0x4, 0x6C4+40},
            new int[]{ 0x700870, 0x118, 0x8, 0x6C4+40},
            new int[]{ 0x700870, 0x118, 0xC, 0x6C4+40},
            new int[]{ 0x700870, 0x118, 0x10, 0x6C4+40},
        };
        private static readonly int[] 七返灵砂Offset = new int[] { 0x6AC574, 0xF4, 0x4C, 0x18, 0xC };
        //private static readonly int[] 寒月冰履Offset = new int[] { };
        //private static readonly int[] 明月巾Offset = new int[] { };

        public bool CanControl = false;
        public bool IsInBattle = false;
        public bool IsLoading = false;
        public int BattleReslut = 0;//0胜利 1失败
        public int UIType = 0;//5战斗 6剧情 3读条 2主界面 4正常
        public int Money = 0;
        public float GameTime = 0.0F;
        private int EnemyCountBegin = 0;
        public int EnemyCount = 0;
        public List<GuJian2EnemyObject> Enemies = new List<GuJian2EnemyObject>();
        public enum EBattle
        {
            none,
            normal,
            太乙神兵,
            慧明,
            金砖,
            灵虚,
            偃甲将军,
            雩风,
            尸兽厌火,
            安尼瓦尔,
            流月城守卫头领,
            远古石兽,
            火龙伏英,
            怒霜天君,
            温留,
            蜃精玉怜,
            初七,
            程廷钧,
            魔化华月,
            沈夜变身,
            砺罂
        }
        public EBattle BattleID = EBattle.none;
        public int TotalEnemyHP = 0;
        public int TotalEnemyMaxHP = 0;
        public int YWYLevel = 0;
        public string BattleName = "";
        public int QFLSCount = 0;
        public bool IsWin()
        {
            if (IsInBattle) return false;
            return BattleReslut == 0;
        }

        private bool LastIsInBattle = false;
        public GameObjectGuJian2()
        {
        }
        public override string ToString()
        {
            string ret = "钱：" + Money + "\r\n"; ;
            ret += "读条中：" + (IsLoading ? "是" : "否") + "\r\n\r\n";
            ret += "可操作：" + (CanControl ? "可" : "不可") + "\r\n";
            ret += "战斗中：" + (IsInBattle ? "是" : "否") + " (" + BattleID.ToString() + ")[" + TotalEnemyHP + "/" + TotalEnemyMaxHP + "]" + "\r\n\r\n";
            ret += "敌人数量：" + EnemyCount + "\r\n";
            foreach (var eo in Enemies)
            {
                ret += "[" + eo.ID + "]" + eo.HP + "/" + eo.MaxHP + "\r\n";
            }

            return ret;
        }
        public override void Flush(IntPtr handle, int PID, int ExeBaseAddr,long b)
        {
            Money = Readm<int>(handle, ExeBaseAddr, MoneyOffset);
            GameTime = Readm<float>(handle, ExeBaseAddr, GameTimeOffset);
            BattleReslut = Readm<int>(handle, ExeBaseAddr, BattleResultOffset);
            IsInBattle = (Readm<int>(handle, ExeBaseAddr, InBattleOffset) != 0);
            CanControl = (Readm<int>(handle, ExeBaseAddr + CanControlOffset) == 1);
            YWYLevel = Readm<int>(handle, ExeBaseAddr, YWYLevelOffset);
            UIType = Readm<int>(handle, ExeBaseAddr, UITypeOffset);
            QFLSCount = Readm<int>(handle, ExeBaseAddr, 七返灵砂Offset);
            IsLoading = (UIType == 3);
            EnemyCountBegin = Readm<int>(handle, ExeBaseAddr, EnemyCountBeginOffset);
            int tmpec = 0;
            if (IsInBattle)
            {
                int ecaddr = Readm<int>(handle, ExeBaseAddr, EnemyCountCurrentOffset);
                if (ecaddr > 0)
                {
                    tmpec = Readm<int>(handle, ecaddr + EnemyCountCurrentOffset2);
                }
                else
                {
                    tmpec = EnemyCountBegin;
                }
            }
            else
            {
                tmpec = 0;
            }
            if (tmpec > 50) tmpec = 0;
            EnemyCount = tmpec;
            List<GuJian2EnemyObject> el = new List<GuJian2EnemyObject>();
            int tmphp = 0;
            int tmpmaxhp = 0;
            for (int i = 0; i < EnemyIDOffset.Length && i < EnemyCount; ++i)
            {
                GuJian2EnemyObject eo = new GuJian2EnemyObject();
                eo.ID = Readm<int>(handle, ExeBaseAddr, EnemyIDOffset[i]);
                eo.HP = Readm<int>(handle, ExeBaseAddr, EnemyHPOffset[i]);
                eo.MaxHP = Readm<int>(handle, ExeBaseAddr, EnemyMaxHPOffset[i]);
                if (eo.ID > 0 && eo.ID < 2000)
                {
                    el.Add(eo);
                    tmpmaxhp += eo.MaxHP;
                    if (eo.HP > 0 && eo.HP < 1000000)
                    {
                        tmphp += eo.HP;
                    }
                }
            }
            Enemies = el;
            TotalEnemyHP = tmphp;
            TotalEnemyMaxHP = tmpmaxhp;
            if (IsInBattle)
            {
                if (!LastIsInBattle)
                {
                    BattleID = EBattle.none;
                    BattleName = "";
                }
                if (BattleID == EBattle.none || BattleID == EBattle.normal)
                {
                    _anabattle();
                }
            }
            else
            {
                if (LastIsInBattle)
                {
                    ClearEnemyMemory(handle, ExeBaseAddr);
                }
            }
            LastIsInBattle = IsInBattle;
        }

        public void ClearEnemyMemory(IntPtr handle, int ExeBaseAddr)
        {
            for (int i = 0; i < EnemyIDOffset.Length; ++i)
            {
                int addr = ExeBaseAddr;
                int[] offset = EnemyIDOffset[i];
                for (var j = 0; j < offset.Length - 1; ++j)
                {
                    addr = Readm<int>(handle, addr + offset[j]);
                }
                addr += offset[offset.Length - 1];


                int ID = Readm<int>(handle, addr);
                if (ID > 0 && ID < 2000)
                {
                    byte[] writebuf = new byte[4] { 0x0, 0x0, 0x0, 0x0 };
                    int sizeofWrite;
                    Kernel32.WriteProcessMemory(handle, new IntPtr(addr), writebuf, 3, out sizeofWrite);
                }
            }
        }

        private void _anabattle() {
            if (Enemies.Count <= 0)
            {
                //BattleID = EBattle.none;
                return;
            }
            foreach (var eo in Enemies)
            {
                switch (eo.ID)
                {
                    case 702:
                        BattleID = EBattle.太乙神兵;
                        return;
                    case 703:
                        BattleID = EBattle.慧明;
                        return;
                    case 704:
                        BattleID = EBattle.金砖;
                        return;
                    case 706:
                    case 707:
                        BattleID = EBattle.灵虚;
                        return;
                    case 708:
                        BattleID = EBattle.偃甲将军;
                        return;
                    case 709:
                        BattleID = EBattle.雩风;
                        return;
                    case 711:
                        BattleID = EBattle.尸兽厌火;
                        return;
                    case 713:
                        BattleID = EBattle.安尼瓦尔;
                        return;
                    case 717:
                        BattleID = EBattle.流月城守卫头领;
                        return;
                    case 719:
                        BattleID = EBattle.远古石兽;
                        return;
                    case 721:
                        BattleID = EBattle.火龙伏英;
                        return;
                    case 722:
                        BattleID = EBattle.怒霜天君;
                        return;
                    case 723:
                        BattleID = EBattle.温留;
                        return;
                    case 724:
                        BattleID = EBattle.蜃精玉怜;
                        return;
                    case 726:
                        BattleID = EBattle.初七;
                        return;
                    case 727:
                    case 733:
                        BattleID = EBattle.程廷钧;
                        return;
                    case 729:
                        BattleID = EBattle.魔化华月;
                        return;
                    case 731:
                        BattleID = EBattle.沈夜变身;
                        return;
                    case 732:
                        BattleID = EBattle.砺罂;
                        return;
                }
                switch (eo.ID)
                {
                    case 700:
                        BattleName = "金刚力士";
                        break;
                    case 701:
                        BattleName = "金刚力士合体";
                        break;
                    case 712:
                        BattleName = "浑邪王";
                        break;
                    case 714:
                        BattleName = "明川";
                        break;
                    case 715:
                        BattleName = "华月";
                        break;
                    case 716:
                        BattleName = "沈夜";
                        break;
                    case 720:
                        BattleName = "风琊";
                        break;
                    case 725:
                        BattleName = "初七";
                        break;
                    case 728:
                        BattleName = "华月";
                        break;
                    case 730:
                        BattleName = "沈夜";
                        break;
                    default:
                        BattleName = "";
                        break;
                }
            }
            BattleID = EBattle.normal;
        }

        public class GuJian2EnemyObject
        {
            public int ID;
            public int HP;
            public int MaxHP;
        }
    }
}
