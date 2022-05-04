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
    public class 梦幻22 : TimerCore
    {
        private PTimer ST = new PTimer();
        private PTimer LT = new PTimer();
        private string GMD5 = "none";
        public IntPtr PalHandle;
        private int PID = -1;
        private Process PalProcess;
        private int PALBaseAddr = -1;
        private bool _HasGameStart = false;
        private bool _IsFirstStarted = false;
        private DateTime InBattleTime;
        private DateTime OutBattleTime;
        public TimeSpan BattleLong = new TimeSpan(0);
        private bool HasUnCheated = false;
        private bool IsInUnCheat = false;
        private bool IsPause = false;
        private bool IsInBattle = false;

        private bool IsDoMoreEndBattle = true;
        private string WillCopyRPG = "";

        private string cryerror = "";
        private short MaxFC = 0;
        private short MaxFM = 0;
        private short MaxHCG = 0;
        private short MaxXLL = 0;
        private short MaxLQJ = 0;
        private short MaxYXY = 0;

        private GameObjectDream22 GameObj = new GameObjectDream22();
        private List<string> NamedBattleRes = new List<string>();

        public 梦幻22(GForm form) : base(form)
        {
            CoreName = "DREAM22";
        }
        public override bool IsShowC()
        {
            return false;
        }
        public override bool NeedBlockCtrlEnter()
        {
            return false;
        }
        protected override void InitCheckPoints()
        {
            LoadBest();
            _CurrentStep = -1;
            Data = new HObj();
            CheckPoints = new List<CheckPoint>();
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("上船", new TimeSpan(0, 14, 30)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(5, 1076, 1082,5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出蛇洞", new TimeSpan(0, 29, 00)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(48, 304, 1560, 3))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过智修", new TimeSpan(0, 43, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult== 3 && GameObj.Area == 58 && GameObj.EXBattleExpGet >= 900 && GameObj.EXBattleGoldGet >= 4277)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过鬼将军", new TimeSpan(0, 58, 00)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && (GameObj.Area == 65 || GameObj.Area == 66) && GameObj.EXBattleExpGet >= 1980 && GameObj.EXBattleGoldGet >= 1253)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过赤鬼王", new TimeSpan(1, 12, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 59 && GameObj.EXBattleExpGet >= 2850 && GameObj.EXBattleGoldGet >= 1750)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("进扬州", new TimeSpan(1, 27, 00)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(82, 288, 1072, 5))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("出扬州", new TimeSpan(1, 41, 30)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck(105, 64, 960, 3))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过鬼母", new TimeSpan(1, 55, 00)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 103 && GameObj.EXBattleExpGet >= 4440 && GameObj.EXBattleGoldGet >= 3400)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过彩依", new TimeSpan(2, 09, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.Area == 108)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过剑老头", new TimeSpan(2, 24, 00)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 147 && GameObj.EXBattleExpGet >= 7500 && GameObj.EXBattleGoldGet >= 3000)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过明王", new TimeSpan(2, 38, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && (GameObj.Area == 145 || GameObj.Area==153))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("拆塔", new TimeSpan(2, 53, 00)))
            {
                Check = delegate ()
                {
                    if (GameObj.AreaBGM == 23)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过凤凰", new TimeSpan(3, 07, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 185 && GameObj.EXBattleExpGet >= 10580 && GameObj.EXBattleGoldGet >= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过木道人", new TimeSpan(3, 22, 00)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 181 && GameObj.EXBattleExpGet >= 9600 && GameObj.EXBattleGoldGet >= 8000)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过火麒麟", new TimeSpan(3, 36, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 200 && GameObj.EXBattleExpGet >= 14000 && GameObj.EXBattleGoldGet >= 2800)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过十年前", new TimeSpan(3, 51, 00)))
            {
                Check = delegate ()
                {
                    if (GameObj.GetItemCount(0x109) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过七毒", new TimeSpan(4, 05, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 224 && GameObj.EXBattleExpGet >= 7500 && GameObj.EXBattleGoldGet >= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过血角青龙", new TimeSpan(4, 20, 00)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 295 && GameObj.EXBattleExpGet >= 18000 && GameObj.EXBattleGoldGet >= 18000)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过五神龙", new TimeSpan(4, 34, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 213 && GameObj.EXBattleExpGet >= 0 && GameObj.EXBattleGoldGet >= 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("过桥头拜月", new TimeSpan(4, 49, 00)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 281 && GameObj.EXBattleExpGet >= 30000 && GameObj.EXBattleGoldGet >= 10800)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关", new TimeSpan(5, 03, 30)))
            {
                Check = delegate ()
                {
                    if (NewBattleEnd && GameObj.EXBattleResult == 3 && GameObj.Area == 297 && GameObj.EXBattleExpGet >= 30000 && GameObj.EXBattleGoldGet >= 30000)
                    {
                        return true;
                    }
                    return false;
                }
            });
            
        }

        private bool PositionCheck(params int[][] PositionList)
        {
            foreach (int[] p in PositionList)
            {
                if (GameObj.Area == p[0] && GameObj.X == p[1] && GameObj.Y == p[2])
                {
                    return true;
                }
            }
            return false;
        }
        private bool PositionAroundCheck(int Area, int X, int Y, int r = 1)
        {
            if (GameObj.Area == Area)
            {
                if (GameObj.X >= (X - 16 * r) && GameObj.X <= (X + 16 * r)
                    && GameObj.Y >= (Y - 8 * r) && GameObj.Y <= (Y + 8 * r))
                {
                    return true;
                }
            }
            return false;
        }

        public override string GetMoreInfo()
        {
            return "欢迎使用仙剑梦幻2.2自动计时器";
        }

        public override string GetSmallWatch()
        {
            return BattleLong.TotalSeconds.ToString("F2") + "s";
            //return MoveSpeed.ToString("F1");
        }
        
        public override string GetSecondWatch()
        {
            if (ST.CurrentTSOnly.Ticks == 0)
            {
                return "";
            }
            return ST.ToString();
        }

        public override TimeSpan GetMainWatch()
        {
            return MT.CurrentTS;
        }

        public override bool IsMainWatchStar()
        {
            return IsInUnCheat;
        }

        public override string GetGameVersion()
        {
            if (PID != -1)
            {
                return "仙剑梦幻2.2";
            }
            else
            {
                return "等待游戏运行";
            }
        }

        public override void Reset()
        {
            base.Reset();
            HasAlertMutiPal = false;
            HasUnCheated = false;
            IsInUnCheat = false;
            ST.Stop();
            _IsFirstStarted = false;
            MaxFC = 0;
            MaxFM = 0;
            MaxHCG = 0;
            MaxLQJ = 0;
            MaxXLL = 0;
            MaxYXY = 0;
            BattleLong = new TimeSpan(0);
            //InitCheckPoints();
            ST.Reset();
            WillAppendNamedBattle = "";
            NamedBattleRes = new List<string>();
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

        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            LoadCore(new 简版(form));
        }
        private bool EXLastBattleFlag = false;
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
                    if (GameObj.EXBattleFlag)
                    {
                        if (!EXLastBattleFlag)
                        {
                            BattleBegin();
                        }
                        EXLastBattleFlag = true;
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
                        if (EXLastBattleFlag)
                        {
                            BattleEnd();
                            IsDoMoreEndBattle = false;
                        }
                        EXLastBattleFlag = false;
                    }
                    /*if (GameObj.Enemies.Count > 0)
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
                    }*/
                }
                catch { }

                if (HasStartGame())
                {
                    ST.Stop();
                    if (!_IsFirstStarted)
                    {
                        _IsFirstStarted = true;
                    }
                    if (!HasUnCheated)
                    {
                        if (!IsInUnCheat)
                        {
                            CheckCheatBegin();
                            CheckCheatEnd();
                        }
                        else
                        {
                            CheckCheatEnd();
                        }
                    }

                    if (IsInUnCheat)
                    {
                        MT.Stop();
                    }
                    else
                    {
                        MT.Start();
                        Checking();
                        NewBattleEnd = false;
                    }
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
        private bool HasAlertMutiPal = false;
        private bool GetPalHandle()
        {
            Process[] res = Process.GetProcessesByName("sdlpal");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个sdlpal.exe进程，请关闭其他的，只保留一个！";
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
                    PID = PalProcess.Id;
                    PalHandle = new IntPtr(Kernel32.OpenProcess(0x1F0FFF, false, PID));
                    PALBaseAddr = PalProcess.MainModule.BaseAddress.ToInt32();
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
                PalProcess = null;
                PID = -1;
                GMD5 = "none";
                PALBaseAddr = -1;
                return false;
            }
        }

        private void CalcPalMD5()
        {
            try
            {
                string dllmd5 = GetFileMD5(GetGameFilePath("SDL.dll"));
                GMD5 = dllmd5;
            }
            catch
            {
                GMD5 = "none";
            }
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

        private void JudgePause()
        {
            if (IsUIPause)
            {
                IsPause = true;
                return;
            }
            IntPtr hWnd = User32.GetForegroundWindow();    //获取活动窗口句柄  
            int calcID = 0;    //进程ID  
            int calcTD = 0;    //线程ID  
            calcTD = User32.GetWindowThreadProcessId(hWnd, out calcID);
            if (calcID == PID)
            {
                IsPause = false;
            }
            else
            {
                IsPause = true;
            }
        }

        private void FlushGameObject()
        {
            GameObj.Flush(PalHandle, PID, PALBaseAddr,0);
            FlushPlugins(PalHandle, PID, PALBaseAddr, 0);
        }
        private Dream22BattleItemWatch biw = new Dream22BattleItemWatch();
        private string CurrentNamedBattle = "";
        private string WillAppendNamedBattle = "";
        private void BattleBegin()
        {
            NewBattleEnd = false;
            BattleLong = new TimeSpan(0);
            InBattleTime = DateTime.Now;
            biw = new Dream22BattleItemWatch();
            if (CurrentStep <= 5)
            {
                //战斗前记录下个数
                biw.Insert(0x73, GameObj.GetItemCount(0x73));//蜂
                biw.Insert(0x83, GameObj.GetItemCount(0x83));//蜜
                biw.Insert(0x8F, GameObj.GetItemCount(0x8F));//火
            }
            biw.Insert(0xB8, GameObj.GetItemCount(0xB8));//龙泉剑
            biw.Insert(0xA2, GameObj.GetItemCount(0xA2));//血玲珑
            biw.Insert(0xD4, GameObj.GetItemCount(0xD4));//夜行衣
            CurrentNamedBattle = GameObj.GetNamedBattle();
        }
        private void Battling()
        {
            BattleLong = DateTime.Now - InBattleTime;
            /*if (CurrentStep <= 5)
            {
                //战斗中每隔100毫秒算下差
                biw.SetCount(GameObj);
            }*/
            biw.SetCount(GameObj);
        }
        private bool NewBattleEnd = false;
        private void BattleEnd()
        {
            NewBattleEnd = true;
            OutBattleTime = DateTime.Now;
            BattleLong = OutBattleTime - InBattleTime;
            /*if (CurrentStep <= 5)
            {
                //战斗结束，强制再算差
                biw.SetCount(GameObj);
            }*/
            biw.SetCount(GameObj);

            if (CurrentNamedBattle != "")
            {
                WillAppendNamedBattle = CurrentNamedBattle + "：" + BattleLong.TotalSeconds.ToString("F2") + "s";
                CurrentNamedBattle = "";
            }
        }
        private void BattleEndMore()
        {
            //战斗结束，强制再算差
            biw.SetCount(GameObj);
            if (CurrentStep <= 5)
            {
                //将算出来的差加入显示
                MaxFC += biw.GettedCount(0x73);
                MaxFM += biw.GettedCount(0x83);
                MaxHCG += biw.GettedCount(0x8F);
            }
            MaxLQJ += biw.GettedCount(0xB8);
            MaxXLL += biw.GettedCount(0xA2);
            MaxYXY += biw.GettedCount(0xD4);
        }
        private bool HasStartGame()
        {
            if (!_HasGameStart)
            {
                if (GameObj.Area != 0)
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
        protected override void FillMoreTimerData(HObj exdata)
        {
            exdata["Idle"] = ST.ToString();
            exdata["Lite"] = LT.ToString();
            exdata["BeeHouse"] = MaxFC;
            exdata["BeeSheet"] = MaxFM;
            exdata["FireWorm"] = MaxHCG;
            exdata["DragonSword"] = MaxLQJ;
            exdata["BloodLink"] = MaxXLL;
            exdata["NightCloth"] = MaxYXY;
            exdata["GMD5"] = GMD5;

            string namedbattles = "";
            foreach (string nmb in NamedBattleRes)
            {
                namedbattles += nmb + "|";
            }
            if (namedbattles != "")
            {
                namedbattles = namedbattles.Substring(0, namedbattles.Length - 1);
            }
            exdata["NamedBattles"] = namedbattles;
        }
        private void CheckCheatBegin()
        {
            if (PositionCheck(new int[3] { 177, 1296, 728 }, new int[3] { 177, 1264, 728 }, new int[3] { 177, 1296, 712 }, new int[3] { 177, 1232, 728 }))
            {
                IsInUnCheat = true;
            }
        }
        private void CheckCheatEnd()
        {
            if (GameObj.GetItemCount(0x123) > 0)
            {
                IsInUnCheat = false;
                HasUnCheated = true;
            }
        }
        public override void OnFunctionKey(int FunNo)
        {
            switch (FunNo)
            {
                case 8:
                    HandPause();
                    break;
                case 6:
                    if (form.Confirm("更换内核将会重置计时器，确认么？"))
                    {
                        BtnSwitch_Click(null, null);
                    };
                    break;
                case 12:
                    //DebugForm df = new DebugForm();
                    //df.ShowData(GameObj/*, BattleLong*/);
                    //df.Show();
                    break;
            }
        }

        public delegate void OnExSuccess();

        private void UI_SaveGameEx(FormEx f, OnExSuccess cb, string fn = "SRPG.bin")
        {
            SetUIPause(true);
            InfoShow isw = null;
            isw = new InfoShow(f, delegate ()
            {
                SuspendSaveListen();
                SetUIPause(false);
                isw.Dispose();
            });
            isw.lblInfo.Text = "计时器已暂停，请在游戏中存档";
            bool haserr = false;
            try
            {
                SaveGameEx(f, delegate (bool isok, string errstr)
                {
                    if (isok)
                    {
                        if (cb != null)
                        {
                            cb();
                        }
                    }
                    else
                    {
                        if (errstr != "")
                        {
                            f.Error(errstr);
                        }
                        else
                        {
                            f.Alert("操作中断");
                        }
                    }
                    SetUIPause(false);
                    isw.Dispose();
                }, fn);
            }
            catch (Exception ex)
            {
                haserr = true;
                f.Error(ex.Message);
            }
            if (haserr)
            {
                if (isw != null)
                {
                    isw.Dispose();
                }
                SetUIPause(false);
            }
            else
            {
                isw.ShowDialog(f);
            }
        }

        public delegate void ExEnd(bool IsOK, string ErrStr);

        private bool IsListenSave = false;

        private void SuspendSaveListen()
        {
            IsListenSave = false;
        }

        private string GetPalFolder()
        {
            string palpath = PalProcess.MainModule.FileName;
            string[] spli = palpath.Split('\\');
            spli[spli.Length - 1] = "";
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

        private void SaveGameEx(FormEx f, ExEnd cb, string fn = "SRPG.bin")
        {
            if (!GetPalHandle()) throw new Exception("游戏没有在运行，无法保存");
            IsListenSave = true;
            FormEx.Run(delegate ()
            {
                Dictionary<int, DateTime> RPGs = new Dictionary<int, DateTime>();
                string palfolder = GetPalFolder() + "SAVE\\";
                for (int i = 1; i <= 5; ++i)
                {
                    string p = palfolder + i + ".RPG";
                    if (File.Exists(p))
                    {
                        FileInfo fi = new FileInfo(p);
                        RPGs.Add(i, fi.LastWriteTime);
                    }
                }
                Thread.Sleep(300);
                string ChangedFile = "";
                while (IsListenSave && ChangedFile == "")
                {
                    for (int i = 1; i <= 5; ++i)
                    {
                        string p = palfolder + i + ".RPG";
                        if (File.Exists(p))
                        {
                            if (RPGs.ContainsKey(i))
                            {
                                FileInfo fi = new FileInfo(p);
                                if (fi.LastWriteTime > RPGs[i])
                                {
                                    ChangedFile = p;
                                    break;
                                }
                            }
                            else
                            {
                                ChangedFile = p;
                                break;
                            }
                        }
                    }
                    Thread.Sleep(300);
                }

                if (!IsListenSave)
                {
                    if (cb != null)
                    {
                        f.UI(delegate ()
                        {
                            cb(false, "");
                        });
                    }
                    return;
                }

                SRPGobj so = new SRPGobj();
                //so.RPG = SaveObject.GetSaveBuffer(this.PalHandle);
                so.TimerStr = GetRStr();
                try
                {
                    using (FileStream rfs = new FileStream(ChangedFile, FileMode.Open))
                    {
                        long size = rfs.Length;
                        so.RPG = new byte[size];
                        rfs.Read(so.RPG, 0, so.RPG.Length);
                    }
                }
                catch (Exception ex)
                {
                    cb(false, ex.Message);
                }

                string FilePath = fn;
                try
                {
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                    using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, so);
                    }
                }
                catch (Exception ex)
                {
                    if (cb != null)
                    {
                        f.UI(delegate ()
                        {
                            cb(false, ex.Message);
                        });
                    }
                }
                if (cb != null)
                {
                    f.UI(delegate ()
                    {
                        cb(true, "");
                    });
                }
            });
        }
        
        public override string GetAAction()
        {
            if (WillAppendNamedBattle == "")
            {
                return "";
            }
            else
            {
                NamedBattleRes.Add(WillAppendNamedBattle);
                string res = WillAppendNamedBattle;
                WillAppendNamedBattle = "";
                return res;
            }
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
    }


    public class GameObjectDream22:MemoryReadBase
    {
        public const int BaseAddrPTR = 0x0044129C;
        public const int MoneyOffset = 0x4468;
        public const int XOffset = 0x43ec;
        public const int YOffset = 0x43ee;
        public const int AreaOffset = 0x444a;
        public const int CurrentBGMOffset = 0x4454;
        public const int AreaBGMOffset = 0x4454;
        public const int ItemSlotOffsetPTR = 0x472c;

        public const int BattleEnemySlotOffsetPTR = 0x54da2;
        public const int BattleFlagOffset= 0x54cd0;

        public const int EXBattleFlagOffset = 0x5518C;
        public const int EXBattleResultOffset = 0x551B4;
        public const int EXBattleExpGetOffset = 0x551A4;
        public const int EXBattleGoldGetOffset = 0x551A8;

        public short[] BossIDs = new short[] { 533,519,464,494,474,500,472,463,546,575,576,524,473,468 };
        //533:七毒最后 519:明王 464:凤凰 494:剑老头 474:木道人 500:鬼母 472:鬼将军 463:火麒麟 546:桥头拜月 575:拜月+水魔兽 576:拜魔兽 524:智修 473:赤鬼王 468:彩依

        public int Money = 0;
        public short X = 0;
        public short Y = 0;
        public short Area = 0;
        public short CurrentBGM = 0;//0x3胜利 0x1失败 0x4葫芦界面
        public short AreaBGM = 0;
        public bool IsBattleFlag = false;

        public bool EXBattleFlag = false;
        public int EXBattleResult = 0;//1000以上战斗中，1战败，3战胜 0敌人主动脱战 65535逃脱
        public int EXBattleExpGet = 0;
        public int EXBattleGoldGet = 0;

        private IntPtr handle;
        private int PID;
        public int BaseAddr = 0x0;
        public int BattleEnemySlotAddr = 0x0;
        public int ItemSlotAddr = 0x0;

        public Dictionary<short, short> Items = new Dictionary<short, short>();
        public List<EnemyObjectDream22> Enemies = new List<EnemyObjectDream22>();
        public short BossID = -1;
        public int BattleTotalBlood = 0;

        public List<NamedBattle> NamedBattles = new List<NamedBattle>();

        public override string ToString()
        {
            string tmp = "";
            tmp += "EX战斗中：" + this.EXBattleFlag + "\r\n";
            tmp += "EX战斗结果：" + this.EXBattleResult + "\r\n";
            tmp += "EX (exp:" + this.EXBattleExpGet + ")(gold:" + this.EXBattleGoldGet + ")\r\n";
            tmp += "area:" + this.Area + "\r\n";
            tmp += "x:" + this.X + "\r\n";
            tmp += "y:" + this.Y + "\r\n";
            tmp += "money:" + this.Money + "\r\n";
            tmp += "BGM:" + this.CurrentBGM + "\r\n";
            tmp += "AreaBGM:" + this.AreaBGM + "\r\n";
            tmp += "\r\n";
            tmp += "是否有破天锤:" + ((this.GetItemCount(0x117) > 0) ? "是" : "否") + "\r\n";
            tmp += "是否有香蕉:" + ((this.GetItemCount(0x123) > 0) ? "是" : "否") + "\r\n";
            tmp += "\r\n";
            tmp += "IsBattle:" + this.IsBattleFlag + "\r\n";
            tmp += "Enemy(" + this.Enemies.Count + "):" + this.BattleTotalBlood + "\r\n";
            if (this.BossID > 0)
            {
                tmp += "Boss:" + this.BossID + "\r\n";
            }
            for (int i = 0; i < this.Enemies.Count; ++i)
            {
                tmp += "[" + (i + 1) + "] id:" + this.Enemies[i].ID + " blood:" + this.Enemies[i].Blood + "\r\n";
            }
            return tmp;
        }

        public GameObjectDream22()
        {
            this.InitNamedBattles();
        }

        public void InitNamedBattles()
        {
            NamedBattles.Add(new NamedBattle()
            {
                Name = "七毒",
                Checker = delegate ()
                {
                    if (GetEnemyCount(533) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "镇狱明王",
                Checker = delegate ()
                {
                    if (GetEnemyCount(519) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "凤凰",
                Checker = delegate ()
                {
                    if (GetEnemyCount(464) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "剑老头",
                Checker = delegate ()
                {
                    if (GetEnemyCount(494) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "木道人",
                Checker = delegate ()
                {
                    if (GetEnemyCount(474) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "鬼母",
                Checker = delegate ()
                {
                    if (GetEnemyCount(500) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "鬼将军",
                Checker = delegate ()
                {
                    if (GetEnemyCount(472) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "火麒麟",
                Checker = delegate ()
                {
                    if (GetEnemyCount(463) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "桥头拜月",
                Checker = delegate ()
                {
                    if (GetEnemyCount(546) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "拜月+水魔兽",
                Checker = delegate ()
                {
                    if (GetEnemyCount(575) == 1 && GetEnemyCount(574) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "拜魔兽",
                Checker = delegate ()
                {
                    if (GetEnemyCount(576) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "智修",
                Checker = delegate ()
                {
                    if (GetEnemyCount(524) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "赤鬼王",
                Checker = delegate ()
                {
                    if (GetEnemyCount(473) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "彩依",
                Checker = delegate ()
                {
                    if (GetEnemyCount(468) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "五神龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(541) == 1 && GetEnemyCount(542) == 1 && GetEnemyCount(543) == 1 && GetEnemyCount(544) == 1 && GetEnemyCount(545) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
            NamedBattles.Add(new NamedBattle()
            {
                Name = "血角青龙",
                Checker = delegate ()
                {
                    if (GetEnemyCount(572) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            });
        }

        public int GetEnemyCount(short EID)
        {
            short res = 0;
            if (this.Enemies != null)
            {
                foreach (EnemyObjectDream22 eo in this.Enemies)
                {
                    if (eo.ID == EID && eo.Blood>0)
                    {
                        res++;
                    }
                }
            }
            return res;
        }

        public string GetNamedBattle()
        {
            foreach (NamedBattle nb in this.NamedBattles)
            {
                if (nb.Checker())
                {
                    return nb.Name;
                }
            }
            return "";
        }

        public override void Flush(IntPtr handle, int PID,int SDLBaseAddr,long b)
        {
            if (PID != this.PID)
            {
                this.PID = PID;
                this.handle = handle;
            }
            BaseAddr = Readm<int>(this.handle, BaseAddrPTR);
            //BattleEnemySlotAddr = Readm<int>(this.handle, SDLBaseAddr + BattleEnemySlotOffsetPTR);
            BattleEnemySlotAddr = SDLBaseAddr + BattleEnemySlotOffsetPTR;
            IsBattleFlag = Readm<short>(this.handle, SDLBaseAddr + BattleFlagOffset) != 0;
            ItemSlotAddr = BaseAddr + ItemSlotOffsetPTR;
            Money = Readm<int>(this.handle, BaseAddr + MoneyOffset);
            X = Readm<short>(this.handle, BaseAddr + XOffset);
            Y = Readm<short>(this.handle, BaseAddr + YOffset);
            Area = Readm<short>(this.handle, BaseAddr + AreaOffset);
            CurrentBGM = Readm<short>(this.handle, BaseAddr + CurrentBGMOffset);
            AreaBGM = Readm<short>(this.handle, BaseAddr + AreaBGMOffset);

            EXBattleFlag = Readm<int>(this.handle, SDLBaseAddr + EXBattleFlagOffset) != 0;
            EXBattleResult = Readm<int>(this.handle, SDLBaseAddr + EXBattleResultOffset);
            EXBattleExpGet = Readm<int>(this.handle, SDLBaseAddr + EXBattleExpGetOffset);
            EXBattleGoldGet = Readm<int>(this.handle, SDLBaseAddr + EXBattleGoldGetOffset);

            FlushItems();
            FlushBattleEnemies();
        }

        private void FlushItems()
        {
            Dictionary<short, short> tmp = new Dictionary<short, short>();
            int currentaddr = ItemSlotAddr;
            for (int i = 0; i < 251; ++i, currentaddr += 0x6)
            {
                short id = Readm<short>(this.handle, currentaddr);
                if (id <= 0) continue;

                short count = Readm<short>(this.handle, currentaddr + 0x2);
                if (count <= 0) continue;

                if (tmp.ContainsKey(id))
                {
                    tmp[id] = count;
                }
                else
                {
                    tmp.Add(id, count);
                }


            }
            Items = tmp;
        }

        private void FlushBattleEnemies()
        {
            BossID = -1;
            BattleTotalBlood = 0;
            List<EnemyObjectDream22> tmp = new List<EnemyObjectDream22>();
            if (this.IsBattleFlag)
            {
                int currentaddr = BattleEnemySlotAddr;
                for (int i = 0; i < 5; ++i, currentaddr += EnemyObjectDream22.Length)
                {
                    if (Readm<short>(handle, currentaddr) <= 0)
                    {
                        continue;
                    }
                    EnemyObjectDream22 c = new EnemyObjectDream22(handle, currentaddr);
                    if (c.ID <= 0) continue;
                    if (c.Blood > 0)
                    {
                        BattleTotalBlood += c.Blood;
                    }
                    else
                    {
                        c.Blood = 0;
                    }
                    if (ArrayContains<short>(BossIDs, c.ID))
                    {
                        BossID = c.ID;
                    }
                    tmp.Add(c);
                }
            }
            Enemies = tmp;
        }

        public short GetItemCount(short ItemID)
        {
            if (Items.ContainsKey(ItemID))
            {
                return Items[ItemID];
            }
            else
            {
                return 0;
            }
        }
    }

    public class EnemyObjectDream22
    {
        public const int Length = 0xCC;

        private const int IDOffset = 0x0-22;
        private const int BloodOffset = 0x2;

        public short ID;
        public short Blood;

        public EnemyObjectDream22(IntPtr handle, int HeadAddr)
        {
            ID = MemoryReadBase.Readm<short>(handle, HeadAddr + IDOffset);
            Blood = MemoryReadBase.Readm<short>(handle, HeadAddr + BloodOffset);
        }
    }

    public class Dream22BattleItemWatch
    {
        private List<short> ids = new List<short>();
        private Dictionary<short, short> BattleGetItemWatch = new Dictionary<short, short>();
        private Dictionary<short, short> BattleUseItemWatch = new Dictionary<short, short>();
        private Dictionary<short, short> BattleLastItemCount = new Dictionary<short, short>();
        public Dream22BattleItemWatch()
        {
            ids = new List<short>();
            BattleGetItemWatch = new Dictionary<short, short>();
            BattleUseItemWatch = new Dictionary<short, short>();
            BattleLastItemCount = new Dictionary<short, short>();
        }
        public void Insert(short id, short Count)
        {
            ids.Add(id);
            BattleGetItemWatch.Add(id, 0);
            BattleUseItemWatch.Add(id, 0);
            BattleLastItemCount.Add(id, Count);
        }
        public void SetCount(short id, short Count)
        {
            short cha = (short)(Count - BattleLastItemCount[id]);
            BattleLastItemCount[id] = Count;
            if (cha > 0)
            {
                BattleGetItemWatch[id] = (short)(BattleGetItemWatch[id] + cha);
            }
            else if (cha < 0)
            {
                BattleUseItemWatch[id] = (short)(BattleUseItemWatch[id] - cha);
            }
        }
        public void SetCount(GameObjectDream22 GameObj)
        {
            foreach (short id in ids)
            {
                SetCount(id, GameObj.GetItemCount(id));
            }
        }
        public short UsedCount(short id)
        {
            return BattleUseItemWatch[id];
        }
        public short GettedCount(short id)
        {
            return BattleGetItemWatch[id];
        }
    }
}
