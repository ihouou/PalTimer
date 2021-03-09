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
    public class 仙剑三 : TimerCore
    {
        private Pal3Object GameObj = new Pal3Object();
        private bool IsAllRun = true;
        private string GMD5 = "none";
        public IntPtr PalHandle;
        public IntPtr GameWindowHandle = IntPtr.Zero;
        private int PID = -1;
        private Process PalProcess;
        private int CheckInterval = 70;
        private bool _HasGameStart = false;
        private bool _IsFirstStarted = false;

        private PTimer MT = new PTimer();
        private PTimer ST = new PTimer();
        private PTimer LT = new PTimer();
        private DateTime InBattleTime;
        private DateTime OutBattleTime;
        public TimeSpan BattleLong = new TimeSpan(0);
        private int HandPauseCount = 0;
        private bool IsPause = false;
        private bool IsUIPause = false;

        private bool IsInBattle = false;
        private bool IsDoMoreEndBattle = true;

        private string SelectSword = "";
        private string EndName = "";

        private string cryerror = "";

        private TimeSpan WillClear;
        private TimeSpan BestClear;

        public 仙剑三() : base()
        {
            try
            {
                BestFile = "best3.txt";
                this.OnCurrentStepChangedInner = delegate (int cstep)
                {
                    try
                    {
                        if (cstep > 0)
                        {
                            //CheckPoints[cstep].Current = MT.CurrentTSOnly;
                            long cha = CheckPoints[cstep - 1].GetCHA() * 1000 * 10000;
                            if ((BestClear.Ticks + cha) <= 0)
                            {
                                WillClear = BestClear.Add(new TimeSpan(0));
                            }
                            else
                            {
                                WillClear = BestClear.Add(new TimeSpan(cha));
                            }
                        }
                    }
                    catch
                    { }
                };
            }
            catch
            { }
        }

        public override void InitCheckPoints()
        {
            LoadBest();
            _CurrentStep = -1;
            Data = new HObj();
            Data["j0"] = false;
            Data["end2"] = false;
            Data["end2bat"] = false;
            Data["boss737"] = false;
            SelectSword = "";
            EndName = "";
            CheckPoints = new List<CheckPoint>();
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("仗剑璧山", new TimeSpan(0, 9, 35)))
            {
                Check = delegate ()
                {
                    if (GameObj.JTWeapon== 6601)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("雷鸣霹雳", new TimeSpan(0, 20, 21)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("M02", "1", "1", 1491, -253, 2214, 50))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("船溯长江", new TimeSpan(0, 46, 35)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("M04", "1", "1", 3, -25, -394, 50))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("蓬莱惊变", new TimeSpan(0, 55, 50)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("q04", "q04", "q04", 647, 1, -25, 100))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("剑灵小葵", new TimeSpan(1, 6, 11)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("q01", "bn06", "bn06a", 172, 20, -11, 50))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("再见紫萱", new TimeSpan(1, 25, 38)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("M09", "3", "3", 65, 0, -213, 100))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("土灵珠现", new TimeSpan(1, 46, 45)))
            {
                Check = delegate ()
                {
                    if (GameObj.ItemCount(6123) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("沧桑叹", new TimeSpan(2, 14, 49)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("q05", "q05", "q05", -3234, 1, 1378, 100))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("雷灵出壳", new TimeSpan(2, 40, 24)))
            {
                Check = delegate ()
                {
                    if (GameObj.ItemCount(6120) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("风灵记忆", new TimeSpan(2, 58, 13)))
            {
                Check = delegate ()
                {
                    if (GameObj.ItemCount(6119) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("雪见献身", new TimeSpan(3, 23, 20)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("q08", "qn10", "qn10", -1686, -67, 364, 100))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("回魂仙梦", new TimeSpan(3, 46, 35)))
            {
                Check = delegate ()
                {
                    if (PositionAroundCheck("q08", "q08p", "q08p", 1293, 113, 1290, 100))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("火灵地狱", new TimeSpan(4, 6, 13)))
            {
                Check = delegate ()
                {
                    if (GameObj.ItemCount(6122) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("三魂归体", new TimeSpan(4, 20, 21)))
            {
                Check = delegate ()
                {
                    /*if (GameObj.BossID == 721 && GameObj.BattleTotalBlood <= 0)
                    {
                        return true;
                    }*/

                    if (PositionAroundCheck("m22", "5", "5", 141, 31, 5, 100))
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("圣灵奇缘", new TimeSpan(4, 42, 55)))
            {
                Check = delegate ()
                {
                    if (GameObj.ItemCount(6124) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("抉择之剑", new TimeSpan(5, 0, 33)))
            {
                Check = delegate ()
                {
                    if (GameObj.JTWeapon == 0)
                    {
                        Data["j0"] = true;
                    }
                    if (Data.GetValue<bool>("j0"))
                    {
                        if (GameObj.JTWeapon == 6602)
                        {
                            SelectSword = "镇妖剑";
                            return true;
                        }
                        if (GameObj.JTWeapon == 6603)
                        {
                            SelectSword = "魔剑";
                            return true;
                        }
                    }
                    return false;
                }
            });
            CheckPoints.Add(new CheckPoint(CheckPoints.Count, GetBest("通关", new TimeSpan(5, 8, 29)))
            {
                Check = delegate ()
                {
                    if (!Data.GetValue<bool>("boss737"))
                    {
                        if (GameObj.BossID == 737 && GameObj.BattleFlag==1)
                        {
                            Data["boss737"] = true;
                        }
                    }
                    else
                    {
                        if (GameObj.BattleFlag==2)
                        {
                            if (GameObj.XJfv < GameObj.ZXfv)
                            {
                                EndName = " - 结局1";
                                return true;
                            }
                            else
                            {
                                EndName = " - 结局2";
                                Data["end2"] = true;
                                Data["end2bat"] = false;
                            }
                            Data["boss737"] = false;
                        }
                    }
                    if (Data.GetValue<bool>("end2"))
                    {
                        if (Data.GetValue<bool>("end2bat"))
                        {
                            if (!GameObj.IsInBattle)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (GameObj.BossID == 738)
                            {
                                Data["end2bat"] = true;
                            }
                        }
                    }
                    return false;
                }
            });

            WillClear = GetBest("通关", new TimeSpan(5, 8, 29)).BestTS;
            BestClear = GetBest("通关", new TimeSpan(5, 8, 29)).BestTS;
        }

        private bool PositionAroundCheck(string Map, string SubMap, string SmallMap, float FrontBack, float UpDown, float LeftRight, float Distance = 5)
        {
            if (GameObj.Map == Map && GameObj.SubMap == SubMap && GameObj.SmallMap == SmallMap)
            {
                if ((FrontBack - Distance) <= GameObj.FrontBack && GameObj.FrontBack <= (FrontBack + Distance))
                {
                    if ((UpDown - Distance) <= GameObj.UpDown && GameObj.UpDown <= (UpDown + Distance))
                    {
                        if ((LeftRight - Distance) <= GameObj.LeftRight && GameObj.LeftRight <= (LeftRight + Distance))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private Button btnPause;
        public override void InitUI(NewForm form)
        {
            btnPause = form.NewMenuButton(0);
            btnPause.Text = "暂停";
            btnPause.Click += BtnPause_Click;

            var btnExportCurrent = form.NewMenuItem();
            btnExportCurrent.Text = "导出本次成绩";
            btnExportCurrent.Click += delegate (object sender, EventArgs e) {
                DateTime now = DateTime.Now;
                string filename = now.ToString("pal3_yyyyMMddHHmmss");
                string ext = GetRStr();

                try
                {
                    using (FileStream fileStream = new FileStream(filename + ".txt", FileMode.Create))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                        {
                            streamWriter.Write(ext);
                            //streamWriter.Flush();
                        }
                    }
                    form.Success("已将此次成绩保存至" + filename + ".txt");
                }
                catch (Exception ex)
                {
                    form.Error("保存失败：" + ex.Message);
                }
            };

            var btnSetCurrentToBest = form.NewMenuItem();
            btnSetCurrentToBest.Text = "设置本次成绩为最佳";
            btnSetCurrentToBest.Click += delegate (object sender, EventArgs e) {
                DateTime now = DateTime.Now;
                string filename = now.ToString("pal3_yyyyMMddHHmmss");
                string ext = GetRStr();
                try
                {
                    if (File.Exists("best3.txt"))
                    {
                        File.Move("best3.txt", "best" + filename + ".txt");
                    }
                    using (FileStream fileStream = new FileStream("best3.txt", FileMode.Create))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                        {
                            streamWriter.Write(ext);
                            //streamWriter.Flush();
                        }
                    }
                    if (form.Confirm("保存成功，确定要重置计时器么？"))
                    {
                        form._ResetAll();
                    }
                }
                catch (Exception ex)
                {
                    form.Error("保存失败：" + ex.Message);
                }
            };
        }
        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (!IsUIPause)
            {
                HandPauseCount++;
            }
            SetUIPause(!IsUIPause);
            if (HandPauseCount > 0)
            {
                btnPause.Text = "暂停 " + HandPauseCount;
            }
        }
        private void SetUIPause(bool isp)
        {
            IsUIPause = isp;
            if (IsUIPause)
            {
                btnPause.ForeColor = Color.Red;
            }
            else
            {
                btnPause.ForeColor = Color.White;
            }
        }
        public string GetRStr()
        {
            HObj exdata = new HObj();
            exdata["Current"] = MT.ToString();
            exdata["Idle"] = ST.ToString();
            exdata["Lite"] = LT.ToString();
            exdata["Step"] = CurrentStep;
            exdata["OSTime"] = DateTime.Now.Ticks.ToString();
            exdata["GMD5"] = GMD5;
            HObj cps = new HObj();
            foreach (CheckPoint c in CheckPoints)
            {
                HObj cur = new HObj();
                cur["name"] = c.Name;
                cur["des"] = c.NickName;
                cur["time"] = TItem.TimeSpanToString(c.Current);
                cps.Add(cur);
            }
            exdata["CheckPoints"] = cps;
            
            return exdata.ToJson();
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
                string dllmd5 = GetFileMD5(GetGameFilePath("pal3.dll"));
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
        private bool GetPalHandle()
        {
            Process[] res = Process.GetProcessesByName("Pal3");
            if (res.Length > 1)
            {
                if (!HasAlertMutiPal)
                {
                    cryerror = "检测到多个Pal3.exe进程，请关闭其他的，只保留一个！";
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
                return false;
            }
        }
        private bool HasStartGame()
        {
            if (!_HasGameStart)
            {
                if (GameObj.Map!="")
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
        private void Checking()
        {
            if (CurrentStep < 0 && CheckPoints.Count > 0)
            {
                CheckPoints[0].IsBegin = true;
                CurrentStep = 0;
            }

            if (CurrentStep < CheckPoints.Count)
            {
                CheckPoints[CurrentStep].Current = MT.CurrentTSOnly;
                if (CheckPoints[CurrentStep].Check())
                {
                    CheckPoints[CurrentStep].Current = new TimeSpan(MT.CurrentTSOnly.Ticks);
                    CheckPoints[CurrentStep].IsEnd = true;
                    //CurrentStep++;
                    int nextstep = CurrentStep + 1;
                    if (nextstep >= CheckPoints.Count)
                    {
                        OnLastCheckPointEnd();
                    }
                    else
                    {
                        CheckPoints[nextstep].IsBegin = true;
                    }
                    CurrentStep = nextstep;
                }
            }
            else
            {
                OnLastCheckPointEnd();
            }
        }
        private void OnLastCheckPointEnd()
        {
            MT.Stop();
        }
        public override void Start()
        {
            FormEx.Run(delegate () {
                while (IsAllRun)
                {
                    try
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
                                if (GameObj.Enemies.Count > 0)
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
                                /*if (!HasUnCheated)
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
                                }*/
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
                    catch
                    { }
                    Thread.Sleep(CheckInterval);
                }
            });
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
            GameObj.Flush(PalHandle, PID);
        }

        public override string GetAAction()
        {
            return "";
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
                return "仙剑3";
            }
            else
            {
                return "等待游戏运行";
            }
        }

        public override string GetMainWatch()
        {
            return MT.ToString();
        }

        public override string GetMoreInfo()
        {
            return "雪" + GameObj.XJfv + " 葵" + GameObj.LKfv + " 萱" + GameObj.ZXfv + " " + SelectSword + EndName;
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

        public override void OnFunctionKey(int FunNo, NewForm form)
        {
            switch (FunNo)
            {
                case 8:
                    BtnPause_Click(null, null);
                    break;
                case 12:
                    DebugForm df = new DebugForm();
                    df.ShowData(GameObj);
                    df.Show();
                    break;
            }
        }

        public override void Reset()
        {
            HandPauseCount = 0;
            HasAlertMutiPal = false;
            ST.Stop();
            _IsFirstStarted = false;
            BattleLong = new TimeSpan(0);
            InitCheckPoints();
            MT.Reset();
            ST.Reset();
            btnPause.Text = "暂停";
        }

        public override void SetTS(TimeSpan ts)
        {
            MT.SetTS(ts);
            CheckPoints[CurrentStep].Current = ts;
        }

        public override void Unload()
        {
            IsAllRun = false;
        }
    }
    public class Pal3Object
    {
        private const int MapAddr = 0x00C12B48;
        private const int SubMapAddr = 0x00C12B88;
        private const int SmallMapAddr = 0x00C12B68;
        private const int PositionBasePtr = 0x00BFDA68;
        private const int PosFBOffset = 0x18C;
        private const int PosUDOffset = 0x188;
        private const int PosLROffset = 0x184;
        private int[] EnemyIndexAddrs = new int[6] { 0x00C11700, 0x00C11704, 0x00C11708, 0x00C1170C, 0x00C11710, 0x00C11714 };
        private const int BattleBasePtr = 0x00BFDA74;
        private const int BattleBasePtrOffset = 0x10;
        private const int BattleOffset = 0x1394;
        private const int BattleFlagPtrOffset = 0x7c;
        private const int EnemyObjectLength = 0x37C;
        private const int BloodIDOffset = 0x24;
        private const int PacketFirstPtr = 0x00C05F34;
        private const int JTWeaponAddr = 0x00C04730;
        private const int XJfvAddr = 0x00C04338;
        private const int ZXfvAddr = 0x00C04340;
        private const int LKfvAddr = 0x00C0433C;

        public int[] BossIDs = new int[] { 721,737,738,739 };
        //721:邪剑仙 737:邪灵 738:重楼 739:重楼改

        public int BattleEnemySlotAddr = 0x0;
        private IntPtr handle;
        private int PID;

        public string Map = "";
        public string SubMap = "";
        public string SmallMap = "";
        public float FrontBack = 0;
        public float UpDown = 0;
        public float LeftRight = 0;
        public int JTWeapon = 0;
        public int XJfv = 0;
        public int ZXfv = 0;
        public int LKfv = 0;
        public bool IsInBattle = false;
        public int BattleFlag = -1;

        public List<Pal3EnemyObject> Enemies = new List<Pal3EnemyObject>();
        public Dictionary<int, int> Items = new Dictionary<int, int>();
        public int BossID = -1;
        public int BattleTotalBlood = 0;

        public int ItemCount(int ItemID)
        {
            if (Items != null && Items.Count > 0)
            {
                if (Items.ContainsKey(ItemID))
                {
                    return Items[ItemID];
                }
            }
            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MAP:" + this.Map);
            sb.AppendLine("SubMap:" + this.SubMap);
            sb.AppendLine("SmallMap:" + this.SmallMap);
            sb.AppendLine();
            sb.AppendLine("FrontBack:" + this.FrontBack);
            sb.AppendLine("UpDown:" + this.UpDown);
            sb.AppendLine("LeftRight:" + this.LeftRight);
            sb.AppendLine();
            sb.AppendLine("JTWeapon:" + this.JTWeapon);
            sb.AppendLine();
            sb.AppendLine(this.IsInBattle ? "战斗中" : "战斗外");
            sb.AppendLine();
            sb.AppendLine("Enemys: " + this.BattleTotalBlood);
            for (int i = 0; i < Enemies.Count; ++i)
            {
                sb.AppendLine("[" + i + "] ID:" + Enemies[i].ID + " Blood:" + Enemies[i].Blood);
            }
            if (this.BossID != -1)
            {
                sb.AppendLine("BossID:" + this.BossID);
            }

            sb.AppendLine();
            sb.AppendLine("Packet:");
            foreach (KeyValuePair<int, int> kv in Items)
            {
                sb.AppendLine(kv.Key.ToString() + " : " + kv.Value);
            }

            return sb.ToString();
        }

        public void Flush(IntPtr handle, int PID)
        {
            if (PID != this.PID)
            {
                this.PID = PID;
                this.handle = handle;
            }
            this.Map = Readm<string>(this.handle, MapAddr);
            this.SubMap = Readm<string>(this.handle, SubMapAddr);
            this.SmallMap = Readm<string>(this.handle, SmallMapAddr);
            int PositionBaseAddr= Readm<int>(this.handle, PositionBasePtr);
            this.FrontBack = Readm<float>(this.handle, PositionBaseAddr + PosFBOffset);
            this.UpDown = Readm<float>(this.handle, PositionBaseAddr + PosUDOffset);
            this.LeftRight = Readm<float>(this.handle, PositionBaseAddr + PosLROffset);
            this.JTWeapon = Readm<int>(this.handle, JTWeaponAddr);
            this.XJfv = Readm<int>(this.handle, XJfvAddr);
            this.ZXfv = Readm<int>(this.handle, ZXfvAddr);
            this.LKfv = Readm<int>(this.handle, LKfvAddr);

            int battletmpaddr= Readm<int>(this.handle, BattleBasePtr);
            this.BattleEnemySlotAddr = Readm<int>(this.handle, battletmpaddr+ BattleBasePtrOffset);
            this.BattleEnemySlotAddr += BattleOffset;

            int battleflag= Readm<int>(this.handle, battletmpaddr+ BattleFlagPtrOffset);//1战斗中 2战斗胜利逃跑 3战斗失败
            this.BattleFlag = battleflag;
            this.IsInBattle = (battleflag == 1);

            if (this.IsInBattle)
            {
                FlushBattleEnemies();
            }
            else
            {
                BossID = -1;
                BattleTotalBlood = 0;
                Enemies.Clear();
            }
            FlushItems();
        }
        public void FlushItems()
        {
            Dictionary<int, int> tmp = new Dictionary<int, int>();
            int currentaddr = Readm<int>(this.handle, PacketFirstPtr);
            for (int i = 0; i < 1024; ++i, currentaddr += 12)
            {
                int id = Readm<int>(this.handle, currentaddr);
                if (id <= 0) break;
                int count = Readm<int>(this.handle, currentaddr + 4);
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
            int tmpBossID = -1;
            int tmpBattleTotalBlood = 0;
            List<Pal3EnemyObject> tmp = new List<Pal3EnemyObject>();

            int[] eneidx = new int[6] { 0, 0, 0, 0, 0, 0 };
            int totalid = 0;
            for (int i = 0; i < EnemyIndexAddrs.Length; ++i)
            {
                eneidx[i] = Readm<int>(this.handle, EnemyIndexAddrs[i]);
                totalid += eneidx[i];
            }

            if (totalid <= 0)
            {
                BossID = -1;
                BattleTotalBlood = 0;
                Enemies.Clear();
                return;
            }

            int currentaddr = BattleEnemySlotAddr;
            for (int i = 0; i < 6; ++i, currentaddr += Pal3EnemyObject.Length)
            {
                /*if (Readm<int>(handle, currentaddr) <= 0)
                {
                    continue;
                }*/
                Pal3EnemyObject c = new Pal3EnemyObject(handle, currentaddr);
                if (c.Blood > 0)
                {
                    tmpBattleTotalBlood += c.Blood;
                }
                if (ArrayContains<int>(BossIDs, c.ID))
                {
                    tmpBossID = c.ID;
                }
                tmp.Add(c);
            }

            if (tmpBattleTotalBlood <= 0)
            {
                BossID = -1;
                BattleTotalBlood = 0;
                Enemies.Clear();
                return;
            }

            for (int i = 0; i < eneidx.Length; ++i)
            {
                if (eneidx[i] != tmp[i].ID)
                {
                    BossID = -1;
                    BattleTotalBlood = 0;
                    Enemies.Clear();
                    return;
                }
            }

            BossID = tmpBossID;
            BattleTotalBlood = tmpBattleTotalBlood;
            Enemies = tmp;
        }
        public static bool ArrayContains<T>(T[] array, T ele)
        {
            foreach (T e in array)
            {
                if (e.Equals(ele))
                {
                    return true;
                }
            }
            return false;
        }
        public static T Readm<T>(IntPtr handle, int addr)
        {
            T res = default(T);
            Type t = typeof(T);
            int size = 0;
            if (t.Name=="String")
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
                            res=(T)Convert.ChangeType(sb.ToString(), t);
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
    }
    public class Pal3EnemyObject
    {
        public const int Length = 0x37C;
        private const int IDOffset = 0x0;
        private const int BloodOffset = 0x24;

        public int ID;
        public int Blood;
        public Pal3EnemyObject(IntPtr handle, int HeadAddr)
        {
            ID = GameObject.Readm<int>(handle, HeadAddr + IDOffset);
            Blood = GameObject.Readm<int>(handle, HeadAddr + BloodOffset);
        }
    }
}
